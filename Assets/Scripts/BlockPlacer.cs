using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // For UI interactions
using UnityEngine.EventSystems;

public class BlockPlacer : MonoBehaviour
{
    public GameObject[] blockPrefabs;  // Array of available block prefabs
    public GameObject previewBlock;    // Block preview variable
    public LayerMask placementLayer;   // Set this to the layer of the plane
    public float gridSize = 1f;        // Size of the grid
    public Button[] blockButtons;      // UI buttons to select blocks
    public GameObject selectedBlockPrefab; // Currently selected block prefab

    private HashSet<Vector3> placedPositions = new HashSet<Vector3>(); // Tracking placed block positions

    void Start()
    {
        // Set the default selected block to the first prefab
        if (blockPrefabs.Length > 0)
        {
            selectedBlockPrefab = blockPrefabs[0];
        }
        else
        {
            Debug.LogError("No block prefabs assigned!");
            return;
        }

        // Add button listeners for block selection
        for (int i = 0; i < blockButtons.Length; i++)
        {
            int index = i; // Capture index in closure
            blockButtons[i].onClick.AddListener(() => SelectBlock(index));
        }
    }

    void Update()
    {
    	if (IsPointerOverUI())
	{
		return; // Don't place a block if interacting with UI
	}
        if (Input.GetMouseButtonDown(0)) 
        {
            PlaceBlock();
        }
    }

    // Function to place the selected block
    private void PlaceBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
        {
            Debug.Log("Hit detected on: " + hit.collider.name);

            Vector3 snappedPosition = SnapToGrid(hit.point);
            float newYPosition = GetTopmostY(snappedPosition, hit.point.y, hit.normal);
            snappedPosition.y = newYPosition;

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            GameObject newBlock = Instantiate(selectedBlockPrefab, snappedPosition, rotation);
            
            if (newBlock.TryGetComponent<Renderer>(out Renderer blockRenderer))
            {
                Vector3 blockSize = blockRenderer.bounds.size;
                newBlock.transform.position = snappedPosition;
            }

            placedPositions.Add(snappedPosition);

            Debug.Log("Block placed at: " + snappedPosition);
        }
        else
        {
            Debug.Log("Raycast did not hit any object.");
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
        float maxY = planeY;

        foreach (Vector3 placedPosition in placedPositions)
        {
            if (Mathf.Approximately(placedPosition.x, basePosition.x) &&
                Mathf.Approximately(placedPosition.z, basePosition.z))
            {
                maxY = Mathf.Max(maxY, placedPosition.y);
            }
        }

        float blockHeight = selectedBlockPrefab.GetComponent<Renderer>().bounds.size.y;
        Vector3 heightAdjustment = planeNormal.normalized * blockHeight;
        return maxY + heightAdjustment.y;
    }

    // Function to handle block selection from UI buttons
    public void SelectBlock(int index)
    {
        if (index >= 0 && index < blockPrefabs.Length)
        {
            selectedBlockPrefab = blockPrefabs[index];
            Debug.Log("Selected block: " + selectedBlockPrefab.name);
        }
        else
        {
            Debug.LogError("Invalid block index selected.");
        }
    }


    private bool IsPointerOverUI()
	{
	    if (EventSystem.current == null)
	    {
		Debug.LogWarning("No EventSystem found in the scene!");
		return false; // Allow placement if no EventSystem is available
	    }

	    if (EventSystem.current.IsPointerOverGameObject())
	    {
		Debug.Log("UI clicked, ignoring raycast.");
		return true;
	    }

	    if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
	    {
		Debug.Log("Touch detected on UI.");
		return true;
	    }

    return false;
	}
}

