using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    public static GameManager instance;

    // Pause parameters 

    bool enablePauseGame = true;
    bool blockedInput = false;

    // Load parameters

    LoadManager loadManager;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        loadManager = FindObjectOfType<LoadManager>();
    }

    private void Update()
    {
        PauseSwitch();
        blockedInput = PauseManager.instance.IsPause();
    }

    private void PauseSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && enablePauseGame)
        {
            PauseManager.instance.SwitchPause(true);
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

    public void RestartGame()
    {
        loadManager.ReloadCurrentScene();
        PauseManager.instance.UnPauseGame(true);
    }

    #endregion
}
