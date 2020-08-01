using System;
using System.Collections.Generic;
using debug;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public List<TextMeshProUGUI> timerTextOuts;
    public bool ignoreTimerOver = false;

    private ITimer timer;

    private readonly Func<bool> pauseKeyCheck = () => Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape);

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

    private void Update()
    {
        if (!timer.isTimeOver())
        {
            if (!timer.isPaused())
            {
                // Game is running
                if (pauseKeyCheck()) handlePauseGame();
            }
            else
            {
                // Game is paused
                if (pauseKeyCheck()) handleResumeGame();
            }
        }
        else
        {
            // Time is over
            if (!ignoreTimerOver) handleOutOfTime();
        }
    }

    private void handlePauseGame()
    {
        // Handle imminent pause of game
        timer.pauseTimer();
        TODO.asLogWarning("Display Pause UI");
    }

    private void handleResumeGame()
    {
        // Handle imminent pause of game
        TODO.asLogWarning("Hide Pause UI");
        timer.resumeTimer();
    }

    private void handleOutOfTime()
    {
        TODO.asLogWarning("Game End not implemented");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}