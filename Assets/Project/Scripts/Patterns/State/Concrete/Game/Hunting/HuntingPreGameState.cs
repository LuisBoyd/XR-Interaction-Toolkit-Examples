using Project.Scripts.Patterns.State.Concrete.Game.Hunting;
using UnityEngine;

namespace Project.Scripts.Patterns.State.Concrete.Game.Hunting
{
    public class HuntingPreGameState : HuntingGameBaseState
    {
        public override void OnStateStart()
        {
            Debug.Log("HuntingPreGameState, Started");
        }

        public override void OnStateUpdate()
        {
            Debug.Log("HuntingPreGameState, Update");
        }

        public override void OnStateExit()
        {
            Debug.Log("HuntingPreGameState, Exit");
        }

        public override void OnCollision()
        {
            Debug.Log("HuntingPreGameState, Collided");
        }

        public HuntingPreGameState(HuntingStateMachine stateMachine) : base(stateMachine)
        {
        }
    }
}