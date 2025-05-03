using UniRx;
using System;

public class StopwatchModel
{
    public ReactiveProperty<TimeSpan> ElapsedTime { get; }
    public ReactiveProperty<bool> IsRunning { get; }
    public ReactiveCollection<TimeSpan> Laps { get; }

    public StopwatchModel()
    {
        ElapsedTime = new ReactiveProperty<TimeSpan>(TimeSpan.Zero);
        IsRunning = new ReactiveProperty<bool>(false);
        Laps = new ReactiveCollection<TimeSpan>();
    }
}