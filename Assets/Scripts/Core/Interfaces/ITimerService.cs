using System;
using UniRx;

public interface ITimerService
{
    IReadOnlyReactiveProperty<TimeSpan> RemainingTime { get; }
    IReadOnlyReactiveProperty<bool> IsRunning { get; }
    IObservable<Unit> Finished { get; }

    void SetDuration(TimeSpan duration);
    void Start();
    void Pause();
    void Reset();
}
