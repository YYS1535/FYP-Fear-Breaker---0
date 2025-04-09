using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositionManager : MonoBehaviour
{
    public float spawnDistance = 1.5f;
    public float recenterAngleThreshold = 60f;
    public float recenterSpeed = 2f;
    public Vector3 viewOffset = Vector3.zero; // Custom offset for each UI panel

    private Transform cameraTransform;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        PositionUI(true); // Position immediately on start
    }

    void Update()
    {
        CheckAndRecenterUI();
        MoveUIToTarget();
    }

    void CheckAndRecenterUI()
    {
        Vector3 toUI = (transform.position - cameraTransform.position).normalized;
        float angleDifference = Vector3.Angle(cameraTransform.forward, toUI);

        if (angleDifference > recenterAngleThreshold)
        {
            PositionUI(false);
        }
    }

    void PositionUI(bool immediate)
    {
        Vector3 basePosition = cameraTransform.position + cameraTransform.forward * spawnDistance;
        targetPosition = basePosition + cameraTransform.TransformDirection(viewOffset);
        targetRotation = Quaternion.LookRotation(targetPosition - cameraTransform.position, Vector3.up);

        if (immediate)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }

    void MoveUIToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * recenterSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * recenterSpeed);
    }
}
