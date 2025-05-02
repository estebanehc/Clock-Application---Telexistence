using System;
using UniRx;
using Zenject;

public class TimerService : ITimerService
{
    private readonly TimerModel timerModel;
    private TimeSpan initialDuration;
    private IDisposable timerDisposable;
    private readonly Subject<Unit> finishedSubject = new();

    public IReadOnlyReactiveProperty<TimeSpan> RemainingTime => timerModel.RemainingTime;
    public IReadOnlyReactiveProperty<bool> IsRunning => timerModel.IsRunning;
    public IObservable<Unit> Finished => finishedSubject;

    [Inject]
    public TimerService(TimerModel timerModel)
    {
        this.timerModel = timerModel;
    }

    public void Start()
    {
        if(timerModel.IsRunning.Value 
        || timerModel.RemainingTime.Value <= TimeSpan.Zero)
            return;

        timerModel.IsRunning.Value = true;
        timerDisposable = Observable
            .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                var newTime = timerModel.RemainingTime.Value - TimeSpan.FromSeconds(1);
                if (newTime <= TimeSpan.Zero)
                {
                    timerModel.RemainingTime.Value = TimeSpan.Zero;
                    InternalStop();
                    finishedSubject.OnNext(Unit.Default);
                }
                else
                {
                    timerModel.RemainingTime.Value = newTime;
                }
            });
    }

    public void Pause()
    {
        if (!timerModel.IsRunning.Value)
            return;
        InternalStop();
    }

    public void Reset()
    {
        InternalStop();
        timerModel.RemainingTime.Value = initialDuration;
    }

    public void SetDuration(TimeSpan duration)
    {
        initialDuration = duration;
        timerModel.RemainingTime.Value = duration;
    }

    private void InternalStop()
    {
        timerDisposable?.Dispose();
        timerModel.IsRunning.Value = false;
    }

    public void Dispose()
    {
        timerDisposable?.Dispose();
        finishedSubject.Dispose();
        timerModel.RemainingTime.Dispose();
        timerModel.IsRunning.Dispose();
    }
}
