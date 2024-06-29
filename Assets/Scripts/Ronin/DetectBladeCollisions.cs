using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBladeCollisions : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    float dmg;
    float stun;
    float force;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            RoninBlade blade = GetComponentInParent<RoninBlade>();

            dmg = blade.GetAttackDmg();
            stun = blade.GetStunTime();
            force = blade.GetPushForce();

            enemyHealth.TakeDamage(dmg, force, stun);
        }
    }

    #endregion
}
