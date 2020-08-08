using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class MainMenuController : MonoBehaviour
{
    public MenuButtonController mainMenuBtnController;
    public MenuButtonController levelSelectionBtnController;
    
    public MenuButton newGameORResetBtn;
    public TMP_Text newGameORResetBtnLabel;
    public MenuButton continueBtn;

    public MenuButton quitBtn;

    public static bool isLevelSelection;

    [SerializeField] private GameObject main_menu;
    [SerializeField] private GameObject level_selection;
    [SerializeField] private List<LevelButtonPair> levelSelectButtons;

    [Serializable]
    public class LevelButtonPair
    {
        public MenuButton btn;
        public int level;
    }


    private void Awake()
    {
        if (levelSelectButtons == null) levelSelectButtons = new List<LevelButtonPair>();
    }

    private static void openLevel(string sceneName) => SceneManager.LoadScene(sceneName);
    
    /// <summary>
    /// Resets progress and starts a new game
    /// </summary>
    private void newGame()
    {
        PersistenceHandler.resetGameProgress();
        switchBetweenMainAndLevelSelect(true);
    }

    /// <summary>
    /// Continues a active game at last scene
    /// </summary>
    private void continueGame()
    {
       switchBetweenMainAndLevelSelect(true);
    }

    /// <summary>
    /// Prepares and launches Sandbox Scene
    /// </summary>
    private void openSandbox()
    {
        openLevel(GameScenes.SANDBOX);
    }

    private void Start()
    {
        newGameORResetBtn.onClick.AddListener(newGame);
        quitBtn.onClick.AddListener(Application.Quit);
        updateUI();
        
        switchBetweenMainAndLevelSelect(isLevelSelection);
    }

    private void updateUI() {
        if (isLevelSelection)
        {
            var clearedLevel = PersistenceHandler.continueGame();
            levelSelectButtons.ForEach(pair =>
            {
                if (pair.level <= clearedLevel + 1)
                {
                    pair.btn.onClick.AddListener(() => openLevel(GameScenes.LEVELS[pair.level]));
                    pair.btn.isDisabled = false;
                    pair.btn.gameObject.GetComponent<CheckedMarker>().updateCheckedState(pair.level <= clearedLevel);
                }
                else
                {
                    pair.btn.isDisabled = true;
                    pair.btn.gameObject.GetComponent<CheckedMarker>().updateCheckedState(false);
                }
            });
            levelSelectionBtnController.index = clearedLevel + 1;
            return;
        }

        // Check if an active Game exists
        if (PersistenceHandler.hasActiveGame())
        {
            continueBtn.thisIndex = 0;
            newGameORResetBtn.thisIndex = 1;
            quitBtn.thisIndex = 2;
            
            mainMenuBtnController.index = 0;
            mainMenuBtnController.maxIndex = 2;
            
            newGameORResetBtnLabel.text = "Reset Game";
            continueBtn.gameObject.SetActive(true);
            continueBtn.onClick.AddListener(continueGame);
        }
        else
        {
            continueBtn.thisIndex = -1;
            newGameORResetBtn.thisIndex = 0;
            quitBtn.thisIndex = 1;
            mainMenuBtnController.index = 0;
            mainMenuBtnController.maxIndex = 1;
            newGameORResetBtnLabel.text = "New Game";
            continueBtn.gameObject.SetActive(false);
            continueBtn.onClick.RemoveAllListeners();
        }
    }

    public void switchBetweenMainAndLevelSelect(bool toLevel)
    {
        isLevelSelection = toLevel;
        if (isLevelSelection)
        {
            main_menu.SetActive(false);
            level_selection.SetActive(true);
            updateUI();
        }
        else
        {
            main_menu.SetActive(true);
            level_selection.SetActive(false);
            updateUI();
        }
    }
}