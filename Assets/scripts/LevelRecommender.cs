using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRecommender : MonoBehaviour
{
    public enum FearType { Spider, Snake, Holes, Height }
    public FearType currentFear;

    // Sliders
    public Slider question1Slider;
    public Slider question2Slider;
    public Slider question3Slider;

    // Panels
    public GameObject question1Panel;
    public GameObject question2Panel;
    public GameObject question3Panel;
    public GameObject resultPanel;

    // Result text
    public TMP_Text resultText;

    private int q1, q2, q3;

    public void OnSliderQ1Changed() => q1 = Mathf.RoundToInt(question1Slider.value);
    public void OnSliderQ2Changed() => q2 = Mathf.RoundToInt(question2Slider.value);
    public void OnSliderQ3Changed() => q3 = Mathf.RoundToInt(question3Slider.value);

    public void GoToQuestion2()
    {
        question1Panel.SetActive(false);
        question2Panel.SetActive(true);
    }

    public void GoToQuestion3()
    {
        question2Panel.SetActive(false);
        question3Panel.SetActive(true);
    }

    public void GoToQuestion1()
    {
        question2Panel.SetActive(false);
        question1Panel.SetActive(true);
    }

    public void GoToQuestion2Back()
    {
        question3Panel.SetActive(false);
        question2Panel.SetActive(true);
    }

    public void SubmitAnswers()
    {
        question3Panel.SetActive(false);
        resultPanel.SetActive(true);

        int recommendedLevel = 1;

        switch (currentFear)
        {
            case FearType.Spider:
            case FearType.Snake:
                // 5 level logic
                if (q1 <= 3) recommendedLevel = 2;
                if (q2 <= 3) recommendedLevel = 3;
                if (q3 >= 3) recommendedLevel = 4;
                if (q3 == 5) recommendedLevel = 5;
                break;

            case FearType.Holes:
                if (q1 <= 3) recommendedLevel = 2;
                if (q2 <= 3) recommendedLevel = 2;
                if (q2 <= 3 || q3 >= 3) recommendedLevel = 3;
                break;

            case FearType.Height:
                // Similar 3 level logic
                if (q1 <= 3) recommendedLevel = 2;
                if (q2 <= 3) recommendedLevel = 2;
                if (q2 <= 3 || q3 >= 4) recommendedLevel = 3;
                break;
        }

        resultText.text = "Level " + recommendedLevel;
    }
}
