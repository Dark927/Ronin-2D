using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entities/EnemySpawn Data")]
public class EnemySpawnData : ScriptableObject
{
    [Space]
    [Header("Global Spawn Settings")]
    [Space]

    public GameObject enemyContainer;
    public GameObject enemyPrefab;
    public int maxCount;
}
