using UnityEngine;
using UnityEngine.UI;

public class BlockSelector : MonoBehaviour
{
    public GameObject[] blockPrefabs;  // Array to hold different block prefabs
    public BlockPlacer blockPlacer;     // Reference to BlockPlacer script

    void Start()
    {
        // Find the BlockPlacer script if not assigned
        if (blockPlacer == null)
            blockPlacer = FindObjectOfType<BlockPlacer>();
    }

    // Function to change the block type based on button click
    public void SelectBlock(int index)
    {
        if (index >= 0 && index < blockPrefabs.Length)
        {
            blockPlacer.selectedBlockPrefab = blockPrefabs[index];;
            Debug.Log("Selected block: " + blockPrefabs[index].name);
        }
    }
}

