using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoTaskTracker : MonoBehaviour
{
    public VideoPlayer[] videoPlayers;

    private bool task1Reported = false;

    void Start()
    {
        if (videoPlayers != null && videoPlayers.Length > 0)
        {
            foreach (VideoPlayer vp in videoPlayers)
            {
                if (vp != null)
                {
                    vp.loopPointReached += OnVideoFinished;
                }
            }
        }
        else
        {
            Debug.LogWarning("No VideoPlayers assigned in VideoTaskTracker.");
        }
    }

    void OnDestroy()
    {
        if (videoPlayers != null)
        {
            foreach (VideoPlayer vp in videoPlayers)
            {
                if (vp != null)
                {
                    vp.loopPointReached -= OnVideoFinished;
                }
            }
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (!task1Reported)
        {
            task1Reported = true;
            Debug.Log("Video finished. Marking Task 1 complete.");
            TaskListManager.Instance.CompleteTask1();
        }
    }
}
