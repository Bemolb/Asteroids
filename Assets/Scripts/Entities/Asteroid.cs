using NTC.Pool;
using UnityEngine;
using Zenject;

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
            EnemyDestroyedSignal signal = new EnemyDestroyedSignal()
            {
                Enemy = this,
                Score = GetScore(),
                Transform = gameObject.transform
        };
            if ((size * 0.5f) >= _minSize)
            {
                signal.NeedSplit = true;
                signal.Size = size;
            }
            gameObject.SetActive(false);
            _isVisible = false;
            _signalBus.Fire(signal);
        }
    }
    protected override void OnSpawn(float size = 0)
    {
        float newSize = size == 0 ? Random.Range(_minSize, _maxSize) : size;
        SetSize(newSize);
    }

    private void SetSize(float newSize)
    {
        size = newSize;
        transform.localScale = Vector3.one * size;
        rigidbody.mass = size;
    }

    protected override int GetScore()
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
