using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    bool playerInAttackRange = false;
    bool isReloaded = true;
    float reloadTime = 1.5f;

    PlayerController player;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsPlayerCollision(collision))
        {
            playerInAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsPlayerCollision(collision))
        {
            playerInAttackRange = false;
        }
    }

    private bool IsPlayerCollision(Collider2D collision)
    {
        return collision.GetComponent<PlayerController>() != null;
    }

    IEnumerator ReloadDelay(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        isReloaded = true;
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void SetReloadTime(float time)
    {
        reloadTime = time;
    }

    public bool ReadyToAttack()
    {
        return playerInAttackRange && isReloaded;
    }

    public bool IsInAttackRange()
    {
        return playerInAttackRange;
    }

    public void ChargeAttack()
    {
        isReloaded = false;
        StartCoroutine(ReloadDelay(reloadTime));
    }

    public void Attack(float damage)
    {
        if (IsInAttackRange())
        {
            player.TakeDamage(damage);
        }
    }

    #endregion
}
