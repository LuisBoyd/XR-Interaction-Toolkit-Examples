using Project.Scripts.Patterns.Pool;
using UnityEngine;
using UnityEngine.VFX;

namespace Project.Scripts.Opsive.CustomTasks.Action
{
    using BehaviorDesigner.Runtime.Tasks;
    
    public class SpawnWeapon : Action
    {
        public Transform spawnTransform;
        public bool SetParent = false;
        public bool useVFX = false;
        public VisualEffect VisualEffect;
        public MonobehaviourPool<Weapon.Weapon> _WeaponPool;

        public override TaskStatus OnUpdate()
        {
            if (useVFX)
            {
                VisualEffect.Play();
                return TaskStatus.Success;
            }
            var weapon = _WeaponPool.Get();
            if (SetParent)
            {
                weapon.transform.parent = spawnTransform;
                weapon.transform.localPosition = Vector3.zero;
            }
            else
            {
                weapon.transform.position = spawnTransform.position;
            }
            weapon.gameObject.SetActive(true);
            return TaskStatus.Success;
        }
    }
}