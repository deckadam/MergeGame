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
        DateSave.setTimeToWait(timeToWait);
        blocks = new List<Block>();

        //Start point generation coroutine with timer
        StartCoroutine(generateWithTime());
        //Load last save time and block data
        DateSave.load();
      
    }

    //Coroutine for timed point generation
    public static int totalPoint;
    IEnumerator generateWithTime()
    {
        while (true)
        {
            foreach (Block block in blocks)
            {
                totalPoint += block.level;
            }
            pointLabel.text = "Points = " + totalPoint.ToString();
            yield return new WaitForSecondsRealtime(timeToWait);
        }
    }

    void OnApplicationQuit()
    {
        DateSave.save(System.DateTime.Now);
    }

}
