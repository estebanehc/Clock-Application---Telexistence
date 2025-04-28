using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
