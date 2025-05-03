using NUnit.Framework;
using System;
using UniRx;

public class ClockPresenterTests
{
    private TestClockView clockView;
    private TestClockService clockService;
    private ClockModel clockModel;
    private ClockPresenter presenter;
    private DateTime fixedTime;

    private class TestClockView : IClockView
    {
        public string LastDisplayedTime { get; private set; }
        public int UpdateCount { get; private set; }

        public void UpdateTimeDisplay(string time)
        {
            LastDisplayedTime = time;
            UpdateCount++;
        }
    }

    private class TestClockService : IClockService
    {
        public DateTime TimeToReturn { get; set; }
        public DateTime CurrentTime => TimeToReturn;
    }

    [SetUp]
    public void Setup()
    {
        clockView = new TestClockView();
        clockService = new TestClockService();
        clockModel = new ClockModel();
        fixedTime = new DateTime(2023, 1, 1, 12, 0, 0);
        
        clockService.TimeToReturn = fixedTime;
        
        presenter = new ClockPresenter(
            clockView,
            clockService,
            clockModel
        );
    }

    [TearDown]
    public void Cleanup()
    {
        presenter.Dispose();
    }

    [Test]
    public void InitialTime_ShouldBeSetFromService()
    {
        Assert.That(clockModel.CurrentTime.Value, Is.EqualTo(fixedTime));
        Assert.That(clockView.LastDisplayedTime, Is.EqualTo(fixedTime.ToString("HH:mm:ss")));
        Assert.That(clockView.UpdateCount, Is.EqualTo(1));
    }

    [Test]
    public void WhenModelTimeChanges_ShouldUpdateView()
    {
        var newTime = new DateTime(2023, 1, 1, 12, 0, 1);
        
        clockModel.CurrentTime.Value = newTime;

        Assert.That(clockView.LastDisplayedTime, Is.EqualTo(newTime.ToString("HH:mm:ss")));
    }

    [Test]
    public void WhenDisposed_ShouldCleanupSubscriptions()
    {
        int initialUpdateCount = clockView.UpdateCount;
        
        presenter.Dispose();
        var newTime = new DateTime(2023, 1, 1, 12, 0, 1);
        clockModel.CurrentTime.Value = newTime;

        Assert.That(clockView.UpdateCount, Is.EqualTo(initialUpdateCount));
    }

    [Test]
    public void AfterOneSecond_ShouldUpdateTimeFromService()
    {
        var newTime = new DateTime(2023, 1, 1, 12, 0, 1);
        clockService.TimeToReturn = newTime;

        Observable.Timer(TimeSpan.FromSeconds(1))
            .Subscribe(_ => {
                Assert.That(clockModel.CurrentTime.Value, Is.EqualTo(newTime));
                Assert.That(clockView.LastDisplayedTime, Is.EqualTo(newTime.ToString("HH:mm:ss")));
            });
    }
}
