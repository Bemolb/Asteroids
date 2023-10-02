using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIManager
{
    private GameObject _gameOverUI;
    private Text _livesText;
    private SignalBus _bus;
    private int _lives;
    public UIManager(SignalBus signalBus, GameObject ui, Text text)
    {
        _gameOverUI = ui;
        _livesText = text;
        _bus = signalBus;
        _bus.Subscribe<GameStartSignal>(OnStart);
        _bus.Subscribe<GameOverSignal>(OnOnver);
        _bus.Subscribe<PlayerDeathSignal>(OnPlayerDeath);
    }

    private void OnPlayerDeath()
    {
        SetLives(_lives - 1);
    }

    private void OnOnver()
    {
        _gameOverUI.SetActive(true);
        SetLives(0);
    }

    private void OnStart()
    {
        _gameOverUI.SetActive(false);
        SetLives(3);
    }
    private void SetLives(int lives)
    {
        _lives = lives;
        _livesText.text = lives.ToString();
    }
}