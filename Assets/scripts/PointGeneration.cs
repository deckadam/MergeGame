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
    private static Text staticText;
    private static string template = "Points = ";
    public static int totalPoint;
    public static List<Block> blocks;

    void Start()
    {
        staticText = pointLabel;
        blocks = new List<Block>();

        //Start point generation coroutine with timer
        StartCoroutine(generateWithTime());
        //Load last save time and block data
        DateSave.setTimeToWait(timeToWait);
        Invoke("invokeDelay", 0.1f);
    }

    //Display points after all loading is done (waiting for all start functions to complete)
    private void invokeDelay() {
        DateSave.load();
        pointLabel.text = template + totalPoint.ToString();
    }


    //Update point text when something changed
    public static void immediateUpdate() {
        staticText.text = template +totalPoint.ToString();
    }

    //Coroutine for timed point generation
    IEnumerator generateWithTime()
    {
        while (true)
        {
            foreach (Block block in blocks)
            {
                totalPoint += block.level;
            }
            pointLabel.text = template + totalPoint.ToString();
            yield return new WaitForSecondsRealtime(timeToWait);
        }
    }

    //Save the current state of the game
    void OnApplicationQuit()
    {
        DateSave.save(System.DateTime.Now);
    }

}
