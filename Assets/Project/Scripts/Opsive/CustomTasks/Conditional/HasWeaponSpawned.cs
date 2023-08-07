using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Project.Scripts.Opsive.CustomTasks.Conditional
{
    public class HasWeaponSpawned : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        public SharedBool hasWeaponSpawned;

        public override TaskStatus OnUpdate()
        {
            return hasWeaponSpawned.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}