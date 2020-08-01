using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class TimerImpl : MonoBehaviour, ITimer
{
    public static ITimer Instance;

    public float gameTimeInitial = 20f;

    private readonly List<Action<float>> timeConsumers = new List<Action<float>>();

    private bool paused;
    private float time;

    private void Awake()
    {
        Instance = this;
        time = gameTimeInitial;
        pauseTimer();
    }

    private void Update()
    {
        if (paused) return;

        time -= Time.deltaTime;
        foreach (var timeConsumer in timeConsumers)
        {
            timeConsumer.Invoke(getRemainingTime());
        }
    }

    public bool isTimeOver() => time <= 0f;

    public void addTime(float timeIncrement)
    {
        time += timeIncrement;
    }

    public void pauseTimer()
    {
        paused = true;
        Time.timeScale = 0f;
    }

    public void resumeTimer()
    {
        paused = false;
        Time.timeScale = 1f;
    }

    public bool isPaused() => paused;

    public float getRemainingTime() => isTimeOver() ? 0f : time;

    public void addTimeConsumer(Action<float> timeConsumer) => timeConsumers.Add(timeConsumer);
}