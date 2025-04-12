using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance;

    public TMP_Text touchCountText;
    public TMP_Text totalTouchTimeText;
    public TMP_Text sessionDurationText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowResults()
    {
        GrabbableInteractionTracker[] photoTrackers = FindObjectsOfType<GrabbableInteractionTracker>();
        int totalTouch = 0;
        float totalTouchTime = 0f;

        foreach (var tracker in photoTrackers)
        {
            totalTouch += tracker.GetTouchCount();
            totalTouchTime += tracker.GetTotalTouchDuration();
        }

        // Get accurate session time from SessionManager
        float sessionTime = SessionManager.Instance.GetTotalSessionTime();

        int minutes = Mathf.FloorToInt(sessionTime / 60f);
        int seconds = Mathf.FloorToInt(sessionTime % 60f);
        string formattedTime = $"{minutes}:{seconds:00}";

        touchCountText.text = totalTouch.ToString() + "Times";
        totalTouchTimeText.text = totalTouchTime.ToString("F1") + "s";
        sessionDurationText.text = formattedTime;
    }

    public void ConfirmResult()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home Page");
    }
}
