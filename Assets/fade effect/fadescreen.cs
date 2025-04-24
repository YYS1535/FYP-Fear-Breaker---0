using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadescreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;
    private Renderer rend;
    public GameObject gameObject;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (fadeOnStart)
            FadeIn();
    }

    public void FadeIn()
    {
        Fade(1, 0);
        gameObject.SetActive(false);
    }

    public void FadeOut()
    {
        Fade(0, 1);
        gameObject.SetActive(false);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            rend.material.SetColor("_Color", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        // Final color set
        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
        rend.material.SetColor("_Color", newColor2);
    }
}
