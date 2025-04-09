using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class spawnOnTable : MonoBehaviour
{
    [Header("Optional Video Players")]
    public GameObject[] VideoPlayers;

    public float ForwardOffset = 0.2f;
    public float ElementSpacing = 0.25f;
    public float TablePaddingY = 0.02f;

    private MRUKRoom currentRoom;

    public void TrySpawn()
    {
        if (MRUK.Instance)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(() =>
            {
                currentRoom = MRUK.Instance.GetCurrentRoom();
                SpawnVideoElements();
            });
        }
        else
        {
            Debug.LogError("MRUK Instance not found!");
        }
    }

    private void SpawnVideoElements()
    {
        if (currentRoom == null)
        {
            Debug.LogWarning("MRUK Room not ready.");
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
            Bounds tableBounds = tableCollider != null ? tableCollider.bounds : new Bounds(tablePosition, Vector3.one);

            Vector3 tableCenter = tableBounds.center;
            Vector3 tableForward = Vector3.ProjectOnPlane(forwardDirection, Vector3.up).normalized;
            Vector3 baseSpawnPos = tableCenter + (tableForward * (tableBounds.extents.z - ForwardOffset));
            float tableTopY = tableBounds.max.y + TablePaddingY;

            Quaternion rotationToUser = Quaternion.LookRotation(-forwardDirection, Vector3.up);

            // Spawn optional videos
            for (int i = 0; i < VideoPlayers.Length; i++)
            {
                if (VideoPlayers[i] == null) continue;

                Vector3 spawnPos = baseSpawnPos - tableForward * (ElementSpacing * (2 - i));
                spawnPos.y = tableTopY - 0.01f; // Slightly inside the table
                Instantiate(VideoPlayers[i], spawnPos, rotationToUser);
            }
        }
        else
        {
            Debug.LogWarning("No suitable table found.");
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
