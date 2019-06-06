using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int level;
    public GameObject nextLevel;

    public bool isMergeble(GameObject temp) {
        Block block = temp.GetComponent<Block>();
        if (block.level == this.level) return true;
        else return false;
    }

    public string getBlockName() {
        return gameObject.transform.parent.transform.parent.name;
    }

    public bool merge(GameObject other) {
        if (other != gameObject)
        {
            Debug.Log(PointGeneration.blocks.Remove(other.GetComponent<Block>()));
            Destroy(other);
            gameObject.transform.localScale *= 2.0f;
            level++;
            return true;
        }
        else return false;
    }
}
