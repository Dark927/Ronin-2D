using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entities/Ronin Data")]
public class RoninData : ScriptableObject
{
    [Space]
    [Header("Main Settings")]
    [Space]

    public float basicHP;
    public float basicSpeed;
    public float basicJumpForce;
}
