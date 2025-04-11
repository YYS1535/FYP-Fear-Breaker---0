using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

public class TriggerHapticOnHover : MonoBehaviour
{
    [Range(0,2.5f)]
    public float duration;
    [Range(0,1)]
    public float amplitude;
    [Range (0,1)]
    public float frequency;
    

    public GrabInteractable grabInteractable;

    // Start is called before the first frame update
    void Start()
    {
        grabInteractable.WhenSelectingInteractorAdded.Action += WhenSelectingInteractorAdded_Action;
    }

    private void WhenSelectingInteractorAdded_Action(GrabInteractor obj)
    {
        ControllerRef controllerRef = obj.GetComponent<ControllerRef>();
        if (controllerRef)
        {
            if (controllerRef.Handedness == Handedness.Right)
                TriggerHaptics(OVRInput.Controller.RTouch);
            else
                TriggerHaptics(OVRInput.Controller.LTouch);
        }
    }

    public void TriggerHaptics(OVRInput.Controller controller)
    {
        StartCoroutine(TriggerHapticsRoutine(controller));
    }

    public IEnumerator TriggerHapticsRoutine(OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, controller);

    }
}
