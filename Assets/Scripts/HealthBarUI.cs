using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    [SerializeField] Image healthBarFill;
    PlayerController player;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if(player == null)
        {
            Debug.Log("# Fatal error : PlayerController can not be found. - " + gameObject.name);
        }
    }

    private void LateUpdate()
    {
        Transform healthFillTransform = healthBarFill.transform;

        healthFillTransform.localScale = new Vector2(player.GetHpRatio(), healthFillTransform.localScale.y);
    }
    
    #endregion
}
