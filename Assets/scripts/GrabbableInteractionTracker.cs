using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableInteractionTracker : MonoBehaviour
{
    private float grabStartTime = 0f;
    private float totalGrabDuration = 0f;
    private int grabCount = 0;
    private bool isBeingGrabbed = false;
    private bool task2Reported = false; // ensure we call CompleteTask2 only once

    private OVRGrabbable grabbable;

    void Start()
    {
        grabbable = GetComponent<OVRGrabbable>();
    }

    void Update()
    {
        if (grabbable != null)
        {
            if (grabbable.isGrabbed && !isBeingGrabbed)
            {
                // Grab started
                isBeingGrabbed = true;
                grabStartTime = Time.time;
                grabCount++;
            }
            else if (!grabbable.isGrabbed && isBeingGrabbed)
            {
                // Grab ended
                isBeingGrabbed = false;
                float grabDuration = Time.time - grabStartTime;
                totalGrabDuration += grabDuration;

                // Check for task completion (2s total interaction)
                if (!task2Reported && totalGrabDuration >= 2f)
                {
                    TaskListManager.Instance.CompleteTask2();
                    task2Reported = true;
                }
            }
        }
    }

    public int GetGrabCount() => grabCount;
    public float GetTotalGrabDuration() => totalGrabDuration;
}
