using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawnFront : MonoBehaviour
{
    public GameObject uiPanel; // Reference your UI Panel in the scene
    public float spawnDistance = 1.5f;
    public float recenterAngleThreshold = 60f;
    public float recenterSpeed = 2f;

    private Transform cameraTransform;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        if (uiPanel != null)
        {
            PositionUI(); // Move the existing UI to the correct position
        }
    }

    void Update()
    {
        if (uiPanel != null)
        {
            CheckAndRecenterUI();
            MoveUIToTarget();
        }
    }

    void PositionUI()
    {
        targetPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;
        targetRotation = Quaternion.LookRotation(targetPosition - cameraTransform.position, Vector3.up);

        uiPanel.transform.position = targetPosition;
        uiPanel.transform.rotation = targetRotation;
    }

    void CheckAndRecenterUI()
    {
        Vector3 toUI = (uiPanel.transform.position - cameraTransform.position).normalized;
        float angleDifference = Vector3.Angle(cameraTransform.forward, toUI);

        if (angleDifference > recenterAngleThreshold)
        {
            SetNewUIPosition();
        }
    }

    void SetNewUIPosition()
    {
        targetPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;
        targetRotation = Quaternion.LookRotation(targetPosition - cameraTransform.position, Vector3.up);
    }

    void MoveUIToTarget()
    {
        uiPanel.transform.position = Vector3.Lerp(uiPanel.transform.position, targetPosition, Time.deltaTime * recenterSpeed);
        uiPanel.transform.rotation = Quaternion.Slerp(uiPanel.transform.rotation, targetRotation, Time.deltaTime * recenterSpeed);
    }

    // PUBLIC FUNCTION TO DISABLE UI
    public void HideUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }
}
