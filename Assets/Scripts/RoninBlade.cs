using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoninAttackType
{
    Attack_fast = 0,
    Attack_heavy = 1,
}

public class RoninBlade : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [SerializeField] Collider2D fastAttackCollider;
    [SerializeField] Collider2D heavyAttackCollider;

    bool _isAttack = false;
    RoninAttackType _attackType = RoninAttackType.Attack_fast;

    RoninStatesController statesController;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        statesController = GetComponent<RoninStatesController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !IsAttack)
        {
            IsAttack = true;
            AttackType = RoninAttackType.Attack_fast;

            SetAttackColliderState(fastAttackCollider, true);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !IsAttack)
        {
            IsAttack = true;
            AttackType = RoninAttackType.Attack_heavy;

            SetAttackColliderState(heavyAttackCollider, true);
        }
    }

    private void EndFastAttack()
    {
        IsAttack = false;
        SetAttackColliderState(fastAttackCollider, false);
    }

    private void EndHeavyAttack()
    {
        IsAttack = false;
        SetAttackColliderState(heavyAttackCollider, false);
    }

    private void SetAttackColliderState(Collider2D col, bool isActive)
    {
        col.gameObject.SetActive(isActive);

        if(isActive)
        {
            statesController.AttackState();
        }
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public RoninAttackType AttackType
    {
        get { return _attackType; }
        set { _attackType = value; }
    }

    public bool IsAttack
    {
        get { return _isAttack; }
        set { _isAttack = value; }
    }

    #endregion
}
