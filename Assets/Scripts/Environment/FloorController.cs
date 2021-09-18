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
    private List<GameObject> fallenBlocks;
    private float secondsBetweenDrop;
    private bool continueCoroutine;

    void Awake()
    {
        blocks = new List<GameObject>();
        fallenBlocks = new List<GameObject>();
        secondsBetweenDrop = 1f;
        continueCoroutine = true;
        SetupBlocks();
        StartCoroutine(DropBlocks());
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

    private IEnumerator DropBlocks()
    {
        while(continueCoroutine)
        {
            yield return new WaitForSeconds(secondsBetweenDrop);

            int indexToDrop = Random.Range(0, blocks.Count - 1);

            GameObject blockToDrop;

            if (blocks[indexToDrop])
            {
                blockToDrop = blocks[indexToDrop];
            }
            else
            {
                continue;
            }

            blockToDrop.GetComponent<BlockController>().Drop();

            if (!fallenBlocks.Contains(blockToDrop))
            {
                fallenBlocks.Add(blockToDrop);
            }

            if (blocks.Contains(blockToDrop))
            {
                blocks.Remove(blockToDrop);
            }
        }
        
    }
}
