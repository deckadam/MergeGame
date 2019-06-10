using UnityEngine;

public class Block : MonoBehaviour
{
    public int level;
    public float scaleMultiplier;
    private ParticleSystem pSystem;

    //Set variables and position itself on the parent object
    private void Start()
    {
        pSystem = gameObject.GetComponent<ParticleSystem>();
        setPosition();
    }

    //Check if it is possible to merge with the sended object
    public bool isMergeble(GameObject temp)
    {
        Block block = temp.GetComponent<Block>();
        if (block.level == this.level && temp != gameObject) return true;
        else return false;
    }

    //Merge itself with the sended object set the level, size and position,
    //Update BuyMenu maxlevel and update buttons to match the current level unlocked
    public void merge(GameObject other)
    {
        gameObject.transform.parent = other.transform.parent;
        setPosition();
        Destroy(other);
        level++;
        levelUpBlock(1);
        pSystem.Play();
        if (level > BuyMenu.maxLevelReached)
        {
            BuyMenu.maxLevelReached = level;
            BuyMenu.updateButtons();
        }
    }

    //Set the block to loaded level
    public void loadFromSave(int level)
    {
        this.level = level;
        levelUpBlock(level - 1);
    }

    //Level up the block to certain level and set position again for matching the parent
    private void levelUpBlock(int level)
    {
        for (int i = 0; i < level; i++)
        {
            transform.localScale *= scaleMultiplier;
        }
        setPosition();

    }


    //Position itself on top of the parent dock for visual purpose
    public void setPosition()
    {
        float yPos = 0.15f;
        for (int i = 0; i < this.level; i++)
        {
            yPos *= 1.2f;
        }
        Vector3 newPosition = new Vector3(0, yPos, 0);
        gameObject.transform.position = gameObject.transform.parent.transform.position + newPosition;
    }
}
