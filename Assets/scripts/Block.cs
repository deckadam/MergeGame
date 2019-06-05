using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int level;
    public GameObject nextLevel;
    public bool dragging=false;

    public bool isMergeble(GameObject temp) {
        Block block = temp.GetComponent<Block>();
        if (block.level == this.level) return true;
        else return false;
    }

    public void merge(GameObject other) {
        Debug.Log("Merged");
        Destroy(other);
        gameObject.transform.localScale *= 2.0f;
        level++;
    }
}
