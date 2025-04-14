using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Flags]
public enum TaskType
{
    None = 0,
    WatchVideo = 1 << 0,
    GrabObject = 1 << 1,
    StayInRoom = 1 << 2
}

public class SessionManager : MonoBehaviour
{
    public TaskType levelTaskConfig = TaskType.WatchVideo | TaskType.GrabObject | TaskType.StayInRoom;
    public static SessionManager Instance;
    public GameObject timedUI, taskUI, quitUI, resultUI, video1, video2;
    public float timedSessionDuration = 90f;
    public float taskSessionDuration = 60f;

    private float countdownTimer;     // for timed session
    private float taskElapsedTime = 0f;   // for task session
    public float totalSessionTime = 0f;  // for both sessions and result

    private bool isTimedSession, isTaskSession;
    private bool sessionRunning;

    public AudioSource breathingAudio;
    public AudioSource guidanceAudio1;
    public AudioSource guidanceAudio2;
    public bool muteVoiceGuidance = false; // can be toggled via UI button

    private Coroutine voiceRoutine;
    private string currentSceneName;

    public TextMeshProUGUI timerText;
    public Slider taskProgressBar;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
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

        if (!currentSceneName.Contains("L1") && !muteVoiceGuidance)
        {
            voiceRoutine = StartCoroutine(PlayVoiceGuidance());
        }

        sessionRunning = true;

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

        if (!currentSceneName.Contains("L1") && !muteVoiceGuidance)
        {
            voiceRoutine = StartCoroutine(PlayVoiceGuidance());
        }

        sessionRunning = true;
        TaskListManager.Instance.InitializeTaskSession(levelTaskConfig);

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

    IEnumerator PlayVoiceGuidance()
    {
        yield return new WaitForSeconds(3f);
        if (breathingAudio != null) breathingAudio.Play();

        yield return new WaitForSeconds(62f); // total = 1:05
        if (Random.value < 0.5f)
            if (guidanceAudio1 != null) guidanceAudio1.Play();
            else
            if (guidanceAudio2 != null) guidanceAudio2.Play();

        yield return new WaitForSeconds(55f); // total = 2:00
        if (Random.value < 0.5f)
            if (guidanceAudio1 != null && !guidanceAudio1.isPlaying) guidanceAudio1.Play();
            else
            if (guidanceAudio2 != null && !guidanceAudio2.isPlaying) guidanceAudio2.Play();
    }

    public void ToggleVoiceGuidance(bool isMuted)
    {
        muteVoiceGuidance = isMuted;
    }

    public void EndSession()
    {
        if (voiceRoutine != null) StopCoroutine(voiceRoutine);
        if (breathingAudio != null) breathingAudio.Stop();
        if (guidanceAudio1 != null) guidanceAudio1.Stop();
        if (guidanceAudio2 != null) guidanceAudio2.Stop();
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
        if (voiceRoutine != null) StopCoroutine(voiceRoutine);
        if (breathingAudio != null) breathingAudio.Stop();
        if (guidanceAudio1 != null) guidanceAudio1.Stop();
        if (guidanceAudio2 != null) guidanceAudio2.Stop();
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
