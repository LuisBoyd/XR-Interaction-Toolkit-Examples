using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Scripts.Enums;
using Project.Scripts.Events;
using Project.Scripts.Events.Vr;
using Project.Scripts.Patterns.Observer;
using Project.Scripts.Patterns.Pool;
using Project.Scripts.Weapon;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace Project.Scripts.Patterns.State.Concrete.Game.Hunting
{
    public class HuntingStateMachine : StateMachine
    {
        [SerializeField] 
        private bool SwitchState = false;

        private HuntingPreGameState _preGameState;
        private HuntingInitGameState _initGameState;
        private HuntingPlayingState _playingState;
        private HuntingPausedState _pausedState;
        private HuntingEndState _endState;

        #region Game Values

        //private JObject HuntingGameJsonSettings;
        private MonobehaviourPool<Target.Target> _TargetPool;
        private Target.Target _CurrentTarget;

        public MonobehaviourPool<Target.Target> TargetPool
        {
            get => _TargetPool;
        }
        [SerializeField] 
        private VisualEffect GunSpawnVFx;

        public List<Target.Target> _roamingTargets { get; private set; }

        #endregion
        
        #region GlobalValues
        private GameDifficulty _currentGameDifficulty;
        public GameDifficulty CurrentGameDifficulty
        {
            get => _currentGameDifficulty;
        }

        private bool _IsGameStarting;

        public bool IsGameStarting
        {
            get => _IsGameStarting;
        }

        private static bool _hasGameStarted;
        public static bool HasGameStarted
        {
            get => _hasGameStarted;
            set => _hasGameStarted = value;
        }

        private static bool _isGamePaused;
        public static bool IsGamePaused
        {
            get => _isGamePaused;
        }

        private static bool _IsWeaponPickedUp;

        public static bool IsWeaponPickedUp
        {
            get => _IsWeaponPickedUp;
        }

        private int _wrongMarksHit;

        public int WrongMarksHit
        {
            get => _wrongMarksHit;
        }

        private int _marksHit;

        public int MarksHit
        {
            get => _marksHit;
        }
        
        public bool TargetSelectionActive
        {
            get => CurrentGameDifficulty == GameDifficulty.HARD;
        }
        #endregion
        
        protected override void Awake()
        {
            JObject HuntingGameJsonSettings = null;
            JsonSerializer serializer = new JsonSerializer(); //GET ALL THE JSON SETTINGS
            using (StreamReader streamReader = new StreamReader(string.Join('/', new string[]
                   {
                       Application.dataPath,
                       Constants.HuntingJsonConfigLocation
                   })))
            {
                HuntingGameJsonSettings = JObject.Parse(streamReader.ReadToEnd());
            }
            
            _preGameState = new HuntingPreGameState(this);
            _initGameState = new HuntingInitGameState(this, HuntingGameJsonSettings, GunSpawnVFx);
            _playingState = new HuntingPlayingState(this, HuntingGameJsonSettings);
            _pausedState = new HuntingPausedState(this);
            _endState = new HuntingEndState(this);
            _roamingTargets = new List<Target.Target>();
            base.Awake();

            graph.AddVerticesAndEdge(new StateTransition(
                _preGameState, _initGameState,
                (() => IsGameStarting == true && _currentGameDifficulty != GameDifficulty.NONE)
            )); //only transition to the init state when we set the game to has started
            graph.AddVerticesAndEdge(new StateTransition(
                _initGameState, _playingState,
                () => IsWeaponPickedUp == true
            )); //return true immediately just let the init state do the setup. 
            graph.AddVerticesAndEdge(new StateTransition(
                _playingState, _pausedState,
                () => IsGamePaused == true || IsWeaponPickedUp == false
            )); //From the playing state to the paused state only when Is Paused = true or if the weapon is not picked up.
            graph.AddVerticesAndEdge(new StateTransition(
                _pausedState, _playingState,
                () => IsGamePaused == false && IsWeaponPickedUp == true
            )); //From the paused state to the playing state only when Is Paused = false and the weapon must be in hand.

            _TargetPool = FindObjectOfType<MonobehaviourPool<Target.Target>>();
        }
        protected override void OnEnable()
        {
            EventBus.Instance.Subscribe<OnGameDifficultyChange>(On_GameDifficultyChange);
            EventBus.Instance.Subscribe<GameStartEvent>(On_GameStart);
            EventBus.Instance.Subscribe<OnVrGrabbableObjectDropped>(On_VrObjectDropped);
            EventBus.Instance.Subscribe<OnVrGrabbableObjectPicked>(On_VrObjectPickedUp);
            EventBus.Instance.Subscribe<OnTargetMarkHit>(OnMarkHit);
        }
        
        protected override void OnDisable()
        {
            EventBus.Instance.Unsubscribe<OnGameDifficultyChange>(On_GameDifficultyChange);
            EventBus.Instance.Unsubscribe<GameStartEvent>(On_GameStart);
            EventBus.Instance.Unsubscribe<OnVrGrabbableObjectDropped>(On_VrObjectDropped);
            EventBus.Instance.Unsubscribe<OnVrGrabbableObjectPicked>(On_VrObjectPickedUp);
            EventBus.Instance.Unsubscribe<OnTargetMarkHit>(OnMarkHit);
            base.OnDisable();
        }

        private void On_GameDifficultyChange(OnGameDifficultyChange evnt)
        {
            if (evnt.newDifficulty == evnt.oldDifficulty)
                return;
            _currentGameDifficulty = evnt.newDifficulty;
        }
        private void On_GameStart(GameStartEvent evnt) => _IsGameStarting = true;
        private void On_VrObjectDropped(OnVrGrabbableObjectDropped evnt)
        {
            if (evnt.DroppedObject.TryGetComponent<GunWeapon>(out GunWeapon weapon))
            {
                _IsWeaponPickedUp = false;
            }
        }

        private void On_VrObjectPickedUp(OnVrGrabbableObjectPicked evnt)
        {
            if (evnt.PickedObject.TryGetComponent<GunWeapon>(out GunWeapon weapon))
            {
                _IsWeaponPickedUp = true;
            }
        }
        private void OnMarkHit(OnTargetMarkHit target)
        {
            //Check if it was the current target if not then ignore.
            if (target.HitTarget.Modifier is GreyModifier)
            {
                Debug.Log("Hit Wrong Target");
                _wrongMarksHit++;
                return;
            }
            if (_CurrentState is HuntingGameBaseState huntingGameBaseState)
            {
                huntingGameBaseState.OnTargetHit(target.HitTarget);
            }
            SwitchTargetMarker(target.HitTarget);
        }

        private void SwitchTargetMarker(Target.Target hitTarget)
        {
            //the target hit was not a grey modifier meaning it was somewhat important.
            //Find random Grey ball target
            if (TargetSelectionActive)
            {
                if (hitTarget != _CurrentTarget)
                {
                    Debug.Log("Hit Wrong Target");
                    _wrongMarksHit++;
                    return;
                }
                //Voice mode active so switch the current active target here. well voice.
            }
            Debug.Log("Hit Right Target");
            var randomGrayTargets = _roamingTargets.Where(x => x.Modifier is not GreyModifier);
            Target.Target randomTarget = _roamingTargets.ElementAt(Random.Range(0, randomGrayTargets.Count()));
            var tempModifier1 = hitTarget.Modifier;
            var tempModifier2 = randomTarget.Modifier;
            randomTarget.Modifier = tempModifier1;
            hitTarget.Modifier = tempModifier2;
            
        }

    }
}