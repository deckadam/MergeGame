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

    public bool merge(GameObject other) {
        if (other != gameObject)
        {
            Destroy(other);
            loadBlock(1);
            level++;
            return true;
        }
        else return false;
    }

    public void loadFromSave(int level) {
        loadBlock(level-1);
        this.level = level;
    }

    private void loadBlock(int level) {
        for (int i = 0; i < level; i++)
        {
            gameObject.transform.localScale *= 2.0f;
        }
    }
}
