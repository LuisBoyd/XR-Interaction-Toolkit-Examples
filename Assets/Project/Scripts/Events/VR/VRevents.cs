using UnityEngine.XR.Interaction.Toolkit;

namespace Project.Scripts.Events.Vr
{
    public class OnVrGrabbableObjectDropped
    {
        public OnVrGrabbableObjectDropped(XRGrabInteractable droppedObject)
        {
            this.DroppedObject = droppedObject;
        }
        public XRGrabInteractable DroppedObject { get; private set; }
    }
    
    public class OnVrGrabbableObjectPicked
    {
        public OnVrGrabbableObjectPicked(XRGrabInteractable pickedObject)
        {
            this.PickedObject = pickedObject;
        }
        public XRGrabInteractable PickedObject { get; private set; }
    }
}