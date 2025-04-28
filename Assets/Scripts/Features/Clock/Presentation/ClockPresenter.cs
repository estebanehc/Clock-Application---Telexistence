using UnityEngine;
using UniRx;
using Zenject;

public class ClockPresenter : MonoBehaviour
{
    private ClockView clockView;
    private IClockService clockService;
    private ClockModel clockModel;
    private CompositeDisposable disposables = new();

    [Inject]
    public void Construct(IClockService clockService, ClockModel clockModel)
    {
        this.clockService = clockService;
        this.clockModel = clockModel;
    }

    private void Start()
    {
        clockView = GetComponent<ClockView>();

        Observable.Interval(System.TimeSpan.FromSeconds(1))
            .Subscribe(_ => UpdateTime())
            .AddTo(disposables);

        clockModel.CurrentTime
            .Subscribe(time => clockView.UpdateClockDisplay(time.ToString("HH:mm:ss")))
            .AddTo(disposables);
    }

    private void UpdateTime()
    {
        clockModel.CurrentTime.Value = clockService.CurrentTime;
    }
        
    private void OnDestroy() => disposables.Dispose();

}
