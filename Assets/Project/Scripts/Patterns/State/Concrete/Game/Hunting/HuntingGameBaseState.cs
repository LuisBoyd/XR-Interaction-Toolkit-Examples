namespace Project.Scripts.Patterns.State.Concrete.Game.Hunting
{
    public abstract class HuntingGameBaseState : State
    {
        protected HuntingStateMachine _stateMachine;
        
        protected HuntingGameBaseState(HuntingStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public virtual void OnTargetHit(Target.Target hitTarget){}
    }
}