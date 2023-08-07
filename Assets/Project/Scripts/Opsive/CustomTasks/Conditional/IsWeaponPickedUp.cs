using BehaviorDesigner.Runtime;
using Project.Scripts.Opsive.SharedVariables;

namespace Project.Scripts.Opsive.CustomTasks.Conditional
{
    using BehaviorDesigner.Runtime.Tasks;
    public class IsWeaponPickedUp : Conditional
    {
        public SharedWeapon SharedWeapon;

        public override TaskStatus OnUpdate()
        {
            return SharedWeapon.Value == null ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}