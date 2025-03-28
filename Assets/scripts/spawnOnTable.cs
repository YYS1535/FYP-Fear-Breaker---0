using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class spawnOnTable : MonoBehaviour
{
    public GameObject SpawnObject;
    public float DistanceFromPlayer = 0.5f;
    private MRUKRoom currentRoom;
    private bool isRoomReady = false; // Flag to check if MRUK is initialized

    private void Start()
    {
        if (MRUK.Instance)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(() =>
            {
                currentRoom = MRUK.Instance.GetCurrentRoom();
                isRoomReady = true; // Set flag when MRUK is ready
                Debug.Log("MRUK Room initialized and ready for spawning.");
            });
        }
        else
        {
            Debug.LogError("MRUK Instance not found!");
        }
    }

    // PUBLIC FUNCTION TO SPAWN OBJECT (Call this from Unity Inspector)
    public void SpawnOnTable()
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
            Vector3 targetPosition = tablePosition + (forwardDirection * DistanceFromPlayer);
            targetPosition.y = tablePosition.y; // Ensure it stays on the table

            Quaternion targetRotation = Quaternion.LookRotation(-forwardDirection, tableNormal);

            // Spawn the object
            GameObject spawnedObject = Instantiate(SpawnObject, targetPosition, targetRotation);

            // Ensure the object properly interacts with physics
            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Allow physics simulation
                rb.useGravity = true; // Ensure it drops correctly onto the table
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
}
