using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class StopwatchService : IStopwatchService
{
    private readonly StopwatchModel stopwatchModel;
    private IDisposable timerSubscription;
    private  TimeSpan currentTime = TimeSpan.Zero;

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
        if (stopwatchModel.IsRunning.Value)
            return;

        stopwatchModel.IsRunning.Value = true;
        timerSubscription = Observable
            .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                currentTime += TimeSpan.FromSeconds(1);
                stopwatchModel.ElapsedTime.Value = currentTime;
            });
    }

    public void Pause()
    {
        if (!stopwatchModel.IsRunning.Value)
            return;

        stopwatchModel.IsRunning.Value = false;
        timerSubscription?.Dispose();
    }

    public void Lap()
    {
        if (!stopwatchModel.IsRunning.Value)
            return;

        stopwatchModel.Laps.Insert(0, currentTime);
        currentTime = TimeSpan.Zero;
    }

    public void Reset()
    {
        stopwatchModel.IsRunning.Value = false;
        stopwatchModel.ElapsedTime.Value = TimeSpan.Zero;
        currentTime = TimeSpan.Zero;
        stopwatchModel.Laps.Clear();
        timerSubscription?.Dispose();
    }

    public void Resume()
    {
        if (stopwatchModel.IsRunning.Value)
            return;

        stopwatchModel.IsRunning.Value = true;
        timerSubscription = Observable
            .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {
                currentTime += TimeSpan.FromSeconds(1);
                stopwatchModel.ElapsedTime.Value = currentTime;
            });
    }

    public void Dispose()
    {
        timerSubscription?.Dispose();
        stopwatchModel.ElapsedTime.Dispose();
        stopwatchModel.IsRunning.Dispose();
        stopwatchModel.Laps.Clear();
    }
}
