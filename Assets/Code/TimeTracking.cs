using System.Collections;
using TMPro;
using UnityEngine;

public class TimeTracking : MonoBehaviour
{
    private float TimerLength;
    public float Timer { get; private set; }

    [SerializeField] TextMeshProUGUI timerTextBox;

    private void Awake()
    {
        TimerLength = 60f;
        ResetTimer();
    }

    private void UpdateGUI()
    {
        timerTextBox.text = Timer.ToString("F0");
    }

    public void SetTimerLength(float t)
    {
        TimerLength = t;
        ResetTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (Timer > 0)
        {
            Timer -= Time.deltaTime;
            Debug.Log(Timer);
            UpdateGUI();
            yield return null;
        }

        Timer = 0;
        UpdateGUI();
    }

    public void ResetTimer()
    {
        Timer = TimerLength;
        UpdateGUI();
    }
}
