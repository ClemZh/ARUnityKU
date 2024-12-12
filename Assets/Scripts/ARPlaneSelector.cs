using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARPlaneSelector : MonoBehaviour
{
    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;
    private ARPlane selectedPlane;

    public Material selectedMaterial; // Highlight material
    public Material defaultMaterial; // Default plane material

    void Start()
    {
        planeManager = GetComponent<ARPlaneManager>();
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                ARPlane plane = planeManager.GetPlane(hits[0].trackableId);

                if (plane != null)
                {
                    SelectPlane(plane);
                }
            }
        }
    }

    private void SelectPlane(ARPlane plane)
    {
        // Reset previously selected plane if any
        if (selectedPlane != null)
        {
            SetPlaneMaterial(selectedPlane, defaultMaterial);
        }

        // Highlight the newly selected plane
        selectedPlane = plane;
        SetPlaneMaterial(selectedPlane, selectedMaterial);
    }

    private void SetPlaneMaterial(ARPlane plane, Material material)
    {
        Renderer renderer = plane.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    public ARPlane GetSelectedPlane()
    {
        return selectedPlane;
    }
}

