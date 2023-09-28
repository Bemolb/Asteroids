using NTC.Pool;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : Enemy
{
    private float size = 1f;
    private float _minSize = 0.35f;
    private float _maxSize = 1.65f;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if ((size * 0.5f) >= _minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            GameManager.Instance.OnAsteroidDestroyed(this);
            NightPool.Despawn(gameObject);
        }
    }

    private void CreateSplit()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;
        Asteroid prefab = Resources.Load<Asteroid>("Prefabs/Asteroid"); //это определенно плохо, но так как Inject конфига по непонятным причинам не работает - выхода нет
        Asteroid half = NightPool.Spawn(prefab, position, transform.rotation);
        half.SetTrajectory(Random.insideUnitCircle.normalized);
        half.SetSize(size * 0.5f);
    }
    public override void OnSpawn()
    {
        float newSize = Random.Range(_minSize, _maxSize);
        SetSize(newSize);
    }

    public void SetSize(float newSize)
    {
        size = newSize;
        transform.localScale = Vector3.one * size;
        rigidbody.mass = size;
    }

    public override int GetScore()
    {
        int score;
        if (size < 0.7f)
            score = 100;
        else if (size < 1.4f)
            score = 50;
        else
            score = 25;
        return score;
    }
}
