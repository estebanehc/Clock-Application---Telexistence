using System;
using UniRx;

public class TimerModel
{
    public ReactiveProperty<TimeSpan> RemainingTime { get; }
    public ReactiveProperty<bool> IsRunning { get; }

    public TimerModel()
    {
        RemainingTime = new ReactiveProperty<TimeSpan>(TimeSpan.Zero);
        IsRunning = new ReactiveProperty<bool>(false);
    }
}