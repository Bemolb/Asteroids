using Assets.Scripts.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

internal class ProjectileFactory : PlaceholderFactory<IProjectile>
{
    private readonly DiContainer _container;
    private readonly SignalBus _signalBus;
    private readonly object _lock = new object();
    private GameObject _poolObject;
    private GameObject _prefab;

    private HashSet<IProjectile> destroyedProjectiles = new HashSet<IProjectile>();

    public ProjectileFactory(DiContainer container, SignalBus signalBus, GameObject projectilePrefab)
    {
        _container = container;
        _signalBus = signalBus;
        _prefab = projectilePrefab;
        Initialize();
    }

    private void Initialize()
    {
        _signalBus.Subscribe<ProjectileDespawnSignal>(Destroy);
        _signalBus.Subscribe<PlayerAttackSignal>(Spawn);
        _signalBus.Subscribe<RespawnSignal>(Clear);
        _poolObject = new GameObject("[Pool]Projectiles");
    }

    private void Spawn(PlayerAttackSignal args)
    {
        IProjectile projectile = Create();
        projectile.Shoot(args.Position, args.Rotation, args.Direction);
    }

    public override IProjectile Create()
    {
        IProjectile projectile;
        lock (_lock)
        {
            if (destroyedProjectiles.Count == 0)
            {
                projectile = _container.InstantiatePrefabForComponent<IProjectile>(_prefab, _poolObject.transform);
            }
            else
            {
                projectile = destroyedProjectiles.First();
                destroyedProjectiles.Remove(projectile);
            }
            return projectile;
        }
    }

    private void Destroy(ProjectileDespawnSignal args)
    {
        lock (_lock)
        {
            destroyedProjectiles.Add(args.projectile);
        }
    }

    private void Clear()
    {
        destroyedProjectiles.Clear();
    }
}