using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTracker : MonoBehaviour
{
    public static InteractionTracker Instance;

    private List<float> interactionTimes = new List<float>();
    private int interactionCount = 0;
    private float sessionStartTime;

    void Awake()
    {
        Instance = this;
    }

    public void StartTracking()
    {
        sessionStartTime = Time.time;
        interactionTimes.Clear();
        interactionCount = 0;
    }

    public void RegisterInteraction()
    {
        float interactionTime = Time.time - sessionStartTime;
        interactionTimes.Add(interactionTime);
        interactionCount++;
    }

    public List<float> GetInteractionTimes() => interactionTimes;
    public int GetInteractionCount() => interactionCount;
}
