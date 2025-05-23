using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GrabbableInteractionTracker : MonoBehaviour
{
    public GrabInteractable grabInteractable;
    public HandGrabInteractable handGrabInteractable;

    [Header("Optional Audio Settings")]
    public bool enableGrabSound = false;
    public AudioSource grabAudioSource;
    public AudioClip grabSound;

    private float grabStartTime;
    private bool isGrabbed = false;

    private float totalTouchDuration = 0f;
    private int touchCount = 0;

    private bool task2Completed = false;

    void Start()
    {
        if (grabInteractable != null)
        {
            grabInteractable.WhenSelectingInteractorAdded.Action += OnGrab;
            grabInteractable.WhenSelectingInteractorRemoved.Action += OnRelease;
        }

        if (handGrabInteractable != null)
        {
            handGrabInteractable.WhenSelectingInteractorAdded.Action += OnGrab;
            handGrabInteractable.WhenSelectingInteractorRemoved.Action += OnRelease;
        }
    }

    private void OnGrab(IInteractorView interactor)
    {
        if (!isGrabbed)
        {
            grabStartTime = Time.time;
            touchCount++;
            isGrabbed = true;

            if (SceneManager.GetActiveScene().name != "VR-height-Level 3" && task2Completed == false)
            {
                TaskListManager.Instance.CompleteTask2();
                task2Completed = true;
                Debug.Log("Task 2 completed from OnGrab");
            }

            // Play cute sound if enabled
            if (enableGrabSound && grabAudioSource != null && grabSound != null)
            {
                grabAudioSource.PlayOneShot(grabSound);
            }

            Debug.Log("Grab started");
        }
    }

    private void OnRelease(IInteractorView interactor)
    {
        if (isGrabbed)
        {
            float duration = Time.time - grabStartTime;
            totalTouchDuration += duration;

            Debug.Log($"Grab ended after {duration:F2}s");

            isGrabbed = false;
        }
    }

    // Public getters
    public int GetTouchCount() => touchCount;
    public float GetTotalTouchDuration() => totalTouchDuration;
}
