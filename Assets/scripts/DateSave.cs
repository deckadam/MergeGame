﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public static class DateSave
{
    private static float timeToWait;

    public static void setTimeToWait(float val) {
        timeToWait = val;
    }

    public static void save(System.DateTime time)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Application.persistentDataPath + "/data.bin";
        FileStream saveStream = new FileStream(savePath, FileMode.Create);
        formatter.Serialize(saveStream, makeGameData());
        saveStream.Close();
    }

    private static GameData makeGameData() {
        string blockData="";
        for (int i = 0; i < DockGenerator.docks.GetLength(0); i++)
        {
            for (int j = 0; j < DockGenerator.docks.GetLength(1); j++)
            {
                if (DockGenerator.docks[i, j].transform.GetChild(0).childCount != 0)
                {
                    blockData += ((i + "," + j)+","+ DockGenerator.docks[i, j].transform.GetChild(0).transform.GetChild(0).GetComponent<Block>().level)+"/".ToString();
                }
            }
        }
        return new GameData(System.DateTime.Now.ToString(),blockData, PointGeneration.totalPoint);
    }

    //Load data that has been saved from the last game
    public static void load()
    {
        string loadPath = Application.persistentDataPath + "/data.bin";
        if (File.Exists(loadPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream loadStream = new FileStream(loadPath, FileMode.Open);
            GameData data =formatter.Deserialize(loadStream) as GameData;
            restoreGame(data.getAllData(),data.timeVal, System.Convert.ToDateTime(data.timeVal));
        }
    }

    private static void restoreGame(string[] data,string timeVal,System.DateTime loadedTime) {

        //index1 block data
        ObjectPlacement placer = Resources.FindObjectsOfTypeAll<ObjectPlacement>()[0];
        int totalVal =placer.placeLoadedBlocks(data[1]);

        //Cycle and leftover time calculation
        System.DateTime current = System.DateTime.Now;
        System.DateTime loadedDate = System.Convert.ToDateTime(timeVal);
        System.TimeSpan timeDifference = current.Subtract(loadedDate);
        int totalCycles = timeDifference.Seconds / (int)timeToWait;
        float leftOver = timeDifference.Seconds / timeToWait;
        //index0 times passed
        //Extra time left from cycles transfered to buttons fill amount
        ObjectPlacement.fillAmount = leftOver;

        //index2 total point calculation
        PointGeneration.totalPoint = totalVal * totalCycles;
    }
}
