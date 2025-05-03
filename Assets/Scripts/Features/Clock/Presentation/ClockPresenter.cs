using UniRx;
using System;

public class ClockPresenter : IDisposable
{
    private readonly IClockView clockView;
    private readonly IClockService clockService;
    private readonly ClockModel clockModel;
    private readonly CompositeDisposable disposables = new();

    public ClockPresenter(IClockView clockView, IClockService clockService, ClockModel clockModel)
    {
        this.clockView = clockView;
        this.clockService = clockService;
        this.clockModel = clockModel;
        
        Start();
    }

    private void Start()
    {
        UpdateTime();

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => UpdateTime())
            .AddTo(disposables);

        clockModel.CurrentTime
            .Subscribe(time => clockView.UpdateTimeDisplay(time.ToString("HH:mm:ss")))
            .AddTo(disposables);
    }
    
    private void UpdateTime()
    {
        clockModel.CurrentTime.Value = clockService.CurrentTime;
    }
        
    public void Dispose()
    {
        disposables.Dispose();
    }
}
