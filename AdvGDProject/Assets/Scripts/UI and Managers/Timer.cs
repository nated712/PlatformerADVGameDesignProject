using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to TMP UI Text
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private bool isTimerStopped = false; // Flag to check if the timer has been stopped

    void Start()
    {
        isRunning = true; // Start the timer automatically
    }

    void Update()
    {
        if (isRunning && !isTimerStopped)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000); // Get milliseconds

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void StopTimer()
    {
        isRunning = false;
        isTimerStopped = true; // Mark the timer as stopped so it can't be started again
    }

    public void StartTimer()
    {
        if (!isTimerStopped) // Check if the timer has been stopped previously
        {
            isRunning = true;
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerDisplay();
        isTimerStopped = false; // Allow the timer to be started again after reset
        isRunning = true; // Start the timer again after reset
    }
}
