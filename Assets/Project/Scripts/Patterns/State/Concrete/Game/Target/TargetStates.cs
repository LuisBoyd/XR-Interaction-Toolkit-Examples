using Project.Scripts.Events;
using Project.Scripts.Patterns.Observer;
using UnityEngine;

namespace Project.Scripts.Patterns.State.Concrete.Game.Target
{
    public abstract class TargetBaseState : State
    {
        protected Target _stateMachine;
        
        protected TargetBaseState(Target stateMachine)
        {
            _stateMachine = stateMachine;
        }
    }

    public class TargetPlayingState : TargetBaseState
    {
        public TargetPlayingState(Target stateMachine) : base(stateMachine)
        {
        }

        public override void OnStateStart()
        {
            
        }

        public override void OnStateUpdate()
        {
            if (_stateMachine.HelperTarget != null)
            {
                float verticalDisplacement = _stateMachine.Modifier.GetAmplitude() *
                                             Mathf.Sin(Time.time * _stateMachine.Modifier.GetFrequency());
                // float verticalDisplacement = _modifier.GetAmplitude() * Mathf.Sin(Time.time * _modifier.GetFrequency());
                // transform.RotateAround(_rotateAroundTarget.position, Vector3.up, _modifier.GetSpeed());
                _stateMachine.transform.RotateAround(_stateMachine.HelperTarget.position,
                    Vector3.up, _stateMachine.Modifier.GetSpeed());
                Vector3 newPosition =  _stateMachine.transform.position + new Vector3(0, verticalDisplacement, 0);
                _stateMachine.transform.position =
                    Vector3.Lerp(_stateMachine.transform.position, newPosition, _stateMachine.Modifier.GetAmplitude() * Time.deltaTime);
                _stateMachine.transform.LookAt(_stateMachine.HelperTarget, Vector3.up); // Keeps Target looking at Center Point
            }
        }
        public override void OnStateExit()
        {
            
        }

        public override void OnCollision()
        {
            
        }

        public override void OnRayReceived(GameObject originatorofRay, Vector3 originOfRay, Vector3 directionOfRay, RaycastHit hit)
        {
           EventBus.Instance.Trigger<OnTargetMarkHit>(new OnTargetMarkHit(_stateMachine));
        }
    }

    public class TargetInitState : TargetBaseState
    {
        public TargetInitState(Target stateMachine) : base(stateMachine)
        {
        }

        public override void OnStateStart()
        {
            _stateMachine.transform.LookAt(_stateMachine.HelperTarget, Vector3.up);
        }

        public override void OnStateUpdate()
        {
            
        }

        public override void OnStateExit()
        {
           
        }

        public override void OnCollision()
        {
           
        }
    }

    public class TargetPauseState : TargetBaseState
    {
        public TargetPauseState(Target stateMachine) : base(stateMachine)
        {
        }

        public override void OnStateStart()
        {
            _stateMachine.transform.LookAt(_stateMachine.HelperTarget, Vector3.up);
        }

        public override void OnStateUpdate()
        {
            
        }

        public override void OnStateExit()
        {
            
        }

        public override void OnCollision()
        {
            
        }
    }
}