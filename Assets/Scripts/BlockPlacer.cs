using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject blockPrefab; // Assign your block prefab here
    public LayerMask placementLayer; // Set this to the layer of the plane
    public float gridSize = 1f; // Size of the grid

    private HashSet<Vector3> placedPositions = new HashSet<Vector3>(); // Tracking placed block positions

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray to detect where to place the block
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
            {
                Debug.Log("Hit detected on: " + hit.collider.name);

                // Snap the hit point to the grid (horizontal snapping only)
                Vector3 snappedPosition = SnapToGrid(hit.point);

                // Determine the correct height for the new block, starting at the plane's Y position
                float newYPosition = GetTopmostY(snappedPosition, hit.point.y, hit.normal);
                snappedPosition.y = newYPosition;

                // Align block rotation with the plane's rotation
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                // Place the block
                GameObject newBlock = Instantiate(blockPrefab, snappedPosition, rotation);
                Renderer blockRenderer = newBlock.GetComponent<Renderer>();

                // Align the block's bottom to the current position
                if (blockRenderer != null)
                {
                    Vector3 blockSize = blockRenderer.bounds.size;
                    newBlock.transform.position = snappedPosition;
                }

                // Add the position to the placed positions
                placedPositions.Add(snappedPosition);

                Debug.Log("Block placed at: " + snappedPosition);
            }
            else
            {
                Debug.Log("Raycast did not hit any object.");
            }
        }
    }

    // Snaps a position to the nearest grid cell
    private Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;
        return new Vector3(x, 0, z); // Only snap horizontal axes; y will be calculated later
    }

    // Get the topmost Y position for stacking a block
    private float GetTopmostY(Vector3 basePosition, float planeY, Vector3 planeNormal)
    {
        // Start with the plane's Y position
        float maxY = planeY;

        // Check for blocks already placed at this grid cell
        foreach (Vector3 placedPosition in placedPositions)
        {
            if (Mathf.Approximately(placedPosition.x, basePosition.x) &&
                Mathf.Approximately(placedPosition.z, basePosition.z))
            {
                maxY = Mathf.Max(maxY, placedPosition.y);
            }
        }

        // Add the block height to stack on top, adjusted for plane orientation
        float blockHeight = blockPrefab.GetComponent<Renderer>().bounds.size.y;
        Vector3 heightAdjustment = planeNormal.normalized * blockHeight;
        return maxY + heightAdjustment.y; // Use the normal's y-component for vertical stacking
    }
}

