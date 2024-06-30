using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    [SerializeField] TextMeshProUGUI survivedTimeText; 
    [SerializeField] TextMeshProUGUI killedEnemiesText;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void OnEnable()
    {
        UpdateSurvivedTime();
        UpdateKilledEnemies();
    }

    private void UpdateKilledEnemies()
    {
        killedEnemiesText.text = GameManager.instance.Kills.ToString("D4");
    }

    private void UpdateSurvivedTime()
    {
        int minutes = GameManager.instance.gameTime.Minutes;
        int seconds = GameManager.instance.gameTime.Seconds;

        survivedTimeText.text = minutes.ToString("D2") + ':' + seconds.ToString("D2");
    }

    #endregion
}
