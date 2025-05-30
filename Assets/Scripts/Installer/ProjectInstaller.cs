using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private StopwatchView stopwatchView;
    [SerializeField] private ClockView clockView;
    [SerializeField] private TimerView timerView;

    public override void InstallBindings()
    {
        Container.Bind<IClockView>()
            .FromInstance(clockView)
            .AsSingle();

        Container.Bind<IClockService>()
            .To<ClockService>()
            .AsSingle();

        Container.Bind<ClockPresenter>()
            .AsSingle()
            .NonLazy();

        Container.Bind<ClockModel>()
            .AsSingle();

        Container.Bind<ITimerView>()
            .To<TimerView>()
            .FromInstance(timerView)
            .AsSingle();
        
        Container.Bind<TimerPresenter>()
            .AsSingle()
            .NonLazy();

        Container.Bind<ITimerService>()
            .To<TimerService>()
            .AsSingle();

        Container.Bind<TimerModel>()
            .AsSingle();

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
