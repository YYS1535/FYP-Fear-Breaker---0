using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnObject : MonoBehaviour
{
    public ARPlaneManager planeManager; // Reference to AR Plane Manager
    public PlaneClassification classification = PlaneClassification.Table; // Only detect tables
    public GameObject objectPrefab; // The object to spawn

    private bool hasSpawned = false; // Prevents multiple spawns

    private void OnEnable()
    {
        planeManager.planesChanged += SetupPlane;
    }

    private void OnDisable()
    {
        planeManager.planesChanged -= SetupPlane;
    }

    private void SetupPlane(ARPlanesChangedEventArgs obj)
    {
        if (hasSpawned) return; // Stop if object is already spawned

        foreach (var plane in obj.added)
        {
            if (plane.classification == classification) // Check if it's a table
            {
                Vector3 spawnPosition = plane.transform.position + Vector3.up * 0.05f; // Slight offset to prevent clipping
                Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
                hasSpawned = true; // Prevent multiple spawns
                Debug.Log("Object spawned on table!");
                break; // Exit loop after spawning
            }
            else
            {
                Renderer planeRenderer = plane.GetComponent<Renderer>();
                if (planeRenderer != null)
                {
                    Destroy(planeRenderer); // Hide non-table planes
                }
            }
        }
    }
}
