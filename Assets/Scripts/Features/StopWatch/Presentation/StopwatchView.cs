using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.UI;
using System;

public class StopwatchView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI elapsedTimeText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button lapButton;
    [SerializeField] private Transform lapListContainer;
    [SerializeField] private GameObject lapItemPrefab;

    public IObservable<Unit> OnStartButtonClicked => startButton.OnClickAsObservable();
    public IObservable<Unit> OnPauseButtonClicked => pauseButton.OnClickAsObservable();
    public IObservable<Unit> OnResetButtonClicked => resetButton.OnClickAsObservable();
    public IObservable<Unit> OnLapButtonClicked => lapButton.OnClickAsObservable();

    public void SetElapsedTime(string time)
    {
        elapsedTimeText.text = time;
    }

    public void SetButtonState(bool start, bool pause, bool reset, bool lap)
    {
        startButton.interactable = start;
        pauseButton.interactable = pause;
        resetButton.interactable = reset;
        lapButton.interactable = lap;
    }

    public void SetPauseResumeLabel(string label)
    {
        pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = label;
    }

    public void ClearLapList()
    {
        foreach (Transform child in lapListContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddLapItem(string lapTime)
    {
        var lapItem = Instantiate(lapItemPrefab, lapListContainer);
        lapItem.GetComponentInChildren<TextMeshProUGUI>().text = lapTime;
    }
}
