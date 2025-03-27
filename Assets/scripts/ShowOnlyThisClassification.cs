using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ShowOnlyThisClassification : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public PlaneClassification classification;

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
        List<ARPlane> newPlane = obj.added;

        foreach (var item in newPlane)
        {
            if(item.classification == classification)
            {
                //
            }
            else
            {
                Renderer itemRenderer = item.GetComponent<Renderer>();
                Destroy(itemRenderer);
            }
        }

    }
}