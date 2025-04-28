using UnityEngine;
using TMPro;

public class ClockView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    public void UpdateClockDisplay(string time) => timeText.text = time;
}
