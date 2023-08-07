using Project.Scripts.Patterns.Factory;
using Project.Scripts.Patterns.State.Concrete.Game.Target;
using UnityEngine;

namespace Project.Scripts.Patterns.Pool
{
    [RequireComponent(typeof(TargetFactory))]
    public class TargetPool : MonobehaviourPool<Target>
    {
    }
}