using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller<MainInstaller>
{
    [SerializeField]
    private DateTime dateTime;
    [SerializeField]
    private ViewInformation viewInfo;

    public override void InstallBindings()
    {
        Container.Bind<DateTime>().FromInstance(dateTime).AsSingle();
        Container.Bind<ViewInformation>().FromInstance(viewInfo).AsSingle();
    }
}