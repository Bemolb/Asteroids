
using Assets.Scripts.Signals;
using Cysharp.Threading.Tasks;
using NTC.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

public class EnemyFactory : PlaceholderFactory<Enemy>
{
    private readonly DiContainer _container;
    private readonly SignalBus _signalBus;
    private readonly object _lock = new object();
    private GameObject _poolObject;
    private GameObject _prefab;


    private float _spawnDistance;
    private float _spawnRate;
    private int _amountPerSpawn;
    private float _trajectoryVariance;
    private CancellationTokenSource _spawnCancellationTokenSource;
    private bool _isSpawning;

    private HashSet<Enemy> destroyedEnemies;

    public EnemyFactory(DiContainer container, SignalBus signalBus, GameObject enemyPrefab, GameConfig config)
    {
        _container = container;
        _signalBus = signalBus;
        _prefab = enemyPrefab;

        _spawnDistance = config.SpawnDistance;
        _spawnRate = config.SpawnRate;
        _amountPerSpawn = config.AmountPerSpawn;
        _trajectoryVariance = config.TrajectoryVariance;
        Initialize();
    }

    private void Initialize()
    {
        _signalBus.Subscribe<EnemyDestroyedSignal>(Destroy);
        _signalBus.Subscribe<GameStartSignal>(Start);
        _signalBus.Subscribe<GameOverSignal>(Clear);
        _poolObject = new GameObject("[Pool]Enemies");
    }

    private void Start()
    {
        _spawnCancellationTokenSource = new CancellationTokenSource();
        _isSpawning = true;
        destroyedEnemies = new HashSet<Enemy>();
        UniTask.RunOnThreadPool(() => SpawnLoop(_spawnCancellationTokenSource.Token));
    }

    private async UniTaskVoid SpawnLoop(CancellationToken cancellationToken)
    {
        while (true)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnRate), cancellationToken: cancellationToken);
                if(_isSpawning)
                    Spawn();
            }
            catch (Exception)
            {
                return;
            }
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
            Enemy enemy = Create();
            enemy.Init(spawnPoint, rotation, trajectory);
        }
    }
    private void SpawnSplit(Transform transform, float size)
    {
        Enemy enemy = Create(); 
        Vector2 position = transform.position;
        position += UnityEngine.Random.insideUnitCircle * 0.5f;
        enemy.Init(position, transform.rotation, UnityEngine.Random.insideUnitCircle.normalized, size * 0.5f);

    }

    public override Enemy Create()
    {
        Enemy projectile;
        lock (_lock)
        {
            if (destroyedEnemies.Count == 0)
            {
                projectile = _container.InstantiatePrefabForComponent<Enemy>(_prefab, _poolObject.transform);
            }
            else
            {
                projectile = destroyedEnemies.First();
                destroyedEnemies.Remove(projectile);
            }
            return projectile;
        }
    }

    private void Destroy(EnemyDestroyedSignal args)
    {
        if (args.NeedSplit)
        {
            SpawnSplit(args.Transform, args.Size);
            SpawnSplit(args.Transform, args.Size);
        }
        destroyedEnemies.Add(args.Enemy);
    }

    private void Clear()
    {
        _isSpawning = false;
        _spawnCancellationTokenSource?.Cancel();
        _spawnCancellationTokenSource?.Dispose();
        destroyedEnemies.Clear();
    }
}