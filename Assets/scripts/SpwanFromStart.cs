using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class SpwanFromStart : MonoBehaviour
{
    public GameObject SpawnObject;
    public float ForwardOffset = 0.2f; // Distance from table center towards player
    private MRUKRoom currentRoom;
    private bool isRoomReady = false;

    private void Start()
    {
        if (MRUK.Instance)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(() =>
            {
                currentRoom = MRUK.Instance.GetCurrentRoom();
                isRoomReady = true;
                SpawnOnTable();
            });
        }
        else
        {
            Debug.LogError("MRUK Instance not found!");
        }
    }

    private void SpawnOnTable()
    {
        if (!isRoomReady || currentRoom == null)
        {
            Debug.LogWarning("MRUK Room is not ready yet! Try again later.");
            return;
        }

        Vector3 playerPosition = Camera.main.transform.position;
        Vector3 forwardDirection = Camera.main.transform.forward;

        if (currentRoom.GenerateRandomPositionOnSurface(
            MRUK.SurfaceType.FACING_UP,
            0.2f,
            new LabelFilter(MRUKAnchor.SceneLabels.TABLE),
            out var tablePosition,
            out var tableNormal))
        {
            // Detect table bounds
            Collider tableCollider = GetTableColliderAtPosition(tablePosition);
            if (tableCollider == null)
            {
                Debug.LogWarning("Table collider not found! Using default position.");
            }

            Vector3 finalPosition;
            if (tableCollider != null)
            {
                Bounds tableBounds = tableCollider.bounds;

                // Find front center of the table based on the player's direction
                Vector3 tableCenter = tableBounds.center;
                Vector3 tableForward = Vector3.ProjectOnPlane(forwardDirection, Vector3.up).normalized;
                Vector3 frontCenter = tableCenter + (tableForward * (tableBounds.extents.z - ForwardOffset));

                finalPosition = new Vector3(frontCenter.x, tableBounds.max.y + 0.02f, frontCenter.z); // Ensure it's slightly above
            }
            else
            {
                finalPosition = new Vector3(tablePosition.x, tablePosition.y + 0.02f, tablePosition.z);
            }

            Quaternion finalRotation = Quaternion.LookRotation(-forwardDirection, Vector3.up);

            // Check for overlapping objects before spawning
            Collider[] colliders = Physics.OverlapSphere(finalPosition, 0.1f);
            if (colliders.Length > 0)
            {
                Debug.LogWarning("Spawn position is occupied! Adjusting...");
                finalPosition += Vector3.up * 0.1f; // Move slightly up to avoid overlap
            }

            // Spawn the object
            GameObject spawnedObject = Instantiate(SpawnObject, finalPosition, finalRotation);

            // Disable Rigidbody initially to prevent premature physics activation
            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Temporarily disable physics
                StartCoroutine(EnablePhysicsAfterDelay(rb));
            }
            else
            {
                Debug.LogWarning("Spawned object has no Rigidbody! Consider adding one.");
            }
        }
        else
        {
            Debug.LogWarning("No table found to spawn the object.");
        }
    }

    private Collider GetTableColliderAtPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Table")) // Ensure your table has the tag "Table"
            {
                return col;
            }
        }
        return null;
    }

    private IEnumerator EnablePhysicsAfterDelay(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.2f);
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
