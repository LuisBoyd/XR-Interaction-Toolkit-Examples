using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Project.Scripts.Events;
using Project.Scripts.Game.Models.Hunting;
using Project.Scripts.Patterns.Observer;
using UnityEngine;

namespace Project.Scripts.Patterns.State.Concrete.Game.Hunting
{
    public class HuntingPlayingState : HuntingGameBaseState
    {
        private List<HuntingGameSettings> _gameSettings;
        private bool _HasAlreadyBeenVisited = false;

        private HuntingGameSettings currentGameSetting;
        private float gameTimeLeft;
        private bool switchTargetsEnabled;
        private float switchTimer;
        
        public HuntingPlayingState(HuntingStateMachine stateMachine, JObject jsonSettings) : base(stateMachine)
        {
            _HasAlreadyBeenVisited = false;
            _gameSettings = new List<HuntingGameSettings>();
            IList<JToken> gameSettings = jsonSettings["HuntingGameSettings"].Children().ToList();
            foreach (JToken gameSetting in gameSettings)
            {
                _gameSettings.Add(gameSetting.ToObject<HuntingGameSettings>());
            }
        }

        public override void OnStateStart()
        {
            if (!_HasAlreadyBeenVisited)
            {
                currentGameSetting =
                    _gameSettings.First(x => x.gamemodedifficulty == _stateMachine.CurrentGameDifficulty);
                gameTimeLeft = currentGameSetting.gamelifetime;
                switchTargetsEnabled = currentGameSetting.targetSwitchTime > 0.0f ? true : false;
                switchTimer = currentGameSetting.targetSwitchTime;
                _HasAlreadyBeenVisited = true;
            }
        }

        public override void OnStateUpdate()
        {
            if (gameTimeLeft <= 0.0f)
                EventBus.Instance.Trigger<GameEndEvent>(new GameEndEvent());
            //otherwise we still have time left.
            gameTimeLeft -= Time.deltaTime;
            if (switchTargetsEnabled)
            {
                if (switchTimer <= 0.0f)
                {
                    //Trigger a switch
                    switchTimer = currentGameSetting.targetSwitchTime;
                }
                else
                {
                    switchTimer -= Time.deltaTime;
                }
            }

        }

        public override void OnStateExit()
        {
            
        }

        public override void OnCollision()
        {
            
        }

        public override void OnTargetHit(Target.Target hitTarget)
        {
            //Here we assume the target was the correct target to be hit.
            if (switchTargetsEnabled)
            {
                switchTimer = currentGameSetting.targetSwitchTime;
            }
            //Instead of destroying the target just play the broke VFX disable it for a couple of seconds (everything but the transform) then re-eneable
        }
    }
}