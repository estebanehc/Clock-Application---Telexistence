using UniRx;
using System;

public class ClockModel
{
    public ReactiveProperty<DateTime> CurrentTime { get; private set; }

    public ClockModel()
    {
        CurrentTime = new ReactiveProperty<DateTime>(DateTime.Now);
    }
}