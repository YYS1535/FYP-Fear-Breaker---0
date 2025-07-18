using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class SpwanFromStart : MonoBehaviour
{
    [Header("Spawn Settings")]
    public bool SpawnMultipleObjects = false;
    public GameObject[] SpawnObjects; // For multiple object support
    public GameObject SingleSpawnObject; // For levels with only 1 object
    public float ForwardOffset = 0.2f;

    [Header("Fallback Table")]
    public GameObject virtualTablePrefab;
    public float virtualTableYOffset = 0.5f; // Height above ground
    public float virtualTableDistance = 1.5f; // Distance from player
    public Vector3 virtualTableSize = new Vector3(1.0f, 0.1f, 0.6f); // Width, Height, Depth

    private MRUKRoom currentRoom;
    private bool isRoomReady = false;
    private GameObject spawnedVirtualTable;

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
            Debug.LogWarning("MRUK Room not ready!");
            return;
        }

        Vector3 playerPosition = Camera.main.transform.position;
        Vector3 forwardDirection = Camera.main.transform.forward;

        // Try to find a physical table first
        if (currentRoom.GenerateRandomPositionOnSurface(
            MRUK.SurfaceType.FACING_UP,
            0.2f,
            new LabelFilter(MRUKAnchor.SceneLabels.TABLE),
            out var tablePosition,
            out var tableNormal))
        {
            Debug.Log("Physical table found! Spawning objects on physical table.");
            SpawnOnPhysicalTable(tablePosition, forwardDirection);
        }
        else
        {
            Debug.Log("No physical table found. Creating virtual table.");
            SpawnVirtualTableAndObjects(playerPosition, forwardDirection);
        }
    }

    private void SpawnOnPhysicalTable(Vector3 tablePosition, Vector3 forwardDirection)
    {
        Collider tableCollider = GetTableColliderAtPosition(tablePosition);
        if (tableCollider == null)
        {
            Debug.LogWarning("No table collider found!");
        }

        Bounds tableBounds = tableCollider?.bounds ?? new Bounds(tablePosition, new Vector3(0.5f, 0.1f, 0.5f));
        Vector3 tableCenter = tableBounds.center;
        Vector3 tableForward = Vector3.ProjectOnPlane(forwardDirection, Vector3.up).normalized;
        Vector3 basePosition = tableCenter + (tableForward * (tableBounds.extents.z - ForwardOffset));
        basePosition.y = tableBounds.max.y + 0.05f;

        SpawnObjectsAtPosition(basePosition, -forwardDirection);
    }

    private void SpawnVirtualTableAndObjects(Vector3 playerPosition, Vector3 forwardDirection)
    {
        if (virtualTablePrefab == null)
        {
            Debug.LogError("Virtual table prefab is not assigned!");
            return;
        }

        // Calculate virtual table position
        Vector3 tableForward = Vector3.ProjectOnPlane(forwardDirection, Vector3.up).normalized;
        Vector3 virtualTablePosition = playerPosition + (tableForward * virtualTableDistance);

        // Find the floor level or use the offset
        float floorY = GetFloorLevel(virtualTablePosition);
        virtualTablePosition.y = floorY + virtualTableYOffset;

        // Spawn the virtual table
        spawnedVirtualTable = Instantiate(virtualTablePrefab, virtualTablePosition, Quaternion.LookRotation(tableForward));

        // Ensure the virtual table has proper tag
        SetupVirtualTableCollider(spawnedVirtualTable);

        Debug.Log($"Virtual table spawned at: {virtualTablePosition}");

        // Wait a frame to ensure the table is fully instantiated, then spawn objects
        StartCoroutine(SpawnObjectsOnVirtualTable(tableForward));
    }

    private float GetFloorLevel(Vector3 position)
    {
        // Try to find the floor level using MRUK
        if (currentRoom != null && currentRoom.GenerateRandomPositionOnSurface(
            MRUK.SurfaceType.FACING_UP,
            0.1f,
            new LabelFilter(MRUKAnchor.SceneLabels.FLOOR),
            out var floorPosition,
            out var floorNormal))
        {
            return floorPosition.y;
        }

        // Fallback: Use raycast to find floor
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 2f, Vector3.down, out hit, 10f))
        {
            return hit.point.y;
        }

        // Ultimate fallback: Use player's foot level
        return Camera.main.transform.position.y - 1.7f; // Assuming average height
    }

    private void SetupVirtualTableCollider(GameObject table)
    {
        // Ensure the table has the correct tag
        if (!table.CompareTag("Table"))
        {
            table.tag = "Table";
        }
    }

    private void SpawnObjectsAtPosition(Vector3 basePosition, Vector3 lookDirection)
    {
        if (SpawnMultipleObjects && SpawnObjects.Length > 0)
        {
            float spacing = 0.15f; // horizontal spacing between objects

            for (int i = 0; i < SpawnObjects.Length; i++)
            {
                Vector3 offset = Vector3.right * ((i - (SpawnObjects.Length - 1) / 2.0f) * spacing);
                SpawnObjectAtPosition(SpawnObjects[i], basePosition + offset, lookDirection);
            }
        }
        else if (SingleSpawnObject != null)
        {
            SpawnObjectAtPosition(SingleSpawnObject, basePosition, lookDirection);
        }
    }

    private void SpawnObjectAtPosition(GameObject prefab, Vector3 position, Vector3 lookDirection)
    {
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);

        if (colliders.Length > 0)
        {
            position += Vector3.up * 0.1f;
        }

        GameObject obj = Instantiate(prefab, position, rotation);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            StartCoroutine(EnablePhysicsAfterDelay(rb));
        }
    }

    private Collider GetTableColliderAtPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Table"))
                return col;
        }
        return null;
    }

    private IEnumerator EnablePhysicsAfterDelay(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.2f);
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private IEnumerator SpawnObjectsOnVirtualTable(Vector3 tableForward)
    {
        // Wait one frame to ensure virtual table is fully instantiated
        yield return null;

        if (spawnedVirtualTable == null)
        {
            Debug.LogError("Virtual table is null!");
            yield break;
        }

        // Get the actual collider bounds from the spawned virtual table
        Collider virtualTableCollider = spawnedVirtualTable.GetComponent<Collider>();
        if (virtualTableCollider == null)
        {
            Debug.LogError("Virtual table has no collider!");
            yield break;
        }

        Bounds tableBounds = virtualTableCollider.bounds;
        Vector3 basePosition = tableBounds.center;
        basePosition.y = tableBounds.max.y + 0.05f;

        Debug.Log($"Spawning objects on virtual table at position: {basePosition}");
        SpawnObjectsAtPosition(basePosition, -tableForward);
    }

    // Optional: Method to manually trigger table respawn (useful for testing)
    public void RespawnTable()
    {
        if (spawnedVirtualTable != null)
        {
            DestroyImmediate(spawnedVirtualTable);
        }

        SpawnOnTable();
    }

    // Clean up when the object is destroyed
    private void OnDestroy()
    {
        if (spawnedVirtualTable != null)
        {
            DestroyImmediate(spawnedVirtualTable);
        }
    }
}