using UnityEngine;

namespace Project.Scripts.Weapon
{
    public abstract class RangedWeapon : Weapon
    {
        [SerializeField] protected Transform firePoint;
        public abstract void FireProjectile();
    }
}