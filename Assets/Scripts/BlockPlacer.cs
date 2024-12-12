using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


// public class BlockPlacer : MonoBehaviour
// {
//     public GameObject blockPrefab; // Assign your block prefab
//     public float gridSize = 0.1f;  // Size for snapping

//     private ARPlaneSelector planeSelector;

//     void Start()
//     {
//         planeSelector = FindObjectOfType<ARPlaneSelector>();
//     }

//     void Update()
//     {
//         if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//         {
//             PlaceBlockAtTouchPosition(Input.GetTouch(0).position);
//         }
//     }

//     void PlaceBlockAtTouchPosition(Vector2 touchPosition)
//     {
//         ARPlane selectedPlane = planeSelector.GetSelectedPlane();
//         if (selectedPlane == null)
//         {
//             Debug.Log("No plane selected for block placement.");
//             return;
//         }

//         List<ARRaycastHit> hits = new List<ARRaycastHit>();
//         ARRaycastManager raycastManager = FindObjectOfType<ARRaycastManager>();

//         if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
//         {
//             Pose hitPose = hits[0].pose;

//             // Ensure the hit is on the selected plane
//             if (hits[0].trackableId == selectedPlane.trackableId)
//             {
//                 Vector3 snappedPosition = SnapToGrid(hitPose.position);
//                 Instantiate(blockPrefab, snappedPosition, Quaternion.identity);
//             }
//         }
//     }

//     Vector3 SnapToGrid(Vector3 position)
//     {
//         float x = Mathf.Round(position.x / gridSize) * gridSize;
//         float y = position.y; // Keep the height the same
//         float z = Mathf.Round(position.z / gridSize) * gridSize;
//         return new Vector3(x, y, z);
//     }
// }


public class BlockPlacer : MonoBehaviour
{
    public GameObject blockPrefab; // Assign your block prefab here
    public LayerMask placementLayer; // Set this to the layer of the plane

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
            {
                Debug.Log("Hit detected on: " + hit.collider.name);

                Vector3 position = hit.point;
                position.x = Mathf.Round(position.x);
                position.z = Mathf.Round(position.z);
                
		// Align block rotation with the plane's rotation
        	Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                Instantiate(blockPrefab, position, rotation);
                Debug.Log("Block placed at: " + position);
            }
            else
            {
                Debug.Log("Raycast did not hit any object.");
            }
        }
    }

}
