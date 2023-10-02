using UnityEngine;

public interface IProjectile
{
    public void Shoot(Vector3 position, Quaternion rotation, Vector2 direction);
}