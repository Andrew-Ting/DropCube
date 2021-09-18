using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private int floorSize;

    private List<GameObject> blocks;

    // Start is called before the first frame update
    void Start()
    {
        blocks = new List<GameObject>();
        SetupBlocks();
    }

    private void SetupBlocks()
    {
        for (int i = -(floorSize - 1); i < floorSize; i++)
        {
            for (int j = -(floorSize - 1); j < floorSize; j++)
            {
                GameObject block = Instantiate(blockPrefab, new Vector3(i, 0, j), Quaternion.identity);
                block.transform.parent = this.transform;
                blocks.Add(block);
            }
        }
    }
}
