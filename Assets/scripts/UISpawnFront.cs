using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawnFront : MonoBehaviour
{
    public GameObject uiPrefab; // Assign your UI prefab
    public float spawnDistance = 1.5f; // Distance in front of the user
    public float recenterAngleThreshold = 60f; // Angle threshold for recentering
    public float recenterSpeed = 2f; // Speed at which UI moves to new position

    private GameObject spawnedUI;
    private Transform cameraTransform;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        SpawnUI();
    }

    void Update()
    {
        if (spawnedUI != null)
        {
            CheckAndRecenterUI();
            MoveUIToTarget(); // Smoothly move UI instead of teleporting
        }
    }

    void SpawnUI()
    {
        if (cameraTransform != null && uiPrefab != null)
        {
            targetPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;
            targetRotation = Quaternion.LookRotation(targetPosition - cameraTransform.position, Vector3.up);

            spawnedUI = Instantiate(uiPrefab, targetPosition, targetRotation);
        }
    }

    void CheckAndRecenterUI()
    {
        Vector3 toUI = (spawnedUI.transform.position - cameraTransform.position).normalized;
        float angleDifference = Vector3.Angle(cameraTransform.forward, toUI);

        if (angleDifference > recenterAngleThreshold)
        {
            Debug.Log("Recentering UI...");
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
        spawnedUI.transform.position = Vector3.Lerp(spawnedUI.transform.position, targetPosition, Time.deltaTime * recenterSpeed);
        spawnedUI.transform.rotation = Quaternion.Slerp(spawnedUI.transform.rotation, targetRotation, Time.deltaTime * recenterSpeed);
    }
}
