using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    public static PauseManager instance;

    [SerializeField] GameObject pausePanelUI;
    bool isGamePaused = false;
    bool isPausePanelActive = false;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!isGamePaused && !isPausePanelActive)
        {
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        ConfigurePauseState();
    }

    private void ConfigurePauseState()
    {
        isPausePanelActive = pausePanelUI.activeInHierarchy;

        if (!isGamePaused && isPausePanelActive)
        {
            PauseGame();
        }
        else if(isGamePaused && !isPausePanelActive)
        {
            UnPauseGame();
        }
    }

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
            isPausePanelActive = true;
        }
    }

    public void UnPauseGame(bool deactivatePausePanel = false)
    {
        Time.timeScale = 1;
        isGamePaused = false;

        if(deactivatePausePanel)
        {
            pausePanelUI.SetActive(false);
            isPausePanelActive = false;
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

    public bool PausePanelActive()
    {
        return isPausePanelActive;
    }

    #endregion
}
