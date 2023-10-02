using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class GameManager : ITickable, IInitializable
{
    public static GameManager Instance { get; private set; }
    private int _lives;

    private float _respawnDelay;
    private SignalBus _signalBus;
    public GameManager(GameConfig gameConfig, SignalBus signalBus)
    {
        _respawnDelay = gameConfig.RespawnDelay;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<PlayerDeathSignal>(OnPlayerDeath);
        NewGame();
    }

    public void Tick()
    {
        if (_lives <= 0 && Input.GetKeyDown(KeyCode.Return)) {
            NewGame();
        }
    }

    private void NewGame()
    {
        _signalBus.Fire<GameStartSignal>();
        SetLives(3);
        Respawn();
    }

    private void SetLives(int lives)
    {
        _lives = lives;
    }

    private async void Respawn(float respawnDelay = 0)
    {
        if (_respawnDelay != 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(respawnDelay));
        }
        _signalBus.Fire<RespawnSignal>();
    }

    private void OnPlayerDeath(PlayerDeathSignal args)
    {
        SetLives(_lives - 1);

        if (_lives <= 0) {
            _signalBus.Fire<GameOverSignal>();
        } else {
            UniTask.RunOnThreadPool(() => Respawn(_respawnDelay));
        }
    }
}
