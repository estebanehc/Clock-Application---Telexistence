using System;
using UniRx;
using UnityEngine;

public interface ITimerView
{
    IObservable<int> OnHoursChanged { get; }
    IObservable<int> OnMinutesChanged { get; }
    IObservable<int> OnSecondsChanged { get; }

    IObservable<Unit> StartButtonClicked { get; }
    IObservable<Unit> PauseButtonClicked { get; }
    IObservable<Unit> ResetButtonClicked { get; }

    int Hours { get; }
    int Minutes { get; }
    int Seconds { get; }

    void InitializeDropdowns();
    void SetRemainingTime(TimeSpan time);
    void SetButtonsState(bool start, bool pause, bool reset);
    void SetDropdownsInteractable(bool interactable);
    void SetPauseButtonLabel(string label);
    void PlayFinishedSound();
}
