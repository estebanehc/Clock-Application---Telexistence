using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class MainViewNavigatorTest
{
    private MainViewNavigator navigator;
    private GameObject clockButton;
    private GameObject timerButton; 
    private GameObject stopwatchButton;
    private GameObject clockPanel;
    private GameObject timerPanel;
    private GameObject stopwatchPanel;

    [SetUp]
    public void SetUp()
    {
        navigator = new GameObject().AddComponent<MainViewNavigator>();

        clockButton = new GameObject("ClockButton");
        timerButton = new GameObject("TimerButton");
        stopwatchButton = new GameObject("StopwatchButton");

        clockButton.AddComponent<Button>();
        timerButton.AddComponent<Button>();
        stopwatchButton.AddComponent<Button>();

        clockPanel = new GameObject("ClockPanel");
        timerPanel = new GameObject("TimerPanel");
        stopwatchPanel = new GameObject("StopwatchPanel");

        TestHelper.InjectSerializedField(navigator, "clockButton", clockButton.GetComponent<Button>());
        TestHelper.InjectSerializedField(navigator, "timerButton", timerButton.GetComponent<Button>());
        TestHelper.InjectSerializedField(navigator, "stopwatchButton", stopwatchButton.GetComponent<Button>());
        TestHelper.InjectSerializedField(navigator, "clockPanel", clockPanel);
        TestHelper.InjectSerializedField(navigator, "timerPanel", timerPanel);
        TestHelper.InjectSerializedField(navigator, "stopwatchPanel", stopwatchPanel);
    }

    [Test]
    public void Start_ShouldShowClockPanel()
    {
        navigator.Start();

        Assert.IsFalse(clockButton.GetComponent<Button>().interactable);
        Assert.IsTrue(timerButton.GetComponent<Button>().interactable);
        Assert.IsTrue(stopwatchButton.GetComponent<Button>().interactable);

        Assert.IsTrue(clockPanel.activeSelf);
        Assert.IsFalse(timerPanel.activeSelf);
        Assert.IsFalse(stopwatchPanel.activeSelf);
    }

    [Test]
    public void TimerButton_Click_ShouldShowTimerPanel()
    {
        navigator.Start();
        timerButton.GetComponent<Button>().onClick.Invoke();

        Assert.IsTrue(clockButton.GetComponent<Button>().interactable);
        Assert.IsFalse(timerButton.GetComponent<Button>().interactable);
        Assert.IsTrue(stopwatchButton.GetComponent<Button>().interactable);

        Assert.IsFalse(clockPanel.activeSelf);
        Assert.IsTrue(timerPanel.activeSelf);
        Assert.IsFalse(stopwatchPanel.activeSelf);
    }

    [Test]
    public void StopwatchButton_Click_ShouldShowStopwatchPanel()
    {
        navigator.Start();
        stopwatchButton.GetComponent<Button>().onClick.Invoke();

        Assert.IsTrue(clockButton.GetComponent<Button>().interactable);
        Assert.IsTrue(timerButton.GetComponent<Button>().interactable);
        Assert.IsFalse(stopwatchButton.GetComponent<Button>().interactable);

        Assert.IsFalse(clockPanel.activeSelf);
        Assert.IsFalse(timerPanel.activeSelf);
        Assert.IsTrue(stopwatchPanel.activeSelf);
    }

    [UnityTest]
    public IEnumerator ClockButton_Click_ShouldShowClockPanel()
    {
        navigator.Start();
        timerButton.GetComponent<Button>().onClick.Invoke();
        yield return null;

        clockButton.GetComponent<Button>().onClick.Invoke();
        yield return null;

        Assert.IsFalse(clockButton.GetComponent<Button>().interactable);
        Assert.IsTrue(timerButton.GetComponent<Button>().interactable);
        Assert.IsTrue(stopwatchButton.GetComponent<Button>().interactable);

        Assert.IsTrue(clockPanel.activeSelf);
        Assert.IsFalse(timerPanel.activeSelf);
        Assert.IsFalse(stopwatchPanel.activeSelf);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(clockButton);
        Object.DestroyImmediate(timerButton);
        Object.DestroyImmediate(stopwatchButton);
        Object.DestroyImmediate(clockPanel);
        Object.DestroyImmediate(timerPanel);
        Object.DestroyImmediate(stopwatchPanel);
        Object.DestroyImmediate(navigator.gameObject);
    }

    private static class TestHelper
    {
        public static void InjectSerializedField<T>(MonoBehaviour target, string fieldName, T value)
        {
            var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(target, value);
            }
        }
    }
}