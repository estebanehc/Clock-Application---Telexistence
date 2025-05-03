using System;
using UniRx;

public interface IStopwatchView
{
    IObservable<Unit> OnStartClicked { get; }
    IObservable<Unit> OnStopClicked { get; }
    IObservable<Unit> OnResetClicked { get; }
    IObservable<Unit> OnLapClicked { get; }

    void SetElapsedTime(TimeSpan time);
    void SetRunningState(bool isRunning);
    void AddLap(TimeSpan lapTime);
    void ClearLaps();
}