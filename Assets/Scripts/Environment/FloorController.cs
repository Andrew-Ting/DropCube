using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private int floorSize;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject resetButtonPrefab;

    private List<GameObject> blocks;
    private List<Vector3> fallenBlocks;
    private float secondsBetweenDrop;
    private bool continueCoroutine;
    private GameObject buttonBlock;

    private float verticalDisplacement = 20f;

    void Awake()
    {
        blocks = new List<GameObject>();
        fallenBlocks = new List<Vector3>();
        secondsBetweenDrop = 1f;
        continueCoroutine = true;
        SetupBlocks();
        GenerateResetButton();
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

    private void GenerateResetButton()
    {
        int indexForResetButton = Random.Range(0, blocks.Count - 1);

        while (blocks[indexForResetButton].transform.position.x == Player.GetComponent<PlayerController>().GetXPosition() &&
            blocks[indexForResetButton].transform.position.z == Player.GetComponent<PlayerController>().GetZPosition())
        {
            indexForResetButton = Random.Range(0, blocks.Count - 1);
        }

        buttonBlock = blocks[indexForResetButton];
        blocks.Remove(buttonBlock);
        Instantiate(
            resetButtonPrefab, 
            new Vector3(buttonBlock.transform.position.x, 0.6f, buttonBlock.transform.position.z),
            Quaternion.identity,
            buttonBlock.transform);
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

            Vector3 newDropBlockPos = blockToDrop.transform.position + new Vector3(0, verticalDisplacement, 0);
            if (!fallenBlocks.Contains(newDropBlockPos))
            {
                fallenBlocks.Add(newDropBlockPos);
            }

            if (blocks.Contains(blockToDrop))
            {
                blocks.Remove(blockToDrop);
            }

            blockToDrop.GetComponent<BlockController>().Drop();

            yield return new WaitForSeconds(4f);

            if (blockToDrop)
            {
                Destroy(blockToDrop);
            }
        }
    }

    private void ResetFloor()
    {
        foreach (Vector3 position in fallenBlocks)
        {
            GameObject block = CreateBlock(position);
            StartCoroutine(LowerBlock(block, position, new Vector3(position.x, 0, position.z), 5f));
        }
    }

    private GameObject CreateBlock(Vector3 position)
    {
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
        block.transform.parent = this.transform;
        blocks.Add(block);
        return block;
    }
    
    private IEnumerator LowerBlock(GameObject block, Vector3 startPos, Vector3 newPos, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            block.transform.position = Vector3.Lerp(startPos, newPos, (Time.time - startTime) / overTime);
            yield return null;
        }

        block.transform.position = newPos;
    }

    public bool CheckPlayerTouchedReset()
    {
        return (Player.GetComponent<PlayerController>().GetXPosition() == buttonBlock.transform.position.x &&
            Player.GetComponent<PlayerController>().GetZPosition() == buttonBlock.transform.position.z);
    }

    public void ResetLevel()
    {
        continueCoroutine = false;
        foreach (Transform child in buttonBlock.transform)
        {
            Destroy(child.gameObject);
        }
        blocks.Add(buttonBlock);

        ResetFloor();
        fallenBlocks.Clear();

        Invoke("DelayedResetParams", 5f);
    }

    private void DelayedResetParams()
    {
        GenerateResetButton();
        Player.GetComponent<PlayerController>().EnableMovement();
        continueCoroutine = true;
        DropBlocksCoroutine();
    }
}
