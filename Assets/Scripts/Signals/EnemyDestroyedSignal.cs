using UnityEngine;

internal class EnemyDestroyedSignal
{
    public Enemy Enemy;
    public int Score;
    public Transform? Transform;
    public float Size;
    public bool NeedSplit = false;
}