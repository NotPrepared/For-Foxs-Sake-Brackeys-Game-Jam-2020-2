using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class MainMenuController : MonoBehaviour
{
    public MenuButtonController menuButtonController;
    
    public MenuButton newGameORResetBtn;
    public TMP_Text newGameORResetBtnLabel;
    public MenuButton continueBtn;

    public MenuButton sandboxBtn;
    public MenuButton quitBtn;
    
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
        quitBtn.onClick.AddListener(Application.Quit);
        updateUI();
    }

    private void updateUI() {
        // Check if an active Game exists
        if (PersistenceHandler.hasActiveGame())
        {
            continueBtn.thisIndex = 0;
            newGameORResetBtn.thisIndex = 1;
            sandboxBtn.thisIndex = 2;
            quitBtn.thisIndex = 3;
            
            menuButtonController.index = 0;
            menuButtonController.maxIndex = 3;
            
            newGameORResetBtnLabel.text = "Reset Game";
            continueBtn.gameObject.SetActive(true);
            continueBtn.onClick.AddListener(continueGame);
        }
        else
        {
            continueBtn.thisIndex = -1;
            newGameORResetBtn.thisIndex = 0;
            sandboxBtn.thisIndex = 1;
            quitBtn.thisIndex = 2;
            menuButtonController.index = 0;
            menuButtonController.maxIndex = 2;
            newGameORResetBtnLabel.text = "New Game";
            continueBtn.gameObject.SetActive(false);
            continueBtn.onClick.RemoveAllListeners();
        }
    }
}