
using System;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Zenject;

public class ScoreManager
{
    private DataContainer _data;
    private Text _scoreText;
    private SignalBus _signalBus;

    private int _scoreCount;
    public ScoreManager(SignalBus signalBus, DataContainer dataContainer, Text text)
    {
        _data = dataContainer;
        _signalBus = signalBus;
        _scoreText = text;
        _signalBus.Subscribe<GameStartSignal>(OnStart);
        _signalBus.Subscribe<GameOverSignal>(OnOver);
        _signalBus.Subscribe<EnemyDestroyedSignal>(AddScore);

    }

    private void AddScore(EnemyDestroyedSignal args)
    {
        if (args.Score == 0)
            return;
        int addScore = args.Score;
        SetScore(_scoreCount + addScore);
    }

    private void OnOver()
    {
        
    }

    private void OnStart()
    {
        SetScore(0);
    }
    private void SetScore(int score)
    {
        _data.BestScore = score;
        _scoreCount = score;
        _scoreText.text = score.ToString();
    }
}