using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class TimerPlayModeTests
{
    private GameObject testObject;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        testObject = new GameObject("TimerTestGO");

        var canvasGO = new GameObject("Canvas", typeof(Canvas));
        canvasGO.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        var timeText = new GameObject("TimerText", typeof(TextMeshProUGUI)).GetComponent<TextMeshProUGUI>();
        timeText.transform.SetParent(canvasGO.transform);
        timeText.text = "00:00:00";

        var startBtn = CreateButton("StartButton");
        var pauseBtn = CreateButton("PauseButton");
        var resetBtn = CreateButton("ResetButton");

        var hoursDropdown = CreateDropdown("Hours");
        var minutesDropdown = CreateDropdown("Minutes");
        var secondsDropdown = CreateDropdown("Seconds");

        var view = testObject.AddComponent<TimerView>();
        var presenter = testObject.AddComponent<TimerPresenter>();

        var timerModel = new TimerModel();
        var timerService = new TimerService(timerModel);

        presenter.GetType()
            .GetMethod("Construct")
            .Invoke(presenter, new object[] { timerService });

        var audioSource = testObject.AddComponent<AudioSource>();

        view.GetType().GetField("remainingTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(view, timeText);
        view.GetType().GetField("startButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(view, startBtn);
        view.GetType().GetField("pauseButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(view, pauseBtn);
        view.GetType().GetField("resetButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(view, resetBtn);
        view.GetType().GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(view, hoursDropdown);
        view.GetType().GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(view, minutesDropdown);
        view.GetType().GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(view, secondsDropdown);
        view.GetType().GetField("finishedAudioClip", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(view, Resources.Load<AudioClip>("Audio/Finished"));

        presenter.GetType().GetField("audioSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(presenter, audioSource);

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(testObject);
        Object.Destroy(GameObject.Find("Canvas"));
        yield return null;
    }

    [UnityTest]
    public IEnumerator TimerCountsDown_WhenStartClicked()
    {
        var view = testObject.GetComponent<TimerView>();

        view.GetType().GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 0);
        view.GetType().GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 0);
        view.GetType().GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 3);

        var startButton = view.GetType()
            .GetField("startButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as Button;
        startButton.onClick.Invoke();
        yield return new WaitForSeconds(3.5f);

        var timeText = view.GetType()
            .GetField("remainingTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as TextMeshProUGUI;
        Assert.AreEqual("00:00:00", timeText.text);
    }

    [UnityTest]
    public IEnumerator TimerPauses_WhenPauseClicked()
    {
        var view = testObject.GetComponent<TimerView>();

        view.GetType().GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 0);
        view.GetType().GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 0);
        view.GetType().GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 3);

        var startButton = view.GetType()
            .GetField("startButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as Button;
        startButton.onClick.Invoke();
        yield return new WaitForSeconds(1.5f);

        var pauseButton = view.GetType()
            .GetField("pauseButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as Button;
        pauseButton.onClick.Invoke();
        yield return new WaitForSeconds(2f);

        var timeText = view.GetType()
            .GetField("remainingTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as TextMeshProUGUI;
        Assert.AreEqual("00:00:01", timeText.text);
    }

    [UnityTest]
    public IEnumerator TimerResets_WhenResetClicked()
    {
        var view = testObject.GetComponent<TimerView>();

        view.GetType().GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 0);
        view.GetType().GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 0);
        view.GetType().GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 3);

        var startButton = view.GetType()
            .GetField("startButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as Button;
        startButton.onClick.Invoke();
        yield return new WaitForSeconds(1.5f);

        var resetButton = view.GetType()
            .GetField("resetButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as Button;
        resetButton.onClick.Invoke();
        yield return new WaitForSeconds(1f);

        var timeText = view.GetType()
            .GetField("remainingTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as TextMeshProUGUI;
        Assert.AreEqual("00:00:00", timeText.text);
    }

    [UnityTest]
    public IEnumerator TimerResumes_WhenPauseClickedTwice()
    {
        var view = testObject.GetComponent<TimerView>();

        view.GetType().GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("hoursDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 0);
        view.GetType().GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("minutesDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 0);
        view.GetType().GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view).GetType().GetProperty("value").SetValue(view.GetType().GetField("secondsDropdown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(view), 3);

        var startButton = view.GetType()
            .GetField("startButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as Button;
        startButton.onClick.Invoke();
        yield return new WaitForSeconds(1f);

        var pauseButton = view.GetType()
            .GetField("pauseButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as Button;
        pauseButton.onClick.Invoke();
        yield return new WaitForSeconds(1f);

        pauseButton.onClick.Invoke();
        yield return new WaitForSeconds(1f);

        var timeText = view.GetType()
            .GetField("remainingTimeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(view) as TextMeshProUGUI;
        Assert.AreEqual("00:00:01", timeText.text);
    }

    private Button CreateButton(string name)
    {
        var btnGO = new GameObject(name, typeof(Button), typeof(Image));
        var textGO = new GameObject("Text", typeof(TextMeshProUGUI));
        textGO.transform.SetParent(btnGO.transform);
        textGO.GetComponent<TextMeshProUGUI>().text = name;
        return btnGO.GetComponent<Button>();
    }

    private TMP_Dropdown CreateDropdown(string name)
    {
        var dropdownGO = new GameObject(name, typeof(TMP_Dropdown), typeof(Image));
        var dropdown = dropdownGO.GetComponent<TMP_Dropdown>();
        dropdown.options.Add(new TMP_Dropdown.OptionData("00"));
        dropdown.options.Add(new TMP_Dropdown.OptionData("01"));
        dropdown.value = 0;
        return dropdown;
    }
}
