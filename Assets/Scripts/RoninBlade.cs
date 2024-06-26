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

    [SerializeField] float fastAttackDmg = 1f;
    [SerializeField] float heavyAttackDmg = 3f;

    [SerializeField] Collider2D fastAttackCollider;
    [SerializeField] Collider2D heavyAttackCollider;
    Collider2D currentAttackCol;

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
        statesController = GetComponentInParent<RoninStatesController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !IsAttack)
        {
            IsAttack = true;
            AttackType = RoninAttackType.Attack_fast;

            currentAttackCol = fastAttackCollider;
            SetAttackState();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !IsAttack)
        {
            IsAttack = true;
            AttackType = RoninAttackType.Attack_heavy;

            currentAttackCol = heavyAttackCollider;
            SetAttackState();
        }
    }

    private void SetAttackState()
    {
        statesController.AttackState(AttackType);
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

    public float GetAttackDmg()
    {
        return AttackType switch
        {
            RoninAttackType.Attack_fast => fastAttackDmg,
            RoninAttackType.Attack_heavy => heavyAttackDmg,
            _ => 0,
        };
    }

    public void EndAttack()
    {
        IsAttack = false;
        DisableDamage();
    }
    
    public void EnableDamage()
    {
        currentAttackCol.gameObject.SetActive(true);
    }

    public void DisableDamage()
    {
        currentAttackCol.gameObject.SetActive(false);
    }

    #endregion
}
