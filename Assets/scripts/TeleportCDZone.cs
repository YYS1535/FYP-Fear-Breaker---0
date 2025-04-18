using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCDZone : MonoBehaviour
{
    public GameObject teleportPlaneToControl; // Assign your teleport plane object here
    public float cooldownDuration = 3f; // Duration to disable teleport
    private bool isCoolingDown = false;

    private Collider teleportPlaneCollider;

    void Start()
    {
        if (teleportPlaneToControl != null)
        {
            teleportPlaneCollider = teleportPlaneToControl.GetComponent<Collider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCoolingDown || teleportPlaneCollider == null) return;

        if (other.CompareTag("MainCamera") || other.CompareTag("Player")) // Tag your rig or head accordingly
        {
            StartCoroutine(TeleportCooldown());
        }
    }

    private IEnumerator TeleportCooldown()
    {
        isCoolingDown = true;
        teleportPlaneCollider.enabled = false; // Temporarily disable teleporting

        yield return new WaitForSeconds(cooldownDuration);

        teleportPlaneCollider.enabled = true; // Re-enable teleporting
        isCoolingDown = false;
    }
}
