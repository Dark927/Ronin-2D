using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [SerializeField] List<EnemySpawnData> spawnDataList;
    List<Enemy>[] pool;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        // Initialize pool and add all enemies using spawn data

        pool = new List<Enemy>[spawnDataList.Count];

        for (int enemyTypeIndex = 0; enemyTypeIndex < pool.Length; ++enemyTypeIndex)
        {
            pool[enemyTypeIndex] = new List<Enemy>();
            FillPoolList(enemyTypeIndex);
        }
    }

    private void FillPoolList(int enemyTypeIndex)
    {
        int maxEnemyCount = spawnDataList[enemyTypeIndex].maxCount;

        // Add enemies of current type to pool using spawn data

        GameObject containerPrefab = spawnDataList[enemyTypeIndex].enemyContainer;
        GameObject spawnContainer = Instantiate(containerPrefab, transform.position, Quaternion.identity, transform);

        spawnContainer.name = containerPrefab.name;


        for (int enemyIndex = 0; enemyIndex < maxEnemyCount; ++enemyIndex)
        {
            GameObject enemyPrefab = spawnDataList[enemyTypeIndex].enemyPrefab;
            GameObject enemyToAdd = Instantiate(enemyPrefab, transform.position, Quaternion.identity, spawnContainer.transform);

            enemyToAdd.name = enemyPrefab.name;
            enemyToAdd.SetActive(false);

            pool[enemyTypeIndex].Add(enemyToAdd.GetComponent<Enemy>());
        }
    }

    private GameObject GetInactiveEnemy(int enemyTypeIndex)
    {
        GameObject foundedEnemy = null;

        foreach (Enemy enemy in pool[enemyTypeIndex])
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                foundedEnemy = enemy.gameObject;
            }
        }

        return foundedEnemy;
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public GameObject RequestEnemyObject(EnemyType type)
    {
        for (int enemyTypeIndex = 0; enemyTypeIndex < pool.Length; ++enemyTypeIndex)
        {
            if (pool[enemyTypeIndex][0].Type != type)
            {
                continue;
            }

            return GetInactiveEnemy(enemyTypeIndex);
        }
        return null;
    }

    #endregion
}
