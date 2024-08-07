using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(Animator))]
public class RoninStatesController : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    PlayerController player;
    Animator animator;

    bool isAnimationActive = false;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateSpeed();
        JumpState();

        if(player.IsDead())
        {
            DeadState();
        }
    }

    private void UpdateSpeed()
    {
        animator.SetFloat("Speed", player.GetActualSpeed());
    }

    private void JumpState()
    {
        if (player.IsJumped)
        {
            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
        }

        if (player.IsFalling())
        {
            animator.SetTrigger("Fall");
        }
        else
        {
            animator.SetBool("Grounded", true);
        }
    }

    public void DeadState()
    {
        animator.SetTrigger("Dead");
        isAnimationActive = true;

        if (isAnimationActive)
        {
            AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (animInfo.normalizedTime >= 1.0f && animInfo.IsName("Death"))
            {
                animator.speed = 0;
                isAnimationActive = false;
            }
        }
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void AttackState(RoninAttackType attackType)
    {
        switch (attackType)
        {
            case RoninAttackType.Attack_fast:
                {
                    animator.SetTrigger("Attack_fast");
                }
                break;


            case RoninAttackType.Attack_heavy:
                {
                    animator.SetTrigger("Attack_heavy");
                }
                break;
        }
    }

    #endregion
}
