using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAndRespawn : MonoBehaviour
{
    public Transform respawnPoint;
    public float fallForce = -10f;
    public float fallDelay = 1f;

    private Rigidbody rb;
    private Collider playerCollider;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        // Start with collider disabled
        playerCollider.enabled = false;
        rb.useGravity = false;
    }

    public void StartFalling()
    {
        if (isFalling) return;

        isFalling = true;

        // Enable collider before fall
        playerCollider.enabled = true;

        rb.useGravity = true;
        rb.velocity = new Vector3(0, fallForce, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFalling && collision.gameObject.CompareTag("Ground"))
        {
            isFalling = false;

            // Optional delay before respawn
            Invoke(nameof(Respawn), 0.5f);
        }
    }

    void Respawn()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        // Disable collider before repositioning
        playerCollider.enabled = false;

        transform.position = respawnPoint.position;
    }
}
