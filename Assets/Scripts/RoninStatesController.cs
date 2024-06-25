using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(Animator))]
public class RoninStatesController : MonoBehaviour
{
    PlayerController player;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", player.GetActualSpeed());
    }
}
