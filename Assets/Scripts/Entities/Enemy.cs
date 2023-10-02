using UnityEngine;
using NTC.Pool;
using Cysharp.Threading.Tasks;
using Zenject;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;

    protected GameConfig _gameConfig;
    protected SignalBus _signalBus;
    private float _movementSpeed = 50f;

    protected bool _isVisible = false;
    [Inject]
    private void Construct(GameConfig gameConfig, SignalBus signalBus)
    {
        _gameConfig = gameConfig;
        _signalBus = signalBus;

    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        gameObject.tag = "Enemy";
    }
    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        _signalBus.Subscribe<GameStartSignal>(Destroy);
    }
    public void Init(Vector3 position, Quaternion rotation, Vector2 direction, float size = 0)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;
        OnSpawn(size);
        SetTrajectory(direction);
    }
    private void SetTrajectory(Vector2 direction)
    {
        rigidbody.AddForce(direction * _movementSpeed);
    }

    private void OnBecameVisible()
    {
        _isVisible = true;
    }
    private void OnBecameInvisible()
    {
        if (_isVisible)
        {
            _isVisible = false;
            gameObject.SetActive(false);
            _signalBus.Fire(new EnemyDestroyedSignal() { Enemy = this, Score = 0});
        }
    }
    private void Destroy()
    {
        _isVisible = false;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GameStartSignal>(Destroy);
    }
    protected abstract int GetScore();
    protected abstract void OnSpawn(float size = 0);
}
