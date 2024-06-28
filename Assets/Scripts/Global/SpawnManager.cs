using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [SerializeField] List<SpawnPoint> spawners;
    [SerializeField] float spawnRate = 1f;
    EnemyPool enemyPool;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        enemyPool = GetComponent<EnemyPool>();
    }

    private void Start()
    {
        StartEnemySpawning(spawnRate);
    }

    private IEnumerator EnemySpawnRoutine(float spawnRate)
    {
        while (true)
        {
            GameObject enemy = enemyPool.RequestEnemyObject(EnemyType.Enemy_goblin);

            if (enemy != null)
            {
                int randomSpawnerIndex = Random.Range(0, spawners.Count);
                spawners[randomSpawnerIndex].SpawnEnemy(enemy);
            }
            yield return new WaitForSeconds(spawnRate);
        }
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
