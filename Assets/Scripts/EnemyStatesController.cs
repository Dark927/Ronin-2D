using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatesController : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    Enemy enemy;
    Animator animator;
    EnemyWeapon weapon;
    bool isDeadAnimation = false;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<EnemyWeapon>();
    }

    private void Update()
    {
        CheckDeadState();

        if (!isDeadAnimation)
        {
            animator.SetFloat("Speed", enemy.ActualSpeed);
        }
    }

    public void SetAttackState()
    {
        animator.SetTrigger("Attack");
    }

    private void CheckDeadState()
    {
        if (isDeadAnimation)
        {
            AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (animInfo.IsName("Death") && animInfo.normalizedTime >= 1.0f)
            {
                animator.speed = 0;
            }
        }
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void SetDeadState()
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("Dead", true);
        isDeadAnimation = true;
    }

    public void SetDamagedState()
    {
        animator.SetTrigger("Damaged");
    }

    #endregion
}
