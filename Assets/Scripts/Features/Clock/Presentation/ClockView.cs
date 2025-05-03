using UnityEngine;
using TMPro;

public class ClockView : MonoBehaviour, IClockView
{
    [SerializeField] private TextMeshProUGUI timeText;

    public void UpdateTimeDisplay(string time) => timeText.text = time;
}
