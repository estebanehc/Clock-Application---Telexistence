using System;
using UnityEngine;
using UniRx;
using Zenject;

public class TimerPresenter : MonoBehaviour
{
    private TimerView timerView;
    private ITimerService timerService;
    private readonly CompositeDisposable disposables = new();
    private bool isPaused = false;

    [SerializeField] private AudioSource audioSource;

    [Inject]
    public void Construct(ITimerService timerService)
    {
        this.timerService = timerService;
    }

    private void Start()
    {   
        timerView = GetComponent<TimerView>();
        timerView.InitializeDropdowns();
        HandleButtonInteractivity();

        Observable
            .CombineLatest(
                timerView.OnHoursChanged.StartWith(timerView.Hours),
                timerView.OnMinutesChanged.StartWith(timerView.Minutes),
                timerView.OnSecondsChanged.StartWith(timerView.Seconds),
                (h, m, s) => h + m + s
            )
            .Subscribe(_ => HandleButtonInteractivity())
            .AddTo(disposables);

        timerView.StartButtonClicked
            .Subscribe(_ => StartTimer())
            .AddTo(disposables);

        timerView.PauseButtonClicked
            .Subscribe(_ => PauseTimer())
            .AddTo(disposables);

        timerView.ResetButtonClicked
            .Subscribe(_ => ResetTimer())
            .AddTo(disposables);

        timerService.RemainingTime
            .Subscribe(time => timerView.SetRemainingTime(time))
            .AddTo(disposables);

        timerService.Finished
            .Subscribe(_ => FinishedTimer())
            .AddTo(disposables);
        
    }

    private void StartTimer()
    {
        var duration = TimeSpan.FromHours(timerView.Hours)
            + TimeSpan.FromMinutes(timerView.Minutes)
            + TimeSpan.FromSeconds(timerView.Seconds);
        if (duration <= TimeSpan.Zero) return;
        
        timerService.SetDuration(duration);
        timerService.Start();
        timerView.SetButtonsState(false, true, true);
        timerView.SetPauseButtonLabel("Pause");
        timerView.SetDropdownsInteractable(false);
        isPaused = false;
    }

    private void PauseTimer()
    {
        if (isPaused)
        {
            timerService.Start();
            timerView.SetPauseButtonLabel("Pause");
        }
        else
        {
            timerService.Pause();
            timerView.SetPauseButtonLabel("Resume");
        }
        isPaused = !isPaused;
    }

    private void ResetTimer()
    {
        timerService.Reset();
        isPaused = false;
        timerView.SetPauseButtonLabel("Pause");
        timerView.SetDropdownsInteractable(true);
        timerView.SetButtonsState(true, false, false);
        timerView.SetRemainingTime(TimeSpan.Zero);
        HandleButtonInteractivity();
    }

    private void FinishedTimer()
    {
        timerView.SetButtonsState(false, false, true);
        timerView.SetPauseButtonLabel("Pause");
        timerView.SetDropdownsInteractable(true);
        if (audioSource != null && timerView.FinishedAudioClip != null)
        {
            audioSource.PlayOneShot(timerView.FinishedAudioClip);
        }
    }

    private void HandleButtonInteractivity()
    {
        bool hasValidTime = 
            timerView.Hours > 0 || 
            timerView.Minutes > 0 || 
            timerView.Seconds > 0;
        timerView.SetButtonsState(hasValidTime, false, false);
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }
}
