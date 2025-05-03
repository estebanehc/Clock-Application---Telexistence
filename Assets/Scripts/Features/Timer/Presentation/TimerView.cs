using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

public class TimerView : MonoBehaviour, ITimerView
{
    [SerializeField] private TextMeshProUGUI remainingTimeText;
    [SerializeField] private TMP_Dropdown hoursDropdown;
    [SerializeField] private TMP_Dropdown minutesDropdown;
    [SerializeField] private TMP_Dropdown secondsDropdown;
    [SerializeField] private Button startButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resetButton;

    private AudioSource audioSource;

    public IObservable<int> OnHoursChanged => hoursDropdown.onValueChanged.AsObservable();
    public IObservable<int> OnMinutesChanged => minutesDropdown.onValueChanged.AsObservable();
    public IObservable<int> OnSecondsChanged => secondsDropdown.onValueChanged.AsObservable();

    public int Hours => hoursDropdown.value;
    public int Minutes => minutesDropdown.value;
    public int Seconds => secondsDropdown.value;

    public IObservable<Unit> StartButtonClicked => startButton.OnClickAsObservable();
    public IObservable<Unit> PauseButtonClicked => pauseButton.OnClickAsObservable();
    public IObservable<Unit> ResetButtonClicked => resetButton.OnClickAsObservable();


    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InitializeDropdowns();
    }
    public void InitializeDropdowns()
    {
        hoursDropdown.ClearOptions();
        minutesDropdown.ClearOptions();
        secondsDropdown.ClearOptions();

        for (int i = 0; i <= 23; i++) hoursDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString("D2")));
        for (int i = 0; i <= 59; i++)
        {
            minutesDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString("D2")));
            secondsDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString("D2")));
        }

        hoursDropdown.value = 0;
        minutesDropdown.value = 0;
        secondsDropdown.value = 0;

        hoursDropdown.RefreshShownValue();
        minutesDropdown.RefreshShownValue();
        secondsDropdown.RefreshShownValue();
    }

    public void SetRemainingTime(TimeSpan time)
    {
        remainingTimeText.text = $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
    }

    public void PlayFinishedSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void SetButtonsState(bool start, bool pause, bool reset)
    {
        startButton.interactable = start;
        pauseButton.interactable = pause;
        resetButton.interactable = reset;
    }

    public void SetDropdownsInteractable(bool interactable)
    {
        hoursDropdown.interactable = interactable;
        minutesDropdown.interactable = interactable;
        secondsDropdown.interactable = interactable;
    }

    public void SetPauseButtonLabel(string label)
    {
        pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = label;
    }
}