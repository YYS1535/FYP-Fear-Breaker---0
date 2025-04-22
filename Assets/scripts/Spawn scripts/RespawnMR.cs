using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnMR : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public float fallThreshold = -1f;
    public float checkInterval = 1f; // Check every 1 second

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Start periodic checking
        InvokeRepeating(nameof(CheckIfFallen), checkInterval, checkInterval);
    }

    void CheckIfFallen()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // Reset position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Optional: Reset velocity if Rigidbody exists
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
