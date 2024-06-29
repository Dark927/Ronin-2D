using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionsController : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    List<Collider2D> enemyCollidersList = new();

    Enemy currentEnemy;
    Collider2D currentCollider;
    Rigidbody2D rb;

    bool ignoringCollisions = false;
    float minSpeedToIgnore = 1f;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        currentEnemy = GetComponent<Enemy>();
        rb = currentEnemy.GetComponent<Rigidbody2D>();
        currentCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (enemyCollidersList.Count == 0)
        {
            enemyCollidersList = EnemyPool.instance.GetAllColliders();
        }

        ConfigureCollisions();
    }

    private void ConfigureCollisions()
    {
        if ((currentEnemy.ActualSpeed < minSpeedToIgnore) && !ignoringCollisions)
        {
            ignoringCollisions = true;
            IgnoreCollisions(ignoringCollisions);
        }
        else if ((currentEnemy.ActualSpeed > minSpeedToIgnore) && ignoringCollisions)
        {
            ignoringCollisions = false;
            IgnoreCollisions(ignoringCollisions);
        }
    }

    private void IgnoreCollisions(bool ignoreCols)
    {
        rb.isKinematic = ignoreCols;
        FreezePositionY(ignoreCols);

        foreach (Collider2D enemyCol in enemyCollidersList)
        {
            if (enemyCol != currentCollider)
            {
                SetEnemyCollisions(enemyCol, ignoreCols);
            }
        }
    }

    private void SetEnemyCollisions(Collider2D enemyCol, bool ignoreCols)
    {
        if(ignoreCols)
        {
            Physics2D.IgnoreCollision(currentCollider, enemyCol, true);
            return;
        }
        else
        {
            Enemy otherEnemy = enemyCol.GetComponent<Enemy>();
            bool isCollisionsAllowed = (otherEnemy.ActualSpeed > minSpeedToIgnore);

            if(isCollisionsAllowed)
            {
                Physics2D.IgnoreCollision(currentCollider, enemyCol, false);
            }
        }
    }

    private void FreezePositionY(bool freeze)
    {
        if (freeze)
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
    }

    #endregion
}
