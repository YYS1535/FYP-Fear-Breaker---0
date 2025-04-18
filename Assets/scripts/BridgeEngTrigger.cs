using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeEngTrigger : MonoBehaviour
{
    private bool task1Completed = false;

    private void OnTriggerEnter(Collider other)
    {
        // Make sure only the player triggers this
        if (!task1Completed && other.CompareTag("Player"))
        {
            task1Completed = true;
            TaskListManager.Instance.CompleteTask1();
            Debug.Log("Task 1 completed: User crossed the bridge!");
        }
    }
}
