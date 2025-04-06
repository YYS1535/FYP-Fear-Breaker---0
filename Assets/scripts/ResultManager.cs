using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance;

    public TextMeshProUGUI resultText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowResults()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Session Complete!");
        sb.AppendLine("Interaction Count: " + InteractionTracker.Instance.GetInteractionCount());

        foreach (float t in InteractionTracker.Instance.GetInteractionTimes())
        {
            sb.AppendLine("Touched at: " + t.ToString("F1") + "s");
        }

        resultText.text = sb.ToString();
    }

    public void ConfirmResult()
    {
        InteractionTracker.Instance.StartTracking(); // Reset
        UnityEngine.SceneManagement.SceneManager.LoadScene("HomePage");
    }
}
