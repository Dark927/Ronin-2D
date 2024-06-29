using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    public static EnemyPool instance;

    [SerializeField] List<EnemySpawnData> spawnDataList;
    List<List<Enemy>> pool;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        InitPool();
    }

    private void InitPool()
    {
        // Initialize pool and add all enemies using spawn data

        pool = new List<List<Enemy>>();

        foreach(EnemySpawnData data in spawnDataList)
        {
            PushSpawnData(data);
        }
    }

    private void PushSpawnData(EnemySpawnData spawnData)
    {
        pool.Add(new());

        int maxEnemyCount = spawnData.maxCount;

        // Add enemies of current type to pool using spawn data

        GameObject containerPrefab = spawnData.enemyContainer;
        GameObject spawnContainer = Instantiate(containerPrefab, transform.position, Quaternion.identity, transform);

        spawnContainer.name = containerPrefab.name;


        for (int enemyIndex = 0; enemyIndex < maxEnemyCount; ++enemyIndex)
        {
            GameObject enemyPrefab = spawnData.enemyPrefab;
            GameObject enemyToAdd = Instantiate(enemyPrefab, transform.position, Quaternion.identity, spawnContainer.transform);

            enemyToAdd.name = enemyPrefab.name;
            enemyToAdd.SetActive(false);

            int lastIndex = pool.Count - 1;
            pool[lastIndex].Add(enemyToAdd.GetComponent<Enemy>());
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
        for (int enemyTypeIndex = 0; enemyTypeIndex < pool.Count; ++enemyTypeIndex)
        {
            if (pool[enemyTypeIndex][0].Type != type)
            {
                continue;
            }

            return GetInactiveEnemy(enemyTypeIndex);
        }
        return null;
    }

    public void AddNewEnemy(EnemySpawnData data)
    {
        PushSpawnData(data);
    }

    public List<Collider2D> GetAllColliders()
    {
        List<Collider2D> allColliders = new();

        for (int enemyTypeIndex = 0; enemyTypeIndex < pool.Count; ++enemyTypeIndex)
        {
            for (int enemyIndex = 0; enemyIndex < pool[enemyTypeIndex].Count; ++enemyIndex)
            {
                Collider2D enemyCollider = pool[enemyTypeIndex][enemyIndex].GetComponent<Collider2D>();
                allColliders.Add(enemyCollider);
            }
        }

        return allColliders;
    }
    #endregion
}
