using UnityEngine;

namespace Project.Scripts.Interfaces
{
    public interface IRaycastReceiver
    {
        void ReceiveRay(GameObject originatorofRay,Vector3 originOfRay, Vector3 directionOfRay, RaycastHit hit);
        void ReceiveRay(GameObject originatorofRay,Ray receivedRay, RaycastHit hit);
    }
}