using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointGeneration : MonoBehaviour
{
    //Start the coroutine for point generation
    [SerializeField]
    private float timeToWait = 5.0f;
    [SerializeField]
    private Text pointLabel;
    //For using on editor mode
    [SerializeField]
    private Button saveButton;

    public static List<Block> blocks;

    void Start()
    {
        blocks = new List<Block>();

        //Start point generation coroutine with timer
        StartCoroutine(generateWithTime());
        //Load last save time and block data
        int result = DateSave.load();
        if (result != -1)
        {

        }
    }

    //For saving data on editor mode
    public void saveData() {
        DateSave.save(System.DateTime.Now);
    }

    //Coroutine for timed point generation
    private int totalPoint;
    IEnumerator generateWithTime()
    {
        while (true)
        {
            foreach (Block block in blocks)
            {
                Debug.Log(block.level);
                totalPoint += block.level;
            }
            pointLabel.text = "Points = " + totalPoint.ToString();
            yield return new WaitForSecondsRealtime(timeToWait);
        }
    }

    //Save current state on quit (Not in editor mode)
    void OnApplicationQuit()
    {
        DateSave.save(System.DateTime.Now);
    }

}
