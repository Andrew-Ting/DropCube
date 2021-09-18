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
    private List<Vector3> fallenBlocks;
    private float secondsBetweenDrop;
    private bool continueCoroutine;

    void Awake()
    {
        blocks = new List<GameObject>();
        fallenBlocks = new List<Vector3>();
        secondsBetweenDrop = 1f;
        continueCoroutine = true;
        SetupBlocks();
        DropBlocksCoroutine();
    }

    private void SetupBlocks()
    {
        for (int i = -(floorSize - 1); i < floorSize; i++)
        {
            for (int j = -(floorSize - 1); j < floorSize; j++)
            {
                CreateBlock(new Vector3(i, 0, j));
            }
        }
    }

    private void DropBlocksCoroutine()
    {
        StartCoroutine(DropBlocks());
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

            if (!fallenBlocks.Contains(blockToDrop.transform.position))
            {
                fallenBlocks.Add(blockToDrop.transform.position);
            }

            if (blocks.Contains(blockToDrop))
            {
                blocks.Remove(blockToDrop);
            }

            yield return new WaitForSeconds(4f);

            if (blockToDrop)
            {
                Destroy(blockToDrop);
            }
        }
    }

    public void ResetFloor()
    {
        foreach (Vector3 position in fallenBlocks)
        {
            CreateBlock(position);
        }
    }

    private void CreateBlock(Vector3 position)
    {
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
        block.transform.parent = this.transform;
        blocks.Add(block);
    }
}
