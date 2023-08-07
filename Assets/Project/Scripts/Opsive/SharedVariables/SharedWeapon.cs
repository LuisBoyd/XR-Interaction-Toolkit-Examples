using BehaviorDesigner.Runtime;

namespace Project.Scripts.Opsive.SharedVariables
{
    public class SharedWeapon : SharedVariable<Weapon.Weapon>
    {
        public static implicit operator SharedWeapon(Weapon.Weapon value)
        {
            return new SharedWeapon {Value = value};
        }
    }
}