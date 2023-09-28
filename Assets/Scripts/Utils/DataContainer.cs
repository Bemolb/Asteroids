using System;
public class DataContainer : IDisposable
{
    private int _bestScore = 0;
    private readonly DataSaver _saver;
    public int BestScore { get => _bestScore; set => ValidateNewScore(value); }
    public DataContainer(DataSaver dataSaver)
    {
        _saver = dataSaver;
        _saver.LoadData(this);
    }

    public void ValidateNewScore(int newScore)
    {
        _bestScore = newScore > _bestScore ? newScore : _bestScore;
    }
    public void Dispose()
    {
        _saver.SaveData(this);
    }
}
