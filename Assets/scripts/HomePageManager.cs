using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HomePageManager : MonoBehaviour
{
    public void StartApp()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MR-Spider-L1");
    }
}
