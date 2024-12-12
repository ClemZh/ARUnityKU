using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



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

                GameObject newBlock = Instantiate(blockPrefab, position, rotation);
                Renderer blockRenderer = newBlock.GetComponent<Renderer>();
                //Place bottom side of the cube to the detected plane
    		if (blockRenderer != null)
		    {
			Vector3 blockSize = blockRenderer.bounds.size;
			newBlock.transform.position = hit.point + Vector3.up * (blockSize.y / 2);
		    }
                Debug.Log("Block placed at: " + position);
            }
            
            else
            {
                Debug.Log("Raycast did not hit any object.");
            }
        }
    }

}
