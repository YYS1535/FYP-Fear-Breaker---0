using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Oculus.Interaction.Context;

public class TaskListManager : MonoBehaviour
{
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
    public void InitializeTaskSession()
    {
        doneButton.SetActive(false);
        timeInRoom = 0f;
        trackingTime = true;
    }

    void Update()
    {
        if (!trackingTime) return;

        timeInRoom += Time.deltaTime;

        // Check if time condition is met
        if (!task3Complete && timeInRoom >= requiredTimeInRoom)
        {
            task3Complete = true;
            task3Text.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            CheckAllTasksComplete();
        }
    }

    public void CompleteTask1() // Call when video is watched
    {
        if (!task1Complete)
        {
            task1Complete = true;
            task1Text.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            CheckAllTasksComplete();
        }
    }

    public void CompleteTask2() // Call when user grabs and looks at a photo
    {
        if (!task2Complete)
        {
            task2Complete = true;
            task2Text.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            CheckAllTasksComplete();
        }
    }

    void CheckAllTasksComplete()
    {
        if (task1Complete && task2Complete && task3Complete)
        {
            doneButton.SetActive(true);
        }
    }
}
