using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActionsUI : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    [SerializeField] string gameplaySceneName = "Gameplay";
    [SerializeField] string mainMenuSceneName = "Menu";

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void ExitMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.RestartGame();
        }

    }

    #endregion
}
