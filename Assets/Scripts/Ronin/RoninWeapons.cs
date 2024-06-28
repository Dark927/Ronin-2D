using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoninWeapons : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    RoninBlade blade;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        blade = GetComponentInChildren<RoninBlade>();
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void BladeAttackEnd()
    {
        blade.EndAttack();
    }

    public void EnableBladeDamage()
    {
        blade.EnableDamage();
    }

    public void DisableBladeDamage()
    {
        blade.DisableDamage();
    }

    #endregion
}
