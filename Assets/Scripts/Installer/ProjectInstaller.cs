using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IClockService>().To<ClockService>().AsSingle();
        Container.Bind<ClockModel>().AsSingle();
    }
}
