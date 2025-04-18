using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;
using UnityEngine;

public class MultiGrabTracker : MonoBehaviour
{
    [System.Serializable]
    public class GrabbableReference
    {
        public string name;
        public GrabInteractable grabInteractable;
        public HandGrabInteractable handGrabInteractable;
        [HideInInspector] public bool grabbed = false;
    }

    public List<GrabbableReference> grabbables;

    private bool taskCompleted = false;

    void Start()
    {
        foreach (var grabbable in grabbables)
        {
            if (grabbable.grabInteractable != null)
            {
                grabbable.grabInteractable.WhenSelectingInteractorAdded.Action += (interactor) => OnGrab(grabbable);
            }

            if (grabbable.handGrabInteractable != null)
            {
                grabbable.handGrabInteractable.WhenSelectingInteractorAdded.Action += (interactor) => OnGrab(grabbable);
            }
        }
    }

    private void OnGrab(GrabbableReference grabbable)
    {
        if (!grabbable.grabbed)
        {
            grabbable.grabbed = true;
            Debug.Log($"Grabbed object: {grabbable.name}");

            if (AllObjectsGrabbed() && !taskCompleted)
            {
                taskCompleted = true;
                TaskListManager.Instance.CompleteTask2();
                Debug.Log("Task 2 completed: All required objects were grabbed!");
            }
        }
    }

    private bool AllObjectsGrabbed()
    {
        foreach (var g in grabbables)
        {
            if (!g.grabbed) return false;
        }
        return true;
    }
}
