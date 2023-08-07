using Project.Scripts.Patterns.Factory;
using UnityEngine;

namespace Project.Scripts.Patterns.Pool
{
    [RequireComponent(typeof(WeaponFactory))]
    public class WeaponPool : MonobehaviourPool<Weapon.Weapon>
    {
        
    }
}