using Project.Scripts.Patterns.State.Concrete.Game.Target;

namespace Project.Scripts.Events
{
    public class OnTargetMarkHit
    {

        public OnTargetMarkHit(Target hitTarget)
        {
            this.HitTarget = hitTarget;
        }
        
        public Target HitTarget { get; private set; }
    }
}