using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public List<TextMeshProUGUI> timerTextOuts;

    private ITimer timer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timer = TimerImpl.Instance;
        timer.addTimeConsumer(time => { timerTextOuts.ForEach(item => item.text = $"{time:0.###}"); });
        timer.resumeTimer();
    }
}