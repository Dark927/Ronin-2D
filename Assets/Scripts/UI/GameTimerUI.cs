using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimerUI : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    TextMeshProUGUI timerText;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        int timeInMinutes = GameManager.instance.gameTime.Minutes;
        int timeInSeconds = GameManager.instance.gameTime.Seconds;

        timerText.text = timeInMinutes.ToString("D2") + ':' + (timeInSeconds).ToString("D2");
    }

    #endregion
}
