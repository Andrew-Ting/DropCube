using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private Text roundText;
    [SerializeField]
    private GameObject powerupPrefab;

    private Animator screenTransition;

    private List<GameObject> blocks;
    private List<Vector3> fallenBlocks;
    private float secondsBetweenDrop;
    private bool continueCoroutine;
    private GameObject buttonBlock;
    private int round;

    private float verticalDisplacement = 10f;
    private float resetTime = 2f;
    private float dropTime;
    private float numBlocksToDrop;
    private float powerupMinimumResetTime = 20f;
    private float powerupResetTimeUncertainty = 10f;

    void Awake()
    {
        blocks = new List<GameObject>();
        fallenBlocks = new List<Vector3>();
        secondsBetweenDrop = 2f;
        dropTime = 2f;
        numBlocksToDrop = 1;
        continueCoroutine = true;
        screenTransition = GameObject.Find("CrossFadePanel").GetComponent<Animator>();
        round = 0;
        SetupBlocks();
        GenerateResetButton();
        DropBlocksCoroutine();
        PowerupRespawnCoroutine();
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

        Vector3 playerPos = new Vector3(Player.GetComponent<PlayerController>().GetXPosition(), Player.GetComponent<PlayerController>().GetZPosition());

        while (Vector3.Distance(blocks[indexForResetButton].transform.position, playerPos) <= 5f)
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
    private void PowerupRespawnCoroutine()
    { // spin lock check for existence of powerup; not the most efficient, but it decouples powerupcontroller and floorcontroller
        StartCoroutine(PowerupRespawn());
    }
    private IEnumerator PowerupRespawn()
    {
        while (true)
        {
            if (!FindObjectOfType<PowerupController>())
            {
                yield return new WaitForSeconds(Random.Range(powerupMinimumResetTime, powerupMinimumResetTime + powerupResetTimeUncertainty));
                ResetPowerupIfObtained();
            }
            yield return new WaitForSeconds(5);
        }
    }
    private IEnumerator DropBlocks()
    {
        while (continueCoroutine)
        {
            yield return new WaitForSeconds(secondsBetweenDrop);

            List<int> indicesToDrop = new List<int>();

            for (int i = 0; i < numBlocksToDrop; i++)
            {
                int indexToDrop = Random.Range(0, blocks.Count - 1);
                while (blocks[indexToDrop].Equals(buttonBlock) || indicesToDrop.Contains(indexToDrop))
                {
                    indexToDrop = Random.Range(0, blocks.Count - 1);
                }
                indicesToDrop.Add(indexToDrop);
            }

            foreach (int index in indicesToDrop)
            {
                GameObject blockToDrop;

                if (blocks[index])
                {
                    blockToDrop = blocks[index];
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

                blockToDrop.GetComponent<BlockController>().Drop(dropTime);
                /*
                yield return new WaitForSeconds(4f);

                if (blockToDrop)
                {
                    Destroy(blockToDrop);
                }*/
            }
        }
    }

    private void ResetFloor()
    {
        foreach (Vector3 position in fallenBlocks)
        {
            GameObject block = CreateBlock(position);
            StartCoroutine(LowerBlock(block, position, new Vector3(position.x, 0, position.z), resetTime));
        }
    }
    public void PlaceBlockAtPosition(Vector3 position)
    {
        GameObject block = CreateBlock(position);
        block.transform.parent = this.transform;
        fallenBlocks.Remove(new Vector3(position.x, verticalDisplacement, position.z));
    }
    private GameObject CreateBlock(Vector3 position)
    {
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
        block.transform.parent = this.transform;
        blocks.Add(block);
        return block;
    }
    private void ResetPowerupIfObtained()
    {
        if (!FindObjectOfType<PowerupController>())
        {
            Vector3 randomPosition = new Vector3(Random.Range(-floorSize + 1, floorSize - 1), 1, Random.Range(-floorSize + 1, floorSize - 1));
            GameObject powerup = Instantiate(powerupPrefab, randomPosition, Quaternion.Euler(0, 45, 45));
        }
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

        roundText.text = (++round).ToString();

        foreach (Transform child in buttonBlock.transform)
        {
            Destroy(child.gameObject);
        }
        blocks.Add(buttonBlock);

        ResetFloor();
        fallenBlocks.Clear();
        CheckLevelProgression();
        Invoke("DelayedResetParams", resetTime);
    }

    private void DelayedResetParams()
    {
        GenerateResetButton();
        Player.GetComponent<PlayerController>().EnableMovement();
        continueCoroutine = true;
        DropBlocksCoroutine();
    }

    public void GameOver()
    {
        continueCoroutine = false;

        Score.currentScore = int.Parse(roundText.text);

        if (Score.currentScore > Score.highScore)
        {
            Score.highScore = Score.currentScore;
        }

        foreach (Transform child in transform)
        {
            Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            rb.AddExplosionForce(12f, Random.insideUnitSphere, 5f, 1.2f);
        }

        Invoke("DestroyBlocks", 4f);
        StartCoroutine(SwitchToGameOver());
    }

    private void DestroyBlocks()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    private IEnumerator SwitchToGameOver() {
        screenTransition.SetTrigger("gameOver");
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene("GameOver");
        yield return false;
    }

    private void CheckLevelProgression()
    {
        if (secondsBetweenDrop > 0.2f)
        {
            secondsBetweenDrop -= 0.1f;
        }

        if (round >= 8 && round % 4 == 0 && dropTime > 0.5f)
        {
            dropTime -= 0.2f;
        }

        if (round >= 20 && round % 20 == 0 && round <= 40)
        {
            numBlocksToDrop++;
            DropBlocksCoroutine();
        }
    }
}
