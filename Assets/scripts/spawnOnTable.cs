using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class spawnOnTable : MonoBehaviour
{
    public GameObject SpawnObject;
    public float DistanceFromPlayer = 0.5f; // Adjust as needed
    private MRUKRoom currentRoom;

    private void Start()
    {
        if (MRUK.Instance)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(() =>
            {
                currentRoom = MRUK.Instance.GetCurrentRoom();
                SpawnOnTable();
            });
        }
    }

    private void SpawnOnTable()
    {
        if (currentRoom == null) return;

        // Get player's position and forward direction
        Vector3 playerPosition = Camera.main.transform.position;
        Vector3 forwardDirection = Camera.main.transform.forward;

        // Try to find a table (FACING_UP surfaces)
        if (currentRoom.GenerateRandomPositionOnSurface(
            MRUK.SurfaceType.FACING_UP,
            0.2f,
            new LabelFilter(MRUKAnchor.SceneLabels.TABLE),
            out var tablePosition,
            out var tableNormal))
        {
            // Adjust spawn position to be in front of player
            Vector3 targetPosition = tablePosition + (forwardDirection * DistanceFromPlayer);
            targetPosition.y = tablePosition.y; // Ensure it stays on the table

            Quaternion targetRotation = Quaternion.LookRotation(-forwardDirection, tableNormal);

            // Spawn the object
            Instantiate(SpawnObject, targetPosition, targetRotation);
        }
        else
        {
            Debug.LogWarning("No table found to spawn the object.");
        }
    }
}
