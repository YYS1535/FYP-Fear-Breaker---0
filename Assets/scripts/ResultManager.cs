using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance;

    public TextMeshProUGUI resultText;
    public GrabbableInteractionTracker[] photoTrackers; // assign in Inspector

    void Awake()
    {
        Instance = this;
    }

    public void ShowResults()
    {
        int totalGrabs = 0;
        float totalGrabTime = 0f;

        foreach (var tracker in photoTrackers)
        {
            totalGrabs += tracker.GetGrabCount();
            totalGrabTime += tracker.GetTotalGrabDuration();
        }

        float sessionTime = Time.timeSinceLevelLoad;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SESSION RESULTS");
        sb.AppendLine("Touch Count: " + totalGrabs);
        sb.AppendLine("Total Interaction Time: " + totalGrabTime.ToString("F1") + "s");
        sb.AppendLine("Session Duration: " + sessionTime.ToString("F1") + "s");

        resultText.text = sb.ToString();
    }

    public void ConfirmResult()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HomePage");
    }
}
