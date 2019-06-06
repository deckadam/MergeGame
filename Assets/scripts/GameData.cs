[System.Serializable]
public class GameData
{
    public string timeVal;
    public string blockData;
    public int totalPoint;

    public GameData(string timeVal, string blockData, int totalPoint)
    {
        this.timeVal = timeVal;
        this.blockData = blockData;
        this.totalPoint = totalPoint;
    }

    public string[] getAllData() {
        return new string[]{timeVal, blockData, totalPoint.ToString()};
    }
}
