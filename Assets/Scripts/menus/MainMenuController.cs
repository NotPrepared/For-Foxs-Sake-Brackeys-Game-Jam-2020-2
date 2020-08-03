using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class MainMenuController : MonoBehaviour
{
    public Button newGameORResetBtn;
    public TMP_Text newGameORResetBtnLabel;
    public Button continueBtn;

    public Button sandboxBtn;
    
    private static void openLevel(string sceneName) => SceneManager.LoadScene(sceneName);
    
    /// <summary>
    /// Resets progress and starts a new game
    /// </summary>
    private static void newGame() {
        PersistenceHandler.resetGameProgress();
        PersistenceHandler.startGame();
        // If it does not launch immediately invoke updateUI() 
    }

    /// <summary>
    /// Continues a active game at last scene
    /// </summary>
    private static void continueGame()
    {
        PersistenceHandler.continueGame();
    }

    /// <summary>
    /// Prepares and launches Sandbox Scene
    /// </summary>
    private static void openSandbox()
    {
        openLevel(GameScenes.SANDBOX);
    }

    private void Start()
    {
        newGameORResetBtn.onClick.AddListener(newGame);
        sandboxBtn.onClick.AddListener(openSandbox);
        updateUI();
    }

    private void updateUI() {
        // Check if an active Game exists
        if (PersistenceHandler.hasActiveGame())
        {
            newGameORResetBtnLabel.text = "Reset Game";
            continueBtn.gameObject.SetActive(true);
            continueBtn.onClick.AddListener(continueGame);
        }
        else
        {
            newGameORResetBtnLabel.text = "New Game";
            continueBtn.gameObject.SetActive(false);
            continueBtn.onClick.RemoveAllListeners();
        }
    }
}