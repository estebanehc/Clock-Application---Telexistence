using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.TestTools;
using System.Collections;

public class ClockViewTest
{
    private ClockView clockView;
    private TextMeshProUGUI textMeshPro;

    [UnityTest]
    public IEnumerator UpdateTimeDisplay_SetsTextCorrectly()
    {
        var gameObject = new GameObject();
        clockView = gameObject.AddComponent<ClockView>();
        textMeshPro = gameObject.AddComponent<TextMeshProUGUI>();
        var field = typeof(ClockView).GetField("timeText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(clockView, textMeshPro);

        string expectedTime = "12:00";

        clockView.UpdateTimeDisplay(expectedTime);
        
        yield return null;

        Assert.That(textMeshPro.text, Is.EqualTo(expectedTime));

        Object.DestroyImmediate(gameObject);
    }
}