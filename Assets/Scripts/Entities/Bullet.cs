using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IProjectile
{
    public new Rigidbody2D rigidbody { get; private set; }

    private float _speed;
    private int _maxLifetime;

    private SignalBus _signalBus; 
    private CancellationTokenSource _despawnCancellationTokenSource;
    [Inject]
    private void Construct(GameConfig config, SignalBus signalBus)
    {
        _speed =  config.Speed;
        _maxLifetime = config.MaxLifeTime;
        _signalBus = signalBus;
    }
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        _signalBus.Subscribe<PlayerDeathSignal>(Destroy);
    }

    public void Shoot(Vector3 position, Quaternion rotation, Vector2 direction)
    {
        gameObject.SetActive(true);
        transform.position = position; 
        transform.rotation = rotation;
        rigidbody.AddForce(direction * _speed);
        _despawnCancellationTokenSource = new CancellationTokenSource();
        UniTask.RunOnThreadPool(() => DespawnDelay(_despawnCancellationTokenSource.Token));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Despawn();
    }
    private async UniTaskVoid DespawnDelay(CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_maxLifetime), cancellationToken: cancellationToken);
            if (gameObject.activeSelf)
                Despawn();
        }
        catch (Exception)
        {
            return;
        }
    }
    private void Despawn()
    {
        gameObject.SetActive(false);
        _signalBus.Fire(new ProjectileDespawnSignal { projectile = this });
    }
    private void Destroy()
    {
        _despawnCancellationTokenSource?.Cancel();
        _despawnCancellationTokenSource?.Dispose();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<PlayerDeathSignal>(Destroy);
    }
}
