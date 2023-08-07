using Project.Scripts.Patterns.State.Concrete.Game.Target;
using UnityEngine;

namespace Project.Scripts.Patterns.Factory
{
    public class TargetFactory: MonoBehaviour, IFactory<Target>
    {
        [SerializeField] 
        private Target prefab;
        public Target Create()
        {
            return Instantiate(prefab);
        }
    }
}