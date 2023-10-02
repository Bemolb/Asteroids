using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    [field: SerializeField] public float RotationSpeed { get; private set; } = 0.1f;
    [field: SerializeField] public float RespawnDelay { get; private set; } = 3f;
    [field: SerializeField] public float RespawnInvulnerability { get; private set; } = 3f;
    [field: SerializeField] public float ThrustSpeed { get; private set; } = 1f;
    [field: SerializeField] public bool ScreenWrapping { get; private set; } = true;

    [field: SerializeField] public float SpawnDistance { get; private set; } = 12f;
    [field: SerializeField] public float SpawnRate { get; private set; } = 1f;
    [field: SerializeField] public int AmountPerSpawn { get; private set; } = 1;
    [field: SerializeField] public Enemy EnemyPrefab { get; private set; }
    [field: SerializeField, Range(0f, 45f)] public float TrajectoryVariance { get; private set; } = 15f;


    [field: SerializeField] public float Speed { get; private set; } = 500f;
    [field: SerializeField] public int MaxLifeTime { get; private set; } = 10;


}
