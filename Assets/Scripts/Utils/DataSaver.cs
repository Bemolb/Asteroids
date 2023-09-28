using UnityEngine;

public class DataSaver
{
    public void SaveData(DataContainer data)
    {
        PlayerPrefs.SetInt("PlayerBestScore", data.BestScore);
        PlayerPrefs.Save();
    }
    public void LoadData(DataContainer dataContainer)
    {
        dataContainer.BestScore = PlayerPrefs.GetInt("PlayerBestScore", 0);
    }
}
