using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public float duration = 1f;
    public bool loop = false;
    public bool autoStart = true;

    public event Action OnTimerComplete;

    private float currentTime;
    private bool isRunning = false;

    void Start()
    {
        if (autoStart)
            StartTimer();
    }

    public void StartTimer()
    {
        currentTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        if (autoStart)
            isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime += Time.deltaTime;

        if (currentTime >= duration)
        {
            OnTimerComplete?.Invoke();

            if (loop)
            {
                currentTime = 0f;
            }
            else
            {
                isRunning = false;
            }
        }
    }

    public float GetProgress() => Mathf.Clamp01(currentTime / duration);
    public float GetRemainingTime() => Mathf.Max(0, duration - currentTime);
}