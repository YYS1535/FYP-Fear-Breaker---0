using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HomePageManager : MonoBehaviour
{
    public void StartSpiderL1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-Spider-L1");
    }

    public void QuitApp()
{
    Debug.Log("Attempting to quit the app...");
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
}
}
