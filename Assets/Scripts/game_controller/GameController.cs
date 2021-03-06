﻿using System;
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

    [SerializeField] private GameObject player;

    public List<TextMeshProUGUI> timerTextOuts;
    public bool ignoreTimerOver = false;

    public bool isPresent = false;
    public Tilemap presentLayerTileMap;
    public Tilemap pastLayerTileMap;
    private LayerMask presentLayerMask;
    private LayerMask pastLayerMask;

    private DeathType? deathReason = null;

    [Serializable]
    private enum UIState
    {
        IN_GAME,
        PAUSE,
        GAME_OVER
    }

    [Serializable]
    private class UIActivator
    {
        public UIState activeState;
        public GameObject subUIRoot;
    }

    [SerializeField] private UIState currentState = UIState.IN_GAME;
    [SerializeField] private List<UIActivator> subUIs;

    private ITimer timer;

    private readonly Func<bool> pauseKeyCheck = () => Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape);
    private readonly Func<bool> changeTimeLayerKeyCheck = () => playerLayerSwitchEnabled && Input.GetKeyDown(KeyCode.Q);
    private readonly Func<bool> muteAudioKeyCheck = () => Input.GetKeyDown(KeyCode.M);
    
    public static bool playerLayerSwitchEnabled;
    public bool playerLayerSwitchEnabledInit = true;

    private void Awake()
    {
        Instance = this;
        presentLayerMask = LayerMask.GetMask("Present");
        pastLayerMask = LayerMask.GetMask("Past");
        playerLayerSwitchEnabled = playerLayerSwitchEnabledInit;
    }

    private void Start()
    {
        timer = TimerImpl.Instance;
        timer.addTimeConsumer(time => { timerTextOuts.ForEach(item => item.text = $"{time:0.###}"); });
        var handleLayerChange = isPresent ? (Action) handlePresentLayerChange : handlePastLayerChange;
        handleLayerChange();
        timer.resumeTimer();
        AudioController.instance.PlayAudio(GameAudioType.ST_01);
        handleUIStateChange(currentState);
        player.GetComponent<PlayerHealthController>().onHealthChange.AddListener(it =>
        {
            if (it <= 0)
            {
                handlePlayerNoHealth();
            }
        });
    }

    private void Update()
    {
        if (muteAudioKeyCheck())
        {
            // Toggle Mute State
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
            if (!timer.isPaused()) handleOutOfTime();
        }
    }

    private void handlePauseGame()
    {
        // Handle imminent pause of game
        timer.pauseTimer();
        handleUIStateChange(UIState.PAUSE);
    }

    // UI Accessed
    // ReSharper disable once MemberCanBePrivate.Global
    public void handleResumeGame()
    {
        // Handle imminent pause of game
        handleUIStateChange(UIState.IN_GAME);
        timer.resumeTimer();
    }

    private void handleOutOfTime()
    {
        timer.pauseTimer();
        PersistenceHandler.incrementPlayerDeaths();
        deathReason = DeathType.TIME;
        handleUIStateChange(UIState.GAME_OVER);
    }

    private void handlePlayerNoHealth()
    {
        timer.pauseTimer();
        PersistenceHandler.incrementPlayerDeaths();
        deathReason = DeathType.DAMAGE;
        handleUIStateChange(UIState.GAME_OVER);
    }

    public void handlePlayerOutBounds()
    {
        timer.pauseTimer();
        PersistenceHandler.incrementPlayerDeaths();
        deathReason = DeathType.BOUNDS;
        handleUIStateChange(UIState.GAME_OVER);
    }

    // UI Accessed
    // ReSharper disable once UnusedMember.Global
    public void handleRestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // UI Accessed
    // ReSharper disable once UnusedMember.Global
    public void handleSwitchToMainMenu()
    {
        SceneManager.LoadScene(GameScenes.MAIN_MENU);
    }

    // UI Accessed
    // ReSharper disable once UnusedMember.Global
    public void handleQuitGame()
    {
        Application.Quit();
    }

    private void handleToggleTimeLayer() => changeLayer(!isPresent);

    public void changeLayer(bool toPresent)
    {
        isPresent = toPresent;
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

    private void handleUIStateChange(UIState state)
    {
        void applyOnList(UIState curState)
        {
            subUIs.ForEach(it => { it.subUIRoot.SetActive(it.activeState == curState); });
        }

        switch (state)
        {
            case UIState.IN_GAME:
                applyOnList(UIState.IN_GAME);
                break;
            case UIState.PAUSE:
                applyOnList(UIState.PAUSE);
                break;
            case UIState.GAME_OVER:
                applyOnList(UIState.GAME_OVER);
                if (deathReason != null)
                {
                    DisplayDeathReason.Instance.displayDeathReason(deathReason.Value);
                }

                DisplayDeathMessage.Instance.displayMessage();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        currentState = state;
    }


    public LayerMask getGroundLayer() => isPresent ? presentLayerMask : pastLayerMask;

    public void completedLevel()
    {
        ignoreTimerOver = true;
        var curLevel = GameScenes.LEVELS.IndexOf(SceneManager.GetActiveScene().name);
        if (curLevel > PersistenceHandler.continueGame())
        {
            PersistenceHandler.saveClearedLevel(curLevel);
        }

        MainMenuController.isLevelSelection = true;
        SceneManager.LoadScene(GameScenes.MAIN_MENU);
    }
}

public enum DeathType
{
    TIME,
    BOUNDS,
    DAMAGE
}