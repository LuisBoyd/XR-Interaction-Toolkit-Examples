using QuikGraph;
using UnityEngine;

namespace Project.Scripts.Patterns.State
{
    public abstract class State
    {
        public abstract void OnStateStart();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();
        public abstract void OnCollision();
        
        public virtual void OnRayReceived(GameObject originatorofRay,Vector3 originOfRay, Vector3 directionOfRay, RaycastHit hit){}

        public void OnRayReceived(GameObject originatorofRay, Ray receivedRay, RaycastHit hit)
            => OnRayReceived(originatorofRay, receivedRay.origin, receivedRay.direction, hit);
    }
}