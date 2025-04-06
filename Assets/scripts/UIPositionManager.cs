using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositionManager : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        UpdatePosition();
    }

    void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector3 targetPos = cameraTransform.position + cameraTransform.rotation * offset;
        Quaternion targetRot = Quaternion.LookRotation(targetPos - cameraTransform.position);
        transform.position = targetPos;
        transform.rotation = targetRot;
    }
}
