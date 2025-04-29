using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSound : MonoBehaviour
{
    private AudioSource audioSource;
    private float timer;
    private float nextSoundTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource found on " + gameObject.name);
        }

        SetNextSoundTime();
    }

    void Update()
    {
        if (audioSource == null)
            return;

        timer += Time.deltaTime;

        if (timer >= nextSoundTime)
        {
            PlaySound();
            SetNextSoundTime();
            timer = 0f;
        }
    }

    void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void SetNextSoundTime()
    {
        nextSoundTime = Random.Range(7f, 15f);
    }
}
