using UniRx;
using System;

public class StopwatchPresenter : IDisposable
{
    private readonly IStopwatchService stopwatchService;
    private readonly IStopwatchView stopwatchView;
    private readonly CompositeDisposable disposables = new();

    public StopwatchPresenter(IStopwatchService stopwatchService, IStopwatchView stopwatchView)
    {
        this.stopwatchService = stopwatchService;
        this.stopwatchView = stopwatchView;

        BindView();
        BindService();
    }

    private void BindView()
    {
        stopwatchView.OnStartClicked.Subscribe(_ => stopwatchService.Start()).AddTo(disposables);
        stopwatchView.OnStopClicked.Subscribe(_ => stopwatchService.Pause()).AddTo(disposables);
        stopwatchView.OnResetClicked.Subscribe(_ => stopwatchService.Reset()).AddTo(disposables);
        stopwatchView.OnLapClicked.Subscribe(_ => stopwatchService.Lap()).AddTo(disposables);
    }

    private void BindService()
    {
        stopwatchService.ElapsedTime
            .Subscribe(time => stopwatchView.SetElapsedTime(time))
            .AddTo(disposables);

        stopwatchService.IsRunning
            .Subscribe(isRunning => stopwatchView.SetRunningState(isRunning))
            .AddTo(disposables);

        stopwatchService.Laps
            .ObserveAdd()
            .Subscribe(addEvent => stopwatchView.AddLap(addEvent.Value))
            .AddTo(disposables);

        stopwatchService.Laps
            .ObserveReset()
            .Subscribe(_ => stopwatchView.ClearLaps())
            .AddTo(disposables);
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}