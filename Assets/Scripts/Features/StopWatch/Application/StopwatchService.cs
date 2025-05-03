using System;
using UniRx;
using Zenject;

public class StopwatchService : IStopwatchService
{
    private readonly StopwatchModel stopwatchModel;
    private IDisposable timerSubscription;
    private DateTime startTime;
    private  TimeSpan pausedTime = TimeSpan.Zero;

    public IReadOnlyReactiveProperty<TimeSpan> ElapsedTime => stopwatchModel.ElapsedTime;
    public IReadOnlyReactiveCollection<TimeSpan> Laps => stopwatchModel.Laps;
    public IReadOnlyReactiveProperty<bool> IsRunning => stopwatchModel.IsRunning;

    [Inject]
    public StopwatchService(StopwatchModel stopwatchModel)
    {
        this.stopwatchModel = stopwatchModel;
    }

    public void Start()
    {
        if (IsRunning.Value) return;

        stopwatchModel.IsRunning.Value = true;
        startTime = DateTime.Now;

        timerSubscription = Observable
            .Interval(TimeSpan.FromMilliseconds(10))
            .Subscribe(_ =>
            {
                var current = DateTime.Now - startTime + pausedTime;
                stopwatchModel.ElapsedTime.Value = current;
            });
    }

    public void Pause()
    {
        if (!IsRunning.Value) return;

        stopwatchModel.IsRunning.Value = false;
        pausedTime = stopwatchModel.ElapsedTime.Value;
        timerSubscription?.Dispose();
    }

    public void Reset()
    {
        stopwatchModel.IsRunning.Value = false;
        stopwatchModel.ElapsedTime.Value = TimeSpan.Zero;
        pausedTime = TimeSpan.Zero;
        stopwatchModel.Laps.Clear();
        timerSubscription?.Dispose();
    }

    public void Lap()
    {
        if (!IsRunning.Value)return;
        stopwatchModel.Laps.Insert(0, stopwatchModel.ElapsedTime.Value);
    }

    public void Dispose()
    {
        timerSubscription?.Dispose();
        stopwatchModel.ElapsedTime.Dispose();
        stopwatchModel.IsRunning.Dispose();
        stopwatchModel.Laps.Clear();
    }
}
