using System;
using UniRx;
using Zenject;

public class TimerPresenter : IDisposable
{
    private ITimerView timerView;
    private ITimerService timerService;
    private readonly CompositeDisposable disposables = new();
    private bool isPaused = false;

    [Inject]
    public void Construct(ITimerView timerView, ITimerService timerService)
    {
        this.timerView = timerView;
        this.timerService = timerService;

        BindService();
    }

    private void BindService()
    {   
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
        timerView.ShowButton(false);
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
        timerView.ShowButton(true);
        HandleButtonInteractivity();
    }

    private void FinishedTimer()
    {
        timerView.SetButtonsState(true, false, true);
        timerView.SetPauseButtonLabel("Pause");
        timerView.SetDropdownsInteractable(true);
        timerView.TimerFinished();
    }

    private void HandleButtonInteractivity()
    {
        bool hasValidTime = 
            timerView.Hours > 0 || 
            timerView.Minutes > 0 || 
            timerView.Seconds > 0;
        timerView.SetButtonsState(hasValidTime, false, false);
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}