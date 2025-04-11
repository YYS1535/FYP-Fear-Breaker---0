using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;
    public GameObject timedUI, taskUI, quitUI, resultUI, video1, video2;
    public float timedSessionDuration = 90f;
    public float taskSessionDuration = 60f;
    public spawnOnTable objectSpawner;

    private float countdownTimer;     // for timed session
    private float taskElapsedTime = 0f;   // for task session
    public float totalSessionTime = 0f;  // for both sessions and result

    private bool isTimedSession, isTaskSession;
    private bool sessionRunning;

    public TextMeshProUGUI timerText;
    public Slider taskProgressBar;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        sessionRunning = false;
        resultUI.SetActive(false);
        timedUI.SetActive(false);
        taskUI.SetActive(false);
        quitUI.SetActive(false);
        if (video1 != null) video1.SetActive(false);
        if (video2 != null) video2.SetActive(false);

        if (taskProgressBar != null)
        {
            taskProgressBar.minValue = 0f;
            taskProgressBar.maxValue = taskSessionDuration;
            taskProgressBar.value = 0f;
        }
    }

    public void StartTimedSession()
    {
        isTimedSession = true;
        countdownTimer = timedSessionDuration;
        totalSessionTime = 0f;

        timedUI.SetActive(true);
        quitUI.SetActive(true);
        if (video1 != null) video1.SetActive(true);
        if (video2 != null) video2.SetActive(true);

        sessionRunning = true;
        objectSpawner.TrySpawn();

        StartCoroutine(TimedSessionTimer());

        if (isTimedSession && isTaskSession)
            Debug.LogWarning("Both sessions active! This should not happen.");
    }

    public void StartTaskSession()
    {
        isTaskSession = true;
        taskElapsedTime = 0f;
        totalSessionTime = 0f;

        taskUI.SetActive(true);
        quitUI.SetActive(true);
        if (video1 != null) video1.SetActive(true);
        if (video2 != null) video2.SetActive(true);

        sessionRunning = true;
        TaskListManager.Instance.InitializeTaskSession();

        StartCoroutine(TaskSessionTimer());

        if (isTimedSession && isTaskSession)
            Debug.LogWarning("Both sessions active! This should not happen.");
    }

    IEnumerator TimedSessionTimer()
    {
        while (sessionRunning && countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            totalSessionTime += Time.deltaTime;

            UpdateTimerText(countdownTimer); // countdown format

            yield return null;
        }

        if (isTimedSession)
            EndSession();
    }

    IEnumerator TaskSessionTimer()
    {
        while (sessionRunning)
        {
            taskElapsedTime += Time.deltaTime;
            totalSessionTime += Time.deltaTime;

            if (taskElapsedTime <= taskSessionDuration && taskProgressBar != null)
            {
                taskProgressBar.value = taskElapsedTime;
            }

            UpdateTimerText(totalSessionTime); // count-up format

            yield return null;
        }
    }

    void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minutes}:{seconds:00}";
    }

    public void EndSession()
    {
        sessionRunning = false;
        timedUI.SetActive(false);
        taskUI.SetActive(false);
        quitUI.SetActive(false);
        resultUI.SetActive(true);

        if (video1 != null) video1.SetActive(false);
        if (video2 != null) video2.SetActive(false);

        ResultManager.Instance.ShowResults();
    }

    public void EarlyQuit()
    {
        sessionRunning = false;
        timedUI.SetActive(false);
        taskUI.SetActive(false);
        quitUI.SetActive(false);
        resultUI.SetActive(false);

        if (video1 != null) video1.SetActive(false);
        if (video2 != null) video2.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Home Page");
    }

    public float GetTotalSessionTime() => totalSessionTime;
}
