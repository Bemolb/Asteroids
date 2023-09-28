using UnityEngine;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller
{
    [SerializeField] private Player _playerPref;
    [SerializeField] private Transform _playerSpawnPoint;
    public override void InstallBindings()
    {
        Player player = Container.InstantiatePrefabForComponent<Player>(_playerPref, _playerSpawnPoint.position, Quaternion.identity, null);
        Container.BindInterfacesAndSelfTo<Player>().FromInstance(player).AsSingle();
        Container.Bind<MovementHandler>().AsSingle().NonLazy();
        Container.Bind<ActionHandler>().AsSingle().NonLazy();

        Container.Bind<EnemySpawner>().AsSingle().NonLazy();
    }
}
