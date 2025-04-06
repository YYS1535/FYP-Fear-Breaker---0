using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    public GameObject timedUI, taskUI, quitUI, resultUI;
    public float timedSessionDuration = 90f;
    public float taskSessionDuration = 60f;

    private float sessionTimer;
    private bool isTimedSession, isTaskSession;
    private bool sessionRunning;

    public TextMeshProUGUI timerText;
    public Slider taskProgressBar;

    void Start()
    {
        sessionRunning = false;
        resultUI.SetActive(false);
        timedUI.SetActive(false);
        taskUI.SetActive(false);
        quitUI.SetActive(false);
    }

    public void StartTimedSession()
    {
        isTimedSession = true;
        sessionTimer = timedSessionDuration;
        timedUI.SetActive(true);
        quitUI.SetActive(true);
        sessionRunning = true;
        InteractionTracker.Instance.StartTracking();
        StartCoroutine(SessionTimer());

        if (isTimedSession && isTaskSession)
        {
            Debug.LogWarning("Both sessions active! This should not happen.");
        }
    }

    public void StartTaskSession()
    {
        isTaskSession = true;
        sessionTimer = 0f;
        taskUI.SetActive(true);
        quitUI.SetActive(true);
        sessionRunning = true;
        TaskListManager.Instance.InitializeTaskSession();
        InteractionTracker.Instance.StartTracking();

        if (isTimedSession && isTaskSession)
        {
            Debug.LogWarning("Both sessions active! This should not happen.");
        }
    }

    IEnumerator SessionTimer()
    {
        while (sessionRunning && sessionTimer > 0)
        {
            sessionTimer -= Time.deltaTime;
            if (timerText != null)
                timerText.text = sessionTimer.ToString("F1") + "s";
            if (taskProgressBar != null)
                taskProgressBar.value = 1f - (sessionTimer / taskSessionDuration);

            yield return null;
        }

        if (isTimedSession)
            EndSession();
    }

    public void EndSession()
    {
        sessionRunning = false;
        timedUI.SetActive(false);
        taskUI.SetActive(false);
        quitUI.SetActive(false);
        resultUI.SetActive(true);

        ResultManager.Instance.ShowResults();
    }

    public void EarlyQuit()
    {
        sessionRunning = false;
        timedUI.SetActive(false);
        taskUI.SetActive(false);
        quitUI.SetActive(false);
        resultUI.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("HomePage");
    }


}
