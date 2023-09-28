using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    public override void InstallBindings()
    {
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
