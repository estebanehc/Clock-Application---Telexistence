using UnityEngine;
using UnityEngine.UI;

public class MainViewNavigator : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button clockButton;
    [SerializeField] private Button timerButton;
    [SerializeField] private Button stopwatchButton;

    [Header("UI Panels")]
    [SerializeField] private GameObject clockPanel;
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private GameObject stopwatchPanel;


    void Start()
    {
        clockButton.onClick.AddListener(ClockUIHandle);
        timerButton.onClick.AddListener(TimerUIHandle);
        stopwatchButton.onClick.AddListener(StopwatchUIHandle);
        ClockUIHandle();
    }

    private void ClockUIHandle()
    {
        HideAllPanels();
        ButtonsInteraction(false, true, true);
        ShowPanel(clockPanel);
    }

    private void TimerUIHandle()
    {
        HideAllPanels();
        ButtonsInteraction(true, false, true);
        ShowPanel(timerPanel);
    }

    private void StopwatchUIHandle()
    {
        HideAllPanels();
        ButtonsInteraction(true, true, false);
        ShowPanel(stopwatchPanel);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        panelToShow.SetActive(true);
    }

    private void ButtonsInteraction(bool clock, bool timer, bool stopwatch)
    {
        clockButton.interactable = clock;
        timerButton.interactable = timer;
        stopwatchButton.interactable = stopwatch;
    }

    private void HideAllPanels()
    {
        clockPanel.SetActive(false);
        timerPanel.SetActive(false);
        stopwatchPanel.SetActive(false);
    }
}
