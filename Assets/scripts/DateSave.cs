using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public static class DateSave
{
    public static void save(System.DateTime time)
    {
        Debug.Log(Application.persistentDataPath);
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Application.persistentDataPath + "/data.bin";
        FileStream saveStream = new FileStream(savePath, FileMode.Create);
        GameData temp = new GameData(time.ToString());
        formatter.Serialize(saveStream, temp);
        saveStream.Close();
    }

    public static int load()
    {
        string loadPath = Application.persistentDataPath + "/data.bin";
        if (File.Exists(loadPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream loadStream = new FileStream(loadPath, FileMode.Open);
            GameData data =formatter.Deserialize(loadStream) as GameData;
            System.DateTime current = System.DateTime.Now;
            System.DateTime loadedDate = System.Convert.ToDateTime(data.time);
            System.TimeSpan timeDifference = current.Subtract(loadedDate);
            return timeDifference.Seconds;
        }
        else
        {
            return -1;
        }
    }

    [System.Serializable]
    class GameData
    {
        public string time;
        public GameData(string time)
        {
            this.time = time;
        }
    }
}
