using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UniRx;
using System;

public class StopwatchPresenterTest
{
    private StopwatchPresenter presenter;
    private TestStopwatchService service;
    private TestStopwatchView view;

    [SetUp]
    public void Setup()
    {
        service = new TestStopwatchService();
        view = new TestStopwatchView();
        presenter = new StopwatchPresenter(service, view);
    }

    [Test]
    public void WhenStartClicked_ServiceStartIsCalled()
    {
        view.StartClickSubject.OnNext(Unit.Default);
        Assert.That(service.StartCalled, Is.True);
    }

    [Test]
    public void WhenStopClicked_ServicePauseIsCalled()
    {
        view.StopClickSubject.OnNext(Unit.Default);
        Assert.That(service.PauseCalled, Is.True);
    }

    [Test]
    public void WhenResetClicked_ServiceResetIsCalled()
    {
        view.ResetClickSubject.OnNext(Unit.Default);
        Assert.That(service.ResetCalled, Is.True);
    }

    [Test]
    public void WhenLapClicked_ServiceLapIsCalled()
    {
        view.LapClickSubject.OnNext(Unit.Default);
        Assert.That(service.LapCalled, Is.True);
    }

    private class TestStopwatchService : IStopwatchService
    {
        public bool StartCalled { get; private set; }
        public bool PauseCalled { get; private set; }
        public bool ResetCalled { get; private set; }
        public bool LapCalled { get; private set; }

        public IReadOnlyReactiveProperty<TimeSpan> ElapsedTime => new ReactiveProperty<TimeSpan>(TimeSpan.Zero);
        public IReadOnlyReactiveProperty<bool> IsRunning => new ReactiveProperty<bool>(false);
        public ReactiveCollection<TimeSpan> Laps { get; } = new ReactiveCollection<TimeSpan>();
        IReadOnlyReactiveCollection<TimeSpan> IStopwatchService.Laps => Laps;


        public void Start() => StartCalled = true;
        public void Pause() => PauseCalled = true;
        public void Reset() => ResetCalled = true;
        public void Lap() => LapCalled = true;
    }

    private class TestStopwatchView : IStopwatchView
    {
        public Subject<Unit> StartClickSubject = new();
        public Subject<Unit> StopClickSubject = new();
        public Subject<Unit> ResetClickSubject = new();
        public Subject<Unit> LapClickSubject = new();

        public IObservable<Unit> OnStartClicked => StartClickSubject;
        public IObservable<Unit> OnStopClicked => StopClickSubject;
        public IObservable<Unit> OnResetClicked => ResetClickSubject;
        public IObservable<Unit> OnLapClicked => LapClickSubject;

        public void SetElapsedTime(TimeSpan time) { }
        public void SetRunningState(bool isRunning) { }
        public void AddLap(TimeSpan lap) { }
        public void ClearLaps() { }
    }
}