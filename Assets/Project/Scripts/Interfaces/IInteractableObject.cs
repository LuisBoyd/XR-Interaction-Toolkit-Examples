using UnityEngine;

namespace Project.Scripts.Interfaces
{
    public interface IInteractableObject
    {
        GameObject GameObject { get; }
        bool IsTouchable { set; }
        bool IsGrabbable { set; }
        bool IsUsable { set; }
    }
}