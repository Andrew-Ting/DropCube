using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFloor : MonoBehaviour
{
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject player;

    private int floorSize;

    // Start is called before the first frame update
    void Start()
    {
        floorSize = 5;
        SetupBlocks();
    }

    private void SetupBlocks()
    {
        for (int i = -(floorSize - 1); i < floorSize; i++)
        {
            for (int j = -(floorSize - 1); j < floorSize; j++)
            {
                Vector3 startPos = new Vector3(i, -10f, j);
                GameObject block = CreateBlock(startPos);
                
                if (i == 0 && j == 0)
                {
                    StartCoroutine(RaiseBlockDelayed(block, startPos, new Vector3(startPos.x, 0, startPos.z), 3f));
                }
                else
                {
                    StartCoroutine(RaiseBlock(block, startPos, new Vector3(startPos.x, 0, startPos.z), 3f));
                }
            }
        }

        Invoke("LoadGame", 6f);
    }

    private GameObject CreateBlock(Vector3 position)
    {
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
        block.transform.parent = this.transform;
        return block;
    }

    private IEnumerator RaiseBlockDelayed(GameObject block, Vector3 startPos, Vector3 newPos, float overTime)
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(RaiseBlock(block, startPos, newPos, overTime));
        StartCoroutine(RaiseBlock(player, startPos + Vector3.up, newPos + Vector3.up, overTime));
    }

    private IEnumerator RaiseBlock(GameObject block, Vector3 startPos, Vector3 newPos, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            block.transform.position = Vector3.Lerp(startPos, newPos, (Time.time - startTime) / overTime);
            yield return null;
        }

        block.transform.position = newPos;
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
