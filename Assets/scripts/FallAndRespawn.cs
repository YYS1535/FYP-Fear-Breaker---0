using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAndRespawn : MonoBehaviour
{
    public Transform respawnPoint;
    public float fallForce = -10f;
    public float fallDelay = 1f;
    public Collider extraCollider1; // Assign in Inspector
    public Collider extraCollider2; // Assign in Inspector
    public float reenableCollidersDelay = 1f;

    private Rigidbody rb;
    private Collider playerCollider;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        // Start with colliders and gravity disabled
        playerCollider.enabled = false;
        rb.useGravity = false;

        if (extraCollider1 != null) extraCollider1.enabled = true;
        if (extraCollider2 != null) extraCollider2.enabled = true;
    }

    public void StartFalling()
    {
        if (isFalling) return;

        isFalling = true;

        // Disable the two colliders you mentioned
        if (extraCollider1 != null) extraCollider1.enabled = false;
        if (extraCollider2 != null) extraCollider2.enabled = false;

        // Enable player collider and gravity
        playerCollider.enabled = true;
        rb.useGravity = true;
        rb.velocity = new Vector3(0, fallForce, 0);

        // Start coroutine to re-enable the other colliders after delay
        StartCoroutine(ReenableCollidersAfterDelay(reenableCollidersDelay));
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

    IEnumerator ReenableCollidersAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (extraCollider1 != null) extraCollider1.enabled = true;
        if (extraCollider2 != null) extraCollider2.enabled = true;
    }
}
