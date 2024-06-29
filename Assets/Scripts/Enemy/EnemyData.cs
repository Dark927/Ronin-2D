using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entities/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Main Settings")]
    [Space]

    public EnemyType type;
    public float basicHP;
    public float basicSpeed;

    [Space]
    [Header("Attack Settings")]
    [Space]

    public float basicDamage;
    public float attackReloadTime;
}
