using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Events;
using Project.Scripts.Events.Vr;
using Project.Scripts.Patterns.Observer;
using Project.Scripts.Patterns.Singelton;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Project.Scripts.Manager
{
    public class XRObjectTracker : MonobehaviourSingleton<XRObjectTracker>
    {
        private XRBaseInteractor[] _baseInteractors;

        [SerializeField] private float TimeBeforeEventFired = 3.0f;
        [SerializeField] private bool WaitForEvent = false;
        
        private void CheckAndValidateInteractors()
        {
            bool allValid = _baseInteractors?.All(x => x != null) ?? false;
            if (!allValid)
            {
                _baseInteractors =
                    FindObjectsByType<XRBaseInteractor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                foreach (XRBaseInteractor baseInteractor in _baseInteractors)
                {
                    baseInteractor.onSelectExited.AddListener(On_ObjectDropped);
                    baseInteractor.onSelectEntered.AddListener(On_ObjectPickedUp);
                }
            }
        }
        private void Update()
        {
            CheckAndValidateInteractors();
        }

        private void OnDisable()
        {
            foreach (XRBaseInteractor baseInteractor in _baseInteractors)
            {
                baseInteractor.onSelectExited.RemoveListener(On_ObjectDropped);
                baseInteractor.onSelectEntered.RemoveListener(On_ObjectPickedUp);
            }
        }

        private void On_ObjectDropped(XRBaseInteractable arg)
        {
            if(arg is not XRGrabInteractable)
                return;
            if (WaitForEvent)
            {
                StartCoroutine(WaitThenFire_Dropped(arg));
            }
            else
            {
                EventBus.Instance.Trigger<OnVrGrabbableObjectDropped>(new OnVrGrabbableObjectDropped((XRGrabInteractable) arg));
            }
        }

        private void On_ObjectPickedUp(XRBaseInteractable arg)
        {
            if(arg is not XRGrabInteractable)
                return;
            if (WaitForEvent)
            {
                StartCoroutine(WaitThenFire_Picked(arg));
            }
            else
            {
                EventBus.Instance.Trigger<OnVrGrabbableObjectPicked>(new OnVrGrabbableObjectPicked(arg as XRGrabInteractable));
            }
        }
        private IEnumerator WaitThenFire_Dropped(XRBaseInteractable arg)
        {
            yield return new WaitForSeconds(TimeBeforeEventFired);
            if (arg.isSelected)
                yield return null;
            EventBus.Instance.Trigger<OnVrGrabbableObjectDropped>(new OnVrGrabbableObjectDropped(arg as XRGrabInteractable));
        }
        
        private IEnumerator WaitThenFire_Picked(XRBaseInteractable arg)
        {
            yield return new WaitForSeconds(TimeBeforeEventFired);
            if (arg.isSelected)
                yield return null;
            EventBus.Instance.Trigger<OnVrGrabbableObjectPicked>(new OnVrGrabbableObjectPicked(arg as XRGrabInteractable));
        }
    }
}