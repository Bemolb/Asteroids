using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller
{
    [SerializeField] private Player _playerPref;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private ParticleSystem _explousionEffect;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private Text _livesText;
    public override void InstallBindings()
    {
        Player player = Container.InstantiatePrefabForComponent<Player>(_playerPref, _playerSpawnPoint.position, Quaternion.identity, null);
        Container.BindInterfacesAndSelfTo<Player>().FromInstance(player).AsSingle();
        Container.Bind<MovementHandler>().AsSingle().NonLazy();
        Container.Bind<ActionHandler>().AsSingle().NonLazy();

        Container.Bind<UIManager>().AsSingle().WithArguments(_gameOverUI, _livesText).NonLazy();
        Container.Bind<ScoreManager>().AsSingle().WithArguments(_scoreText).NonLazy();
        Container.Bind<VFXManager>().AsSingle().WithArguments(_explousionEffect).NonLazy();
        Container.BindFactory<IProjectile, ProjectileFactory>().WithFactoryArguments(_projectilePrefab).FromComponentInNewPrefab(_projectilePrefab).AsSingle().NonLazy();
        Container.BindFactory<Enemy, EnemyFactory>().WithFactoryArguments(_enemyPrefab).FromComponentInNewPrefab(_enemyPrefab).AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
    }
}
