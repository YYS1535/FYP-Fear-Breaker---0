using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHapticFeedback : MonoBehaviour
{
    [Header("Haptic Settings")]
    [Range(0f, 2.5f)] public float hoverDuration = 0.1f;
    [Range(0f, 1f)] public float hoverAmplitude = 0.1f;
    [Range(0f, 1f)] public float hoverFrequency = 0.5f;

    [Range(0f, 2.5f)] public float clickDuration = 0.2f;
    [Range(0f, 1f)] public float clickAmplitude = 0.4f;
    [Range(0f, 1f)] public float clickFrequency = 0.8f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TriggerHaptics(hoverDuration, hoverAmplitude, hoverFrequency);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TriggerHaptics(clickDuration, clickAmplitude, clickFrequency);
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    void TriggerHaptics(float duration, float amplitude, float frequency)
    {
        // Use both controllers for now — you can adapt this later
        StartCoroutine(HapticsRoutine(OVRInput.Controller.LTouch, duration, amplitude, frequency));
        StartCoroutine(HapticsRoutine(OVRInput.Controller.RTouch, duration, amplitude, frequency));
    }

    IEnumerator HapticsRoutine(OVRInput.Controller controller, float duration, float amplitude, float frequency)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
