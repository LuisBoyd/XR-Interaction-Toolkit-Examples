using System.Linq;
using Project.Scripts.Interfaces;
using Project.Scripts.Patterns.State.Concrete.Game.Hunting;
using UnityEngine;

namespace Project.Scripts.Patterns.State.Concrete.Game.Target
{
    [RequireComponent(typeof(BoxCollider))]
    public class Target : StateMachine, IRaycastReceiver
    {
        private TargetModifier _modifier;
        private TargetInitState _initState;
        private TargetPlayingState _playingState;
        private TargetPauseState _pauseState;

        [SerializeField] 
        private MeshRenderer _centerTargetrenderer;

        public TargetModifier Modifier
        {
            get => _modifier;
            set
            {
                var mat = _centerTargetrenderer.material;
                switch (value)
                {
                    case GreyModifier _:
                        mat.color = Color.gray;
                        break;
                    case GreenModifier _:
                        mat.color = Color.green;
                        break;
                    case OrangeModifier _:
                        mat.color = new Color(255f / 255f, 165f / 255f, 0f / 255f); //orange
                        break;
                    case RedModifier _:
                        mat.color = Color.red;
                        break;
                    case PurpleModifier _:
                        mat.color = Color.magenta;
                        break;
                    case BrownModifier _:
                        mat.color = new Color(160f / 255f, 82f / 255f, 45f / 255f); //brown
                        break;
                }
                _modifier = value;
            }
        }
        
        [SerializeField]private Transform _rotateAroundTarget;

        public Transform HelperTarget
        {
            get => _rotateAroundTarget;
            set => _rotateAroundTarget = value;
        }
        private BoxCollider _collider;
        
        

        protected override void Awake()
        {
            _initState = new TargetInitState(this);
            _playingState = new TargetPlayingState(this);
            _pauseState = new TargetPauseState(this);
            _modifier = new GreyModifier(); //find way to dynamically change modifier
            _collider = GetComponent<BoxCollider>();
            base.Awake();
            graph.AddVerticesAndEdge(new StateTransition(
                _initState, _playingState,
                () => HuntingStateMachine.HasGameStarted
            ));
            graph.AddVerticesAndEdge(new StateTransition(
                _playingState, _pauseState,
                () => HuntingStateMachine.IsGamePaused == true || HuntingStateMachine.IsWeaponPickedUp == false
            ));
            graph.AddVerticesAndEdge(new StateTransition(
                _pauseState, _playingState,
                () => HuntingStateMachine.IsGamePaused == false
            ));
            _CurrentState = graph.Vertices.First();
        }
        


        public void ReceiveRay(GameObject originatorofRay,Vector3 originOfRay, Vector3 directionOfRay, RaycastHit hit)
        {
            Debug.Log("I Hit the Target");
            _CurrentState.OnRayReceived(originatorofRay, originOfRay, directionOfRay, hit);
        }
        public void ReceiveRay(GameObject originatorofRay,Ray receivedRay, RaycastHit hit) =>
            ReceiveRay(originatorofRay,receivedRay.origin, receivedRay.direction, hit);
    }
}