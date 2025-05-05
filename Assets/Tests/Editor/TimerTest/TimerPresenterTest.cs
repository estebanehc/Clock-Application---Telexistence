using NUnit.Framework;
using System;
using UniRx;

[TestFixture]
public class TimerPresenterTest
{
    private TimerPresenter presenter;
    private TestTimerView timerView;
    private TestTimerService timerService;

    [SetUp]
    public void SetUp()
    {
        timerView = new TestTimerView();
        timerService = new TestTimerService();
        presenter = new TimerPresenter();
        presenter.Construct(timerView, timerService);
    }

    [Test] 
    public void StartTimer_WithValidDuration_StartsTimerAndUpdatesUI()
    {
        timerView.Hours = 1;
        timerView.Minutes = 30;
        timerView.Seconds = 0;

        ((Subject<Unit>)timerView.StartButtonClicked).OnNext(Unit.Default);

        Assert.That(timerService.WasStartCalled, Is.True);
        Assert.That(timerView.StartButtonInteractable, Is.False);
        Assert.That(timerView.PauseButtonInteractable, Is.True);
        Assert.That(timerView.ResetButtonInteractable, Is.True);
        Assert.That(timerView.DropdownsInteractable, Is.False);
        Assert.That(timerView.PauseButtonText, Is.EqualTo("Pause"));
    }

    [Test]
    public void PauseTimer_WhenRunning_PausesTimerAndUpdatesUI()
    {
        timerService.isRunning.Value = true;
        ((Subject<Unit>)timerView.PauseButtonClicked).OnNext(Unit.Default);

        Assert.That(timerView.StartButtonInteractable, Is.False);
        Assert.That(timerView.PauseButtonInteractable, Is.False);
    }

    [Test]
    public void PauseTimer_WhenPaused_ResumesTimerAndUpdatesUI()
    {
        timerService.isRunning.Value = false;
        ((Subject<Unit>)timerView.PauseButtonClicked).OnNext(Unit.Default);

        Assert.That(timerView.StartButtonInteractable, Is.False);
        Assert.That(timerView.PauseButtonInteractable, Is.False);
    }

    [Test]
    public void ResetTimer_WhenRunning_ResetsTimerAndUpdatesUI()
    {
        timerService.isRunning.Value = true;
        ((Subject<Unit>)timerView.ResetButtonClicked).OnNext(Unit.Default);

        Assert.That(timerView.StartButtonInteractable, Is.False);
        Assert.That(timerView.PauseButtonInteractable, Is.False);
        Assert.That(timerView.ResetButtonInteractable, Is.False);
    }

    [Test]
    public void TimerFinished_WhenTimerFinishes_UpdatesUI()
    {
        timerService.isRunning.Value = true;
        ((Subject<Unit>)timerService.Finished).OnNext(Unit.Default);

        Assert.That(timerView.StartButtonInteractable, Is.True);
        Assert.That(timerView.PauseButtonInteractable, Is.False);
        Assert.That(timerView.ResetButtonInteractable, Is.True);
        Assert.That(timerView.DropdownsInteractable, Is.True);
        Assert.That(timerView.PauseButtonText, Is.EqualTo("Pause"));
    }

    [Test]
    public void Dispose_DisposesAllSubscriptions()
    {
        var testTime = TimeSpan.FromSeconds(10);
        timerService.remainingTime.Value = testTime;
        
        presenter.Dispose();
        
        ((Subject<Unit>)timerView.StartButtonClicked).OnNext(Unit.Default);
        ((Subject<Unit>)timerView.PauseButtonClicked).OnNext(Unit.Default);
        ((Subject<Unit>)timerView.ResetButtonClicked).OnNext(Unit.Default);
        ((Subject<Unit>)timerService.Finished).OnNext(Unit.Default);
        
        Assert.That(timerService.WasStartCalled, Is.False, "Timer should not start after disposal");
        Assert.That(timerView.PauseButtonText, Is.Not.EqualTo("Resume"), "UI should not update after disposal");
    }

    private class TestTimerView : ITimerView
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        private readonly Subject<int> hoursChanged = new();
        private readonly Subject<int> minutesChanged = new();
        private readonly Subject<int> secondsChanged = new();
        private readonly Subject<Unit> startClicked = new();
        private readonly Subject<Unit> pauseClicked = new();
        private readonly Subject<Unit> resetClicked = new();

        public IObservable<int> OnHoursChanged => hoursChanged;
        public IObservable<int> OnMinutesChanged => minutesChanged;
        public IObservable<int> OnSecondsChanged => secondsChanged;
        public IObservable<Unit> StartButtonClicked => startClicked;
        public IObservable<Unit> PauseButtonClicked => pauseClicked;
        public IObservable<Unit> ResetButtonClicked => resetClicked;
        public bool StartButtonInteractable { get; private set; }
        public bool PauseButtonInteractable { get; private set; }
        public bool ResetButtonInteractable { get; private set; }
        public bool DropdownsInteractable { get; private set; }
        public string PauseButtonText { get; private set; }

        public void InitializeDropdowns() { }
        public void SetRemainingTime(TimeSpan time) { }
        public void SetButtonsState(bool start, bool pause, bool reset)
        {
            StartButtonInteractable = start;
            PauseButtonInteractable = pause;
            ResetButtonInteractable = reset;
        }
        public void SetPauseButtonLabel(string label) => PauseButtonText = label;
        public void SetDropdownsInteractable(bool interactable) => DropdownsInteractable = interactable;
        public void TimerFinished() { }
        public void ShowButton(bool visible) { }
    }

    private class TestTimerService : ITimerService
    {
        public bool WasStartCalled { get; private set; }
        public readonly ReactiveProperty<TimeSpan> remainingTime = new();
        private readonly Subject<Unit> finished = new();
        public readonly ReactiveProperty<bool> isRunning = new();

        public IReadOnlyReactiveProperty<TimeSpan> RemainingTime => remainingTime;
        public IReadOnlyReactiveProperty<bool> IsRunning => isRunning;
        public IObservable<Unit> Finished => finished;

        public void SetDuration(TimeSpan duration) { }
        public void Start() => WasStartCalled = true;
        public void Pause() { }
        public void Reset() { }
    }
}