using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private StopwatchView stopwatchView;

    public override void InstallBindings()
    {
        Container.Bind<IClockService>().To<ClockService>().AsSingle();
        Container.Bind<ClockModel>().AsSingle();

        Container.Bind<ITimerService>().To<TimerService>().AsSingle();
        Container.Bind<TimerModel>().AsSingle();

        Container.Bind<IStopwatchView>()
            .FromInstance(stopwatchView)
            .AsSingle();

        Container.Bind<IStopwatchService>()
            .To<StopwatchService>()
            .AsSingle();

        Container.Bind<StopwatchPresenter>()
            .AsSingle()
            .NonLazy();

        Container.Bind<StopwatchModel>()
            .AsSingle();
    }
}
