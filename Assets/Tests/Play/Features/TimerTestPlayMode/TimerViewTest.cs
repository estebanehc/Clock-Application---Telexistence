using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System;
using UnityEngine.TestTools;
using System.Collections;

[TestFixture]
public class TimerViewTest
{
    private GameObject gameObject;
    private TimerView timerView;
    private TextMeshProUGUI remainingTimeText;
    private TMP_Dropdown hoursDropdown;
    private TMP_Dropdown minutesDropdown; 
    private TMP_Dropdown secondsDropdown;
    private Button startButton;
    private Button pauseButton;
    private Button resetButton;
    private AudioSource audioSource;

    [SetUp]
    public void SetUp()
    {
        gameObject = new GameObject("TimerView");
        timerView = gameObject.AddComponent<TimerView>();
        
        var textObj = new GameObject("RemainingText");
        textObj.transform.SetParent(gameObject.transform);
        remainingTimeText = textObj.AddComponent<TextMeshProUGUI>();

        var hoursObj = new GameObject("HoursDropdown");
        hoursObj.transform.SetParent(gameObject.transform);
        hoursDropdown = hoursObj.AddComponent<TMP_Dropdown>();

        var minutesObj = new GameObject("MinutesDropdown");
        minutesObj.transform.SetParent(gameObject.transform);
        minutesDropdown = minutesObj.AddComponent<TMP_Dropdown>();

        var secondsObj = new GameObject("SecondsDropdown");
        secondsObj.transform.SetParent(gameObject.transform);
        secondsDropdown = secondsObj.AddComponent<TMP_Dropdown>();

        var startObj = new GameObject("StartButton");
        startObj.transform.SetParent(gameObject.transform);
        startButton = startObj.AddComponent<Button>();

        var pauseObj = new GameObject("PauseButton");
        pauseObj.transform.SetParent(gameObject.transform);
        pauseButton = pauseObj.AddComponent<Button>();

        var resetObj = new GameObject("ResetButton");
        resetObj.transform.SetParent(gameObject.transform);
        resetButton = resetObj.AddComponent<Button>();

        audioSource = gameObject.AddComponent<AudioSource>();

        var pauseTextObj = new GameObject("PauseText");
        pauseTextObj.transform.SetParent(pauseObj.transform);
        pauseTextObj.AddComponent<TextMeshProUGUI>();

        typeof(TimerView).GetField("remainingTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(timerView, remainingTimeText);
        typeof(TimerView).GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(timerView, hoursDropdown);
        typeof(TimerView).GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(timerView, minutesDropdown);
        typeof(TimerView).GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(timerView, secondsDropdown);
        typeof(TimerView).GetField("startButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(timerView, startButton);
        typeof(TimerView).GetField("pauseButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(timerView, pauseButton);
        typeof(TimerView).GetField("resetButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(timerView, resetButton);
        typeof(TimerView).GetField("audioSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(timerView, audioSource);
    }

    [UnityTest]
    public IEnumerator Start_InitializesTimerViewCorrectly()
    {
        yield return null;

        Assert.That(timerView.GetComponent<AudioSource>(), Is.Not.Null);

        bool hoursChangedTriggered = false;
        bool minutesChangedTriggered = false; 
        bool secondsChangedTriggered = false;
        bool startClickedTriggered = false;
        bool pauseClickedTriggered = false;
        bool resetClickedTriggered = false;

        timerView.OnHoursChanged.Subscribe(_ => hoursChangedTriggered = true);
        timerView.OnMinutesChanged.Subscribe(_ => minutesChangedTriggered = true);
        timerView.OnSecondsChanged.Subscribe(_ => secondsChangedTriggered = true);
        timerView.StartButtonClicked.Subscribe(_ => startClickedTriggered = true);
        timerView.PauseButtonClicked.Subscribe(_ => pauseClickedTriggered = true);
        timerView.ResetButtonClicked.Subscribe(_ => resetClickedTriggered = true);

        hoursDropdown.value = 1;
        minutesDropdown.value = 1; 
        secondsDropdown.value = 1;
        startButton.onClick.Invoke();
        pauseButton.onClick.Invoke();
        resetButton.onClick.Invoke();

        yield return null;

        Assert.That(hoursChangedTriggered, Is.True);
        Assert.That(minutesChangedTriggered, Is.True);
        Assert.That(secondsChangedTriggered, Is.True);
        Assert.That(startClickedTriggered, Is.True);
        Assert.That(pauseClickedTriggered, Is.True);
        Assert.That(resetClickedTriggered, Is.True);
    }

    [Test]
    public void InitializeDropdowns_SetsCorrectOptions()
    {
        timerView.InitializeDropdowns();

        Assert.That(hoursDropdown.options.Count, Is.EqualTo(24));
        Assert.That(minutesDropdown.options.Count, Is.EqualTo(60));
        Assert.That(secondsDropdown.options.Count, Is.EqualTo(60));
        Assert.That(hoursDropdown.value, Is.EqualTo(0));
        Assert.That(minutesDropdown.value, Is.EqualTo(0));
        Assert.That(secondsDropdown.value, Is.EqualTo(0));
    }

    [Test]
    public void SetRemainingTime_UpdatesText()
    {
        var timeSpan = new TimeSpan(1, 30, 45);
        timerView.SetRemainingTime(timeSpan);
        Assert.That(remainingTimeText.text, Is.EqualTo("01:30:45"));
    }

    [Test]
    public void SetButtonsState_UpdatesButtonStates()
    {
        timerView.SetButtonsState(true, false, true);
        Assert.That(startButton.interactable, Is.True);
        Assert.That(pauseButton.interactable, Is.False);
        Assert.That(resetButton.interactable, Is.True);
    }

    [Test]
    public void SetDropdownsInteractable_UpdatesDropdownStates()
    {
        timerView.SetDropdownsInteractable(false);
        Assert.That(hoursDropdown.interactable, Is.False);
        Assert.That(minutesDropdown.interactable, Is.False);
        Assert.That(secondsDropdown.interactable, Is.False);
    }

    [Test]
    public void SetPauseButtonLabel_UpdatesButtonText()
    {
        const string label = "Resume";
        timerView.SetPauseButtonLabel(label);
        Assert.That(pauseButton.GetComponentInChildren<TextMeshProUGUI>().text, Is.EqualTo(label));
    }

    [Test]
    public void Hours_Minutes_Seconds_ReturnsDropdownValues()
    {
        timerView.InitializeDropdowns();
        
        hoursDropdown.value = 12;
        minutesDropdown.value = 30;
        secondsDropdown.value = 45;

        Assert.That(timerView.Hours, Is.EqualTo(12));
        Assert.That(timerView.Minutes, Is.EqualTo(30)); 
        Assert.That(timerView.Seconds, Is.EqualTo(45));
    }

    [Test]
    public void PlayFinishedSound_PlaysAudioClip()
    {
        AudioClip testClip = AudioClip.Create("test", 1, 1, 44100, false);
        audioSource.clip = testClip;

        timerView.PlayFinishedSound();

        Assert.That(audioSource.clip, Is.EqualTo(testClip));
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(gameObject);
    }
}