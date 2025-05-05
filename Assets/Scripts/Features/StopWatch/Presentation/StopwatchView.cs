using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.UI;
using System;

public class StopwatchView : MonoBehaviour, IStopwatchView
{
    [SerializeField] private TextMeshProUGUI timeLabel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button lapButton;
    [SerializeField] private Transform lapsContainer;
    [SerializeField] private GameObject lapTextPrefab;

    private readonly Subject<Unit> onStartClicked = new();
    private readonly Subject<Unit> onStopClicked = new();
    private readonly Subject<Unit> onResetClicked = new();
    private readonly Subject<Unit> onLapClicked = new();

    public IObservable<Unit> OnStartClicked => onStartClicked;
    public IObservable<Unit> OnStopClicked => onStopClicked;
    public IObservable<Unit> OnResetClicked => onResetClicked;
    public IObservable<Unit> OnLapClicked => onLapClicked;
    private int lapCount = 0;


    private void Start()
    {
        startButton.onClick.AddListener(() => onStartClicked.OnNext(Unit.Default));
        stopButton.onClick.AddListener(() => onStopClicked.OnNext(Unit.Default));
        resetButton.onClick.AddListener(() => onResetClicked.OnNext(Unit.Default));
        lapButton.onClick.AddListener(() => onLapClicked.OnNext(Unit.Default));

        stopButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
        lapButton.interactable = false;

        startButton.onClick.AddListener(() => ActiveButtons(false, true, false));
        stopButton.onClick.AddListener(() => ActiveButtons(true, false, true));
        resetButton.onClick.AddListener(() => ActiveButtons(true, false, false));
    }

    public void SetElapsedTime(TimeSpan time)
    {
        timeLabel.text = FormatTime(time);
    }

    public void SetRunningState(bool isRunning)
    {
        startButton.gameObject.SetActive(!isRunning);
        stopButton.gameObject.SetActive(isRunning);

        lapButton.interactable = isRunning;
        resetButton.interactable = !isRunning && timeLabel.text != "00:00.000";
    }

    public void AddLap(TimeSpan lapTime)
    {
        lapCount++;
        var lapGO = Instantiate(lapTextPrefab, lapsContainer);
        if (lapGO.TryGetComponent<TextMeshProUGUI>(out var label))
        {
            label.text = $"Lap {lapCount}: {FormatTime(lapTime)}";
        }
        lapGO.transform.SetSiblingIndex(0);
    }

    public void ClearLaps()
    {
        foreach (Transform child in lapsContainer)
        {
            Destroy(child.gameObject);
        }
        lapCount = 0;
    }

    private string FormatTime(TimeSpan time)
    {
        return $"{time.Minutes:D2}:{time.Seconds:D2}.{time.Milliseconds:D3}";
    }

    private void ActiveButtons(bool start, bool stop, bool reset)
    {
        startButton.gameObject.SetActive(start);
        stopButton.gameObject.SetActive(stop);
        resetButton.gameObject.SetActive(reset);
    }

    private void OnDestroy()
    {
        onStartClicked.Dispose();
        onStopClicked.Dispose();
        onResetClicked.Dispose();
        onLapClicked.Dispose();
    }
}