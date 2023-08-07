using Project.Scripts.Weapon;
using UnityEngine;

namespace Project.Scripts.Patterns.Factory
{
    public class WeaponFactory : MonoBehaviour, IFactory<Weapon.Weapon>
    {
        [SerializeField] 
        private Weapon.Weapon prefab;
        public Weapon.Weapon Create()
        {
            return Instantiate(prefab);
        }
    }
}