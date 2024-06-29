using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    [Header("Configuration")]
    [Space]

    [SerializeField] List<GameObject> gameplayElementsUI;

    public static GameManager instance;

    // ---
    // Pause parameters 
    // ---

    bool enablePauseGame = true;
    bool blockedInput = false;

    // ---
    // Game parameters
    // ---

    public CustomTime gameTime = new();
    float timeCounter = 0;
    bool gameOver = false;

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

    private void Update()
    {
        if(gameOver)
        {
            return;
        }

        timeCounter += Time.deltaTime;
        gameTime.UpdateTime(Mathf.RoundToInt(timeCounter));

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
        LoadManager.instance.ReloadCurrentScene();
        PauseManager.instance.UnPauseGame(true);
    }

    public bool IsPointingUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void SetGameOver()
    {
        gameOver = true;
        blockedInput = true;

        foreach(GameObject element in gameplayElementsUI)
        {
            element.SetActive(false);
        }
    }

    #endregion
}
