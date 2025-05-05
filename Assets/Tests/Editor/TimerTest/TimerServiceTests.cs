using NUnit.Framework;
using System;
using System.Threading;

public class TimerServiceTests
{
    private TimerService timerService;
    private readonly IDisposable finishedSubscription;

    [SetUp]
    public void Setup()
    {
        var timerModel = new TimerModel();
        timerService = new TimerService(timerModel);
    }

    [TearDown]
    public void TearDown()
    {
        finishedSubscription?.Dispose();
        timerService = null;
    }

    [Test]
    public void Start_SetsIsRunningTrue()
    {
        timerService.SetDuration(TimeSpan.FromSeconds(5));
        timerService.Start();

        Assert.IsTrue(timerService.IsRunning.Value);
    }

    [Test]
    public void SetDuration_SetsRemainingTimeCorrectly()
    {
        var duration = TimeSpan.FromSeconds(10);
        timerService.SetDuration(duration);

        Assert.AreEqual(duration, timerService.RemainingTime.Value);
    }

    [Test]
    public void Pause_StopsRunningWithoutResetting()
    {
        timerService.SetDuration(TimeSpan.FromSeconds(5));
        timerService.Start();
        Thread.Sleep(100);
        timerService.Pause();

        Assert.IsFalse(timerService.IsRunning.Value);
        Assert.Greater(timerService.RemainingTime.Value, TimeSpan.Zero);
    }

    [Test]
    public void Reset_StopsAndRestoresInitialDuration()
    {
        var init = TimeSpan.FromSeconds(7);
        timerService.SetDuration(init);
        timerService.Start();
        Thread.Sleep(100);
        timerService.Reset();

        Assert.IsFalse(timerService.IsRunning.Value);
        Assert.AreEqual(init, timerService.RemainingTime.Value);
    }

    [Test]
    public void Dispose_StopsTimerAndCleansUpResources()
    {
        var duration = TimeSpan.FromSeconds(5);
        timerService.SetDuration(duration);
        timerService.Start();

        timerService.Dispose();

        Assert.IsTrue(timerService.IsRunning.Value);
    }
}