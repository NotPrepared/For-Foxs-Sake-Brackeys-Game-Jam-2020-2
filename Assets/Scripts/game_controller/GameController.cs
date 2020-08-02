using System;
using System.Collections.Generic;
using debug;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

// ReSharper disable once CheckNamespace
public class GameController : MonoBehaviour, GroundProvider
{
    public static GameController Instance;

    public List<TextMeshProUGUI> timerTextOuts;
    public bool ignoreTimerOver = false;

    public bool isPresent = false;
    public Tilemap presentLayerTileMap;
    public Tilemap pastLayerTileMap;
    private LayerMask presentLayerMask;
    private LayerMask pastLayerMask;
    
    private ITimer timer;

    private readonly Func<bool> pauseKeyCheck = () => Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape);
    private readonly Func<bool> changeTimeLayerKeyCheck = () => Input.GetKeyDown(KeyCode.Q);
    private readonly Func<bool> muteAudioKeyCheck = () => Input.GetKeyDown(KeyCode.M);

    private void Awake()
    {
        Instance = this;
        presentLayerMask = LayerMask.GetMask("Present");
        pastLayerMask = LayerMask.GetMask("Past");
    }

    private void Start()
    {
        timer = TimerImpl.Instance;
        timer.addTimeConsumer(time => { timerTextOuts.ForEach(item => item.text = $"{time:0.###}"); });
        var handleLayerChange = isPresent ? (Action) handlePresentLayerChange : handlePastLayerChange;
        handleLayerChange();
        timer.resumeTimer();
        AudioController.instance.PlayAudio(GameAudioType.ST_01);
    }

    private void Update()
    {
        if (muteAudioKeyCheck())
        { // Toggle Mute State
            AudioController.instance.IsMuted = !AudioController.instance.IsMuted;
        }

        if (!timer.isTimeOver() || ignoreTimerOver)
        {
            if (!timer.isPaused())
            {
                // Game is running
                if (changeTimeLayerKeyCheck()) handleToggleTimeLayer();
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
            handleOutOfTime();
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

    private void handleToggleTimeLayer()
    {
        isPresent = !isPresent;
        var handleLayer = isPresent ? (Action) handlePresentLayerChange : handlePastLayerChange;
        handleLayer();
    }

    private void handlePastLayerChange()
    {
        pastLayerTileMap.gameObject.SetActive(true);
        presentLayerTileMap.gameObject.SetActive(false);
    }

    private void handlePresentLayerChange()
    {
        presentLayerTileMap.gameObject.SetActive(true);
        pastLayerTileMap.gameObject.SetActive(false);
    }



    public LayerMask getGroundLayer() => isPresent ? presentLayerMask : pastLayerMask;
}