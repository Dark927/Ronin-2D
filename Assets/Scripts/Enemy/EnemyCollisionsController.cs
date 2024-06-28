using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionsController : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    Enemy currentEnemy;
    Collider2D currentCollider;

    List<Collider2D> enemyCollidersList = new();
    bool isIgnoring = false;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        currentEnemy = GetComponent<Enemy>();
        currentCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Enemy[] enemyAll = FindObjectsOfType<Enemy>(true);

        foreach (Enemy enemy in enemyAll)
        {
            enemyCollidersList.Add(enemy.GetComponent<Collider2D>());
        }
    }

    private void Update()
    {
        UpdateCollisionStatus();
    }

    private void UpdateCollisionStatus()
    {
        if (currentEnemy.IsAttack && !isIgnoring)
        {
            SetCollisionStatus(true);
        }
        else if (!currentEnemy.IsAttack && isIgnoring)
        {
            SetCollisionStatus(false);
        }
    }

    private void SetCollisionStatus(bool ignore)
    {
        foreach (Collider2D enemyCol in enemyCollidersList)
        {
            if (currentCollider != enemyCol)
            {
                Physics2D.IgnoreCollision(currentCollider, enemyCol, ignore);
            }
        }
        isIgnoring = ignore;
    }

    #endregion
}
