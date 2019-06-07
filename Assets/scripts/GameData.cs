[System.Serializable]
public class GameData
{
    public string timeVal;//Time value when game quited
    public string blockData;//Block information that is on the docks
    public int totalPoint;//Last updated point value

    public GameData(string timeVal, string blockData, int totalPoint)
    {
        this.timeVal = timeVal;
        this.blockData = blockData;
        this.totalPoint = totalPoint;
    }

    //Returning all the deserialized data with a string array
    public string[] getAllData() {
        return new string[]{timeVal, blockData, totalPoint.ToString()};
    }
}
