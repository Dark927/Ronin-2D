using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatesController : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    Animator animator;
    bool isDeadAnimation = false;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckDeadState();
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
        animator.SetBool("Dead", true);
        isDeadAnimation = true;
    }

    public void SetDamagedState()
    {
        animator.SetTrigger("Damaged");
    }

    #endregion
}
