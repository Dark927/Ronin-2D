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
    public CustomTime gameTime = new();

    // ---
    // Pause parameters 
    // ---

    bool enablePauseGame = true;
    bool _blockedInput = false;

    // ---
    // Game parameters
    // ---

    float timeCounter = 0;
    bool _gameOver = false;

    int _kills = 0;


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
        if(_gameOver)
        {
            return;
        }

        timeCounter += Time.deltaTime;
        gameTime.UpdateTime(Mathf.RoundToInt(timeCounter));

        PauseSwitch();
        _blockedInput = PauseManager.instance.IsPause();
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

    public int Kills
    {
        get { return _kills; }
    }

    public bool IsBlockedInput()
    {
        return _blockedInput;
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
        _gameOver = true;
        _blockedInput = true;

        foreach(GameObject element in gameplayElementsUI)
        {
            element.SetActive(false);
        }
    }

    public void AddKill()
    {
        _kills++;
        KillsCounterUI.instance.UpdateKills(_kills);
    }

    #endregion
}
