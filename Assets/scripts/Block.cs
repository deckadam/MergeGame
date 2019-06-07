using UnityEngine;

public class Block : MonoBehaviour
{
    public int level;
    public GameObject nextLevel;
    public float scaleMultiplier;

    public bool isMergeble(GameObject temp)
    {
        Block block = temp.GetComponent<Block>();
        if (block.level == this.level && temp != gameObject) return true;
        else return false;
    }

    public void merge(GameObject other)
    {
        Destroy(other);
        loadBlock(1);
        level++;
    }

    public void loadFromSave(int level)
    {
        loadBlock(level - 1);
        this.level = level;
    }

    private void loadBlock(int level)
    {
        for (int i = 0; i < level; i++)
        {
            gameObject.transform.localScale *= scaleMultiplier;
        }
    }
}
