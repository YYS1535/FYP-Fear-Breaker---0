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
            Debug.LogWarning("MRUK Room not ready!");
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

            if (SpawnMultipleObjects && SpawnObjects.Length > 0)
            {
                float spacing = 0.15f; // horizontal spacing between objects

                for (int i = 0; i < SpawnObjects.Length; i++)
                {
                    Vector3 offset = Vector3.right * ((i - (SpawnObjects.Length - 1) / 2.0f) * spacing);
                    SpawnObjectAtPosition(SpawnObjects[i], basePosition + offset, -forwardDirection);
                }
            }
            else if (SingleSpawnObject != null)
            {
                SpawnObjectAtPosition(SingleSpawnObject, basePosition, -forwardDirection);
            }
        }
        else
        {
            Debug.LogWarning("No table surface found.");
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
}
