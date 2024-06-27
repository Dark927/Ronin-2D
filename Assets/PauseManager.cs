using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    [SerializeField] GameObject pausePanelUI;
    bool isGamePaused = false;

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public bool IsPause()
    {
        return isGamePaused;
    }

    public void PauseGame(bool activatePausePanel = false)
    {
        Time.timeScale = 0;
        isGamePaused = true;

        if(activatePausePanel)
        {
            pausePanelUI.SetActive(true);
        }
    }

    public void UnPauseGame(bool deactivatePausePanel = false)
    {
        Time.timeScale = 1;
        isGamePaused = false;

        if(deactivatePausePanel)
        {
            pausePanelUI.SetActive(false);
        }
    }

    public void SwitchPause(bool usePausePanel = false)
    {
        if(isGamePaused)
        {
            UnPauseGame(usePausePanel);
        }
        else
        {
            PauseGame(usePausePanel);
        }
    }

    #endregion
}
