using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [Space]
    [Header("Spawn Settings")]
    [Space]

    [SerializeField] List<SpawnPoint> spawners;
    [SerializeField] float spawnRate = 1f;
    [SerializeField] int spawnCount = 2;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        StartEnemySpawning(spawnRate);
    }

    private IEnumerator EnemySpawnRoutine(float spawnRate)
    {
        while (true)
        {
            SpawnEnemies(spawnCount);
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnEnemies(int spawnCount)
    {
        for (int enemyIndex = 0; enemyIndex < spawnCount; ++enemyIndex)
        {
            GameObject enemy = EnemyPool.instance.RequestEnemyObject(EnemyType.Enemy_goblin_default);

            if (enemy != null)
            {
                SpawnNewEnemy(enemy);
            }
        }
    }

    private void SpawnNewEnemy(GameObject enemy)
    {
        int randomSpawnerIndex = Random.Range(0, spawners.Count);
        spawners[randomSpawnerIndex].SpawnEnemy(enemy);
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void StartEnemySpawning(float spawnRate = 1f)
    {
        StartCoroutine(EnemySpawnRoutine(spawnRate));
    }

    public void EndEnemySpawning()
    {
        StopAllCoroutines();
    }

    #endregion
}
