using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.UI;
using System;

public class StopwatchViewTest
{
    private StopwatchView stopwatchView;
    private TextMeshProUGUI timeLabel;
    private Button startButton;
    private Button stopButton; 
    private Button resetButton;
    private Button lapButton;
    private Transform lapsContainer;
    private GameObject lapTextPrefab;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        var testGO = new GameObject();
        stopwatchView = testGO.AddComponent<StopwatchView>();
        timeLabel = new GameObject().AddComponent<TextMeshProUGUI>();
        startButton = new GameObject().AddComponent<Button>();
        stopButton = new GameObject().AddComponent<Button>();
        resetButton = new GameObject().AddComponent<Button>();
        lapButton = new GameObject().AddComponent<Button>();
        lapsContainer = new GameObject().transform;
        lapTextPrefab = new GameObject();
        lapTextPrefab.AddComponent<TextMeshProUGUI>();

        var timeLabelField = typeof(StopwatchView).GetField("timeLabel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var startButtonField = typeof(StopwatchView).GetField("startButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var stopButtonField = typeof(StopwatchView).GetField("stopButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var resetButtonField = typeof(StopwatchView).GetField("resetButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var lapButtonField = typeof(StopwatchView).GetField("lapButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var lapsContainerField = typeof(StopwatchView).GetField("lapsContainer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var lapTextPrefabField = typeof(StopwatchView).GetField("lapTextPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        timeLabelField.SetValue(stopwatchView, timeLabel);
        startButtonField.SetValue(stopwatchView, startButton);
        stopButtonField.SetValue(stopwatchView, stopButton);
        resetButtonField.SetValue(stopwatchView, resetButton);
        lapButtonField.SetValue(stopwatchView, lapButton);
        lapsContainerField.SetValue(stopwatchView, lapsContainer);
        lapTextPrefabField.SetValue(stopwatchView, lapTextPrefab);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SetElapsedTime_SetsTimeLabelCorrectly()
    {
        var time = new TimeSpan(0, 1, 30, 0, 500);
        
        stopwatchView.SetElapsedTime(time);
        yield return null;

        Assert.That(timeLabel.text, Is.EqualTo("30:00.500"));
    }

    [UnityTest]
    public IEnumerator SetRunningState_True_ShowsCorrectButtons()
    {
        stopwatchView.SetRunningState(true);
        yield return null;

        Assert.That(startButton.gameObject.activeSelf, Is.False);
        Assert.That(stopButton.gameObject.activeSelf, Is.True);
        Assert.That(lapButton.interactable, Is.True);
    }

    [UnityTest] 
    public IEnumerator AddLap_CreatesLapEntry()
    {
        var lapTime = new TimeSpan(0, 0, 30, 0, 0);

        stopwatchView.AddLap(lapTime);
        yield return null;

        Assert.That(lapsContainer.childCount, Is.EqualTo(1));
        var lapText = lapsContainer.GetChild(0).GetComponent<TextMeshProUGUI>();
        Assert.That(lapText.text, Is.EqualTo("Lap 1: 30:00.000"));
    }

    [UnityTest]
    public IEnumerator Start_AddLap_Reset_ClearsLaps()
    {
        stopwatchView.SetRunningState(true);
        yield return null;

        startButton.onClick.Invoke();
        yield return null;

        lapButton.onClick.Invoke();
        yield return null;

        stopButton.onClick.Invoke();
        yield return null;

        resetButton.onClick.Invoke();
        stopwatchView.ClearLaps();
        yield return null;

        Assert.That(lapsContainer.childCount, Is.EqualTo(0));
        Assert.That(startButton.gameObject.activeSelf, Is.True);
        Assert.That(stopButton.gameObject.activeSelf, Is.False);
    }

    [TearDown]
    public void Cleanup()
    {
        UnityEngine.Object.DestroyImmediate(stopwatchView.gameObject);
        UnityEngine.Object.DestroyImmediate(timeLabel.gameObject);
        UnityEngine.Object.DestroyImmediate(startButton.gameObject);
        UnityEngine.Object.DestroyImmediate(stopButton.gameObject);
        UnityEngine.Object.DestroyImmediate(resetButton.gameObject);
        UnityEngine.Object.DestroyImmediate(lapButton.gameObject);
        UnityEngine.Object.DestroyImmediate(lapsContainer.gameObject);
        UnityEngine.Object.DestroyImmediate(lapTextPrefab);
    }
}