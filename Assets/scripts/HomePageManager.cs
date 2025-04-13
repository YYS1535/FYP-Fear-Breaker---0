using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HomePageManager : MonoBehaviour
{
    //SPIDER SCENE
    public void StartSpiderL1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-Spider-L1");
    }
    public void StartSpiderL2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-Spider-L2");
    }
    public void StartSpiderL3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-Spider-L3");
    }
    public void StartSpiderL4()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-Spider-L4");
    }
    public void StartSpiderL5()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-Spider-L5");
    }

    //SNAKE SCENES
    public void StartSnakeL1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-SNAKE-L1");
    }
    public void StartSnakeL2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-SNAKE-L2");
    }
    public void StartSnakeL3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-SNAKE-L3");
    }
    public void StartSnakeL4()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-SNAKE-L4");
    }
    public void StartSnakeL5()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-SNAKE-L5");
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
