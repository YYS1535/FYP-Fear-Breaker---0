using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Oculus.Interaction.Context;

public class TaskListManager : MonoBehaviour
{
    private TaskType activeTasks = TaskType.WatchVideo | TaskType.GrabObject | TaskType.StayInRoom;
    public static TaskListManager Instance;
    public GameObject task1Text, task2Text, task3Text;
    public GameObject doneButton;
    public float requiredTimeInRoom = 60f;

    private float timeInRoom = 0f;
    private bool task1Complete = false;//watch video
    private bool task2Complete = false;//grab photo
    private bool task3Complete = false;//stay for certain time
    private bool trackingTime = false;

    void Awake()
    {
        Instance = this;
    }

    // Call this when the Task Session starts
    public void InitializeTaskSession(TaskType tasksToActivate)
    {
        activeTasks = tasksToActivate;

        doneButton.SetActive(false);
        timeInRoom = 0f;
        trackingTime = true;

        // Reset colors and flags
        task1Complete = task2Complete = task3Complete = false;

        task1Text.SetActive((activeTasks & TaskType.WatchVideo) != 0);
        task2Text.SetActive((activeTasks & TaskType.GrabObject) != 0);
        task3Text.SetActive((activeTasks & TaskType.StayInRoom) != 0);
    }

    void Update()
    {
        if (!trackingTime || (activeTasks & TaskType.StayInRoom) == 0) return;

        timeInRoom += Time.deltaTime;

        if (!task3Complete && timeInRoom >= requiredTimeInRoom)
        {
            task3Complete = true;
            task3Text.GetComponent<TextMeshProUGUI>().color = Color.green;
            CheckAllTasksComplete();
        }
    }

    public void CompleteTask1() // Call when video is watched
    {
        if (!task1Complete && (activeTasks & TaskType.WatchVideo) != 0)
        {
            task1Complete = true;
            task1Text.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            CheckAllTasksComplete();
        }
    }

    public void CompleteTask2() // Call when user grabs and looks at a photo
    {
        if (!task2Complete && (activeTasks & TaskType.GrabObject) != 0)
        {
            task2Complete = true;
            task2Text.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            CheckAllTasksComplete();
        }
    }

    void CheckAllTasksComplete()
    {
        bool allComplete = true;

        if ((activeTasks & TaskType.WatchVideo) != 0) allComplete &= task1Complete;
        if ((activeTasks & TaskType.GrabObject) != 0) allComplete &= task2Complete;
        if ((activeTasks & TaskType.StayInRoom) != 0) allComplete &= task3Complete;

        if (allComplete)
        {
            doneButton.SetActive(true);
        }
    }
}
