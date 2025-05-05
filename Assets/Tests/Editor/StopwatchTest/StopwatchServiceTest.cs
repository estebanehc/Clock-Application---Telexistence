using NUnit.Framework;
using System;

public class StopwatchServiceTest
{
    private StopwatchService stopwatchService;
    private StopwatchModel stopwatchModel;

    [SetUp]
    public void Setup()
    {
        stopwatchModel = new StopwatchModel();
        stopwatchService = new StopwatchService(stopwatchModel);
    }

    [Test]
    public void Start_WhenNotRunning_StartsTimer()
    {
        stopwatchService.Start();
        
        Assert.That(stopwatchService.IsRunning.Value, Is.True);
    }

    [Test] 
    public void Start_WhenAlreadyRunning_DoesNothing()
    {
        stopwatchService.Start();
        var initialTime = stopwatchService.ElapsedTime.Value;
        
        stopwatchService.Start();

        Assert.That(stopwatchService.IsRunning.Value, Is.True);
        Assert.That(stopwatchService.ElapsedTime.Value, Is.EqualTo(initialTime));
    }

    [Test]
    public void Pause_WhenRunning_StopsTimer()
    {
        stopwatchService.Start();
        System.Threading.Thread.Sleep(100);
        var timeBeforePause = stopwatchService.ElapsedTime.Value;
        
        stopwatchService.Pause();

        Assert.That(stopwatchService.IsRunning.Value, Is.False);
        Assert.That(stopwatchService.ElapsedTime.Value, Is.EqualTo(timeBeforePause));
    }

    [Test]
    public void Reset_ClearsEverything()
    {
        stopwatchService.Start();
        stopwatchService.Lap();
        
        stopwatchService.Reset();

        Assert.That(stopwatchService.IsRunning.Value, Is.False);
        Assert.That(stopwatchService.ElapsedTime.Value, Is.EqualTo(TimeSpan.Zero));
        Assert.That(stopwatchService.Laps.Count, Is.EqualTo(0));
    }

    [Test]
    public void Lap_WhenRunning_AddsCurrentTimeToLaps()
    {
        stopwatchService.Start();
        System.Threading.Thread.Sleep(100);
        
        stopwatchService.Lap();

        Assert.That(stopwatchService.Laps.Count, Is.EqualTo(1));
    }

    [Test]
    public void Lap_WhenNotRunning_DoesNothing()
    {
        stopwatchService.Lap();
        
        Assert.That(stopwatchService.Laps.Count, Is.EqualTo(0));
    }

    [TearDown]
    public void Cleanup()
    {
        stopwatchService.Dispose();
    }
}