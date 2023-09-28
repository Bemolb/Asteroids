using UnityEngine;
using NTC.Pool;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour, ISpawnable
{
    public new Rigidbody2D rigidbody { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;

    private float _movementSpeed = 50f;
    private float _maxLifetime = 30;
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
        NightPool.Despawn(gameObject, _maxLifetime);
    }
    public void SetTrajectory(Vector2 direction)
    {
        rigidbody.AddForce(direction * _movementSpeed);
    }
    public abstract int GetScore();
    public abstract void OnSpawn();
}
