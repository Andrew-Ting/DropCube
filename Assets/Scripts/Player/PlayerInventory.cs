using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    private int blockPowerupCnt = 0;
    public void AddBlockPowerup() {
        blockPowerupCnt++;
        UpdatePowerupCnt();
    }
    public bool UseBlockPowerup(Vector3 positionToRaycastDownOn) {
        RaycastHit hit;
        Debug.Log(positionToRaycastDownOn + " " + Physics.Raycast(positionToRaycastDownOn, Vector3.down, out hit, 1));
        if (blockPowerupCnt > 0 && !Physics.Raycast(positionToRaycastDownOn, Vector3.down, out hit, 1)) {
            blockPowerupCnt--;
            UpdatePowerupCnt();
            return true;
        }
        return false;
    }
    private void UpdatePowerupCnt() {
        GetComponent<Text>().text = blockPowerupCnt.ToString();
    }
}
