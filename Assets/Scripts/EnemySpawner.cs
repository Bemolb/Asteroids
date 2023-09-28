using Cysharp.Threading.Tasks;
using NTC.Pool;
using System;
using UnityEngine;
using Zenject;
public class EnemySpawner
{
    private float _spawnDistance;
    private float _spawnRate;
    private int _amountPerSpawn;
    private float _trajectoryVariance;

    private Enemy _enemyPrefab;
    [Inject]
    public EnemySpawner(GameConfig config)
    {
        _spawnDistance = config.SpawnDistance;
        _spawnRate = config.SpawnRate;
        _amountPerSpawn = config.AmountPerSpawn;
        _trajectoryVariance = config.TrajectoryVariance; 
        _enemyPrefab = config.EnemyPrefab;
        UniTask.RunOnThreadPool(Start);
    }
    private async UniTaskVoid Start()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_spawnRate));
        SpawnLoop();
    }

    private async void SpawnLoop()
    {
        while (true)
        {
            Spawn();
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnRate));
        }
    }
    private void Spawn()
    {
        for (int i = 0; i < _amountPerSpawn; i++)
        {
            Vector3 spawnDirection = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = Vector3.zero + (spawnDirection * _spawnDistance);
            float variance = UnityEngine.Random.Range(-_trajectoryVariance, _trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
            Vector2 trajectory = rotation * -spawnDirection;
            Enemy enemy = NightPool.Spawn(_enemyPrefab, spawnPoint, rotation);
            enemy.SetTrajectory(trajectory);
        }
    }

}
