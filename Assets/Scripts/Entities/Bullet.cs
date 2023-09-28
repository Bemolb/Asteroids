using Cysharp.Threading.Tasks;
using NTC.Pool;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public float speed = 500f;
    public int maxLifetime = 10;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction)
    {
        rigidbody.AddForce(direction * speed);
        NightPool.Despawn(gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        NightPool.Despawn(gameObject);
    }
}
