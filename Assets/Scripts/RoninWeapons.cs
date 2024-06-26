using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoninWeapons : MonoBehaviour
{
    RoninBlade blade;

    private void Awake()
    {
        blade = GetComponentInChildren<RoninBlade>();
    }

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
}
