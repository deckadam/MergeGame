using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{
    public int level=1;
    public float scaleMultiplier;
    private ParticleSystem pSystem;
    private TextMeshPro textPro;

    //Set variables and position itself on the parent object
    private void Start()
    {
        pSystem = gameObject.GetComponent<ParticleSystem>();
        setPosition();
        textPro = gameObject.GetComponent<TextMeshPro>();
    }

    private void ifEmpty()
    {
        if(textPro == null){
            textPro = gameObject.GetComponent<TextMeshPro>();
        }
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
        textPro.text = level.ToString();
    }

    //Set the block to loaded level
    public void loadFromSave(int level)
    {
        this.level = level;
        levelUpBlock(level);
    }

    //Level up the block to certain level and set position again for matching the parent
    private void levelUpBlock(int level)
    {
        ifEmpty();
        textPro.text = level.ToString();
    }


    //Position itself on top of the parent dock for visual purpose
    public void setPosition()
    {
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - Camera.main.transform.position);
    }
}
