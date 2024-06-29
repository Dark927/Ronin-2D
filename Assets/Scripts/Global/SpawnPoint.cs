using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    Collider2D spawnArea;
    float minSpawnX;
    float maxSpawnX;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Parameters

    private void Awake()
    {
        spawnArea = GetComponent<Collider2D>();

        minSpawnX = spawnArea.bounds.min.x;
        maxSpawnX = spawnArea.bounds.max.x;
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void SpawnEnemy(GameObject enemy)
    {
        Vector3 randomPositionX = transform.position;
        randomPositionX.x = Random.Range(minSpawnX, maxSpawnX);

        enemy.transform.position = randomPositionX;
        enemy.SetActive(true);
    }

    #endregion
}
