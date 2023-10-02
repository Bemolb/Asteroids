using Assets.Scripts.Signals;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<ProjectileDespawnSignal>();
        Container.DeclareSignal<PlayerAttackSignal>();
        Container.DeclareSignal<PlayerDeathSignal>();
        Container.DeclareSignal<RespawnSignal>();
        Container.DeclareSignal<EnemyDestroyedSignal>();
        Container.DeclareSignal<GameStartSignal>();
        Container.DeclareSignal<GameOverSignal>();
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        Container.Bind<DataSaver>().AsSingle();
        Container.BindInterfacesAndSelfTo<DataContainer>().AsSingle();
        Container.Bind<SceneLoader>().AsSingle();
        if (SystemInfo.deviceType == DeviceType.Handheld)
            Container.BindInterfacesAndSelfTo<MobileInput>().AsSingle();
        else
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle().NonLazy();
    }
}
