using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    // Pause parameters 

    PauseManager pauseManager;
    bool enablePauseGame = true;
    bool blockedInput = false;

    // Load parameters

    LoadManager loadManager;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        pauseManager = FindObjectOfType<PauseManager>();
        loadManager = FindObjectOfType<LoadManager>();
    }

    private void Update()
    {
        PauseSwitch();
    }

    private void PauseSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && enablePauseGame)
        {
            pauseManager.SwitchPause(true);
            blockedInput = pauseManager.IsPause();
        }
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public bool IsBlockedInput()
    {
        return blockedInput;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        loadManager.ReloadCurrentScene();
        pauseManager.UnPauseGame();
    }

    #endregion
}
