using UniRx;
using System;

public interface IStopwatchService
{
    IReadOnlyReactiveProperty<TimeSpan> ElapsedTime { get; }
    IReadOnlyReactiveCollection<TimeSpan> Laps { get; }
    IReadOnlyReactiveProperty<bool> IsRunning { get; }

    void Start();
    void Pause();
    void Reset();
    void Lap();
}