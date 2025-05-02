using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class StopwatchPresenter : MonoBehaviour
{
    private StopwatchView stopwatchView;
    private IStopwatchService stopwatchService;
    private CompositeDisposable disposables = new();

    private int lapCount = 0;
    private bool isPaused = false;

    [Inject]
    public void Construct(IStopwatchService stopwatchService)
    {
        this.stopwatchService = stopwatchService;
    }

    void Start()
    {
        stopwatchView = GetComponent<StopwatchView>();
        stopwatchView.SetElapsedTime("00:00:00");
        stopwatchView.SetButtonState(true, false, false, false);
        stopwatchView.ClearLapList();
        stopwatchView.SetPauseResumeLabel("Pause");

        stopwatchView.OnStartButtonClicked
            .Subscribe(_ => StartStopwatch())
            .AddTo(disposables);

        stopwatchView.OnPauseButtonClicked
            .Subscribe(_ =>
            {
                if (isPaused)
                {
                    ResumeStopwatch();
                }
                else
                {
                    PauseStopwatch();
                }
            })
            .AddTo(disposables);

        stopwatchView.OnResetButtonClicked
            .Subscribe(_ => ResetStopwatch())
            .AddTo(disposables);

        stopwatchView.OnLapButtonClicked
            .Subscribe(_ =>
            {
                if (stopwatchService.IsRunning.Value)
                {
                    stopwatchService.Lap();
                }
            })
            .AddTo(disposables);

        stopwatchService.ElapsedTime
            .Subscribe(time =>
            {
                stopwatchView.SetElapsedTime(time.ToString(@"hh\:mm\:ss"));
            })
            .AddTo(disposables);

        stopwatchService.Laps
            .ObserveAdd()
            .Subscribe(lap =>
            {
                LapStopwatch();
            })
            .AddTo(disposables);

    }

    private void StartStopwatch()
    {
        stopwatchService.Start();
        lapCount = 0;
        stopwatchView.SetButtonState(false, true, true, true);
        stopwatchView.SetPauseResumeLabel("Pause");
        isPaused = false;
    }

    private void PauseStopwatch()
    {
        stopwatchService.Pause();
        stopwatchView.SetButtonState(true, false, true, true);
        stopwatchView.SetPauseResumeLabel("Resume");
        isPaused = true;
    }

    private void ResumeStopwatch()
    {
        stopwatchService.Resume();
        stopwatchView.SetButtonState(false, true, true, true);
        stopwatchView.SetPauseResumeLabel("Pause");
        isPaused = false;
    }

    private void ResetStopwatch()
    {
        stopwatchService.Reset();
        stopwatchView.SetElapsedTime("00:00:00");
        stopwatchView.ClearLapList();
        lapCount = 0;
        stopwatchView.SetButtonState(true, false, false, false);
        stopwatchView.SetPauseResumeLabel("Pause");
        isPaused = false;
    }

    private void LapStopwatch()
    {
        lapCount++;
        stopwatchView.AddLapItem(
            $"Lap {lapCount}: {stopwatchService.ElapsedTime.Value.ToString(@"hh\:mm\:ss")}"
            );
    }
    
    private void OnDestroy()
    {
        disposables.Dispose();
    }
}