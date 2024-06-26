using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [Space]
    [Header("Stats Settings")]
    [Space]

    [SerializeField] float basicHP = 5f;
    [SerializeField] float basicSpeed = 4f;
    [SerializeField] float basicDamage = 2f;
    [SerializeField] float attackReloadTime = 1.5f;


    float _actualHP;
    float _actualSpeed;
    float _actualDamage;
    bool _isDead = false;

    [Space]
    [Header("Main Settings")]
    [Space]

    [SerializeField] float sideSwapDelay = 0.5f;
    int lookDirectionX = 1;
    bool isLookRight = true;
    bool activeSwapSide = false;

    bool isAttacking = false;

    [Space]
    [Header("Configuration Settings")]
    [Space]

    [SerializeField] EnemyWeapon weapon;
    EnemyStatesController statesController;
    Rigidbody2D rb;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        statesController = GetComponent<EnemyStatesController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _actualHP = basicHP;
        _actualSpeed = basicSpeed;
        _actualDamage = basicDamage;

        weapon.SetReloadTime(attackReloadTime);
    }

    private void Update()
    {
        if (!IsDead)
        {
            Movement();
            Attack();
        }
    }

    private void Attack()
    {
        if (!isAttacking && weapon.ReadyToAttack())
        {
            isAttacking = true;
            statesController.SetAttackState();
        }
    }

    private void Movement()
    {

        if (!isAttacking)
        {
            if (!activeSwapSide)
            {
                _actualSpeed = (weapon.IsInAttackRange()) ? 0 : basicSpeed;
            }

            CalculatePosition();
            ConfigureLookDirection();
        }
    }

    private void CalculatePosition()
    {
        GameObject target = FindObjectOfType<PlayerController>().gameObject;
        lookDirectionX = (target.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        Vector2 movement = Vector2.right * lookDirectionX * ActualSpeed * Time.deltaTime;

        transform.Translate(movement);
    }


    private void ConfigureLookDirection()
    {
        bool swapToRight = ((lookDirectionX < 0) && isLookRight);
        bool swapToLeft = ((lookDirectionX > 0) && !isLookRight);

        if ((swapToRight || swapToLeft) && !activeSwapSide)
        {
            StartCoroutine(StopMovementDelay(sideSwapDelay, true));
        }
    }

    IEnumerator StopMovementDelay(float stopTime, bool isSideSwap = false)
    {
        _actualSpeed = 0;
        activeSwapSide = true;

        yield return new WaitForSeconds(stopTime);

        _actualSpeed = basicSpeed;

        if (isSideSwap)
        {
            SwapLookDirection();
        }

        activeSwapSide = false;
    }

    private void SwapLookDirection()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        isLookRight = !isLookRight;
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void TakeDamage(float damage, float pushForce = 1f, float stopTime = 0.5f)
    {
        _actualHP -= damage;
        statesController.SetDamagedState();

        StartCoroutine(StopMovementDelay(stopTime));

        if (_actualHP <= 0)
        {
            _isDead = true;
            statesController.SetDeadState();
            rb.simulated = false;
        }

        rb.AddForce(Vector2.right * lookDirectionX * pushForce, ForceMode2D.Impulse);
    }

    public void HitPlayer()
    {
        weapon.Attack(_actualDamage);
        isAttacking = false;
    }

    public float ActualSpeed
    {
        get { return _actualSpeed; }
    }

    public bool IsDead
    {
        get { return _isDead; }
    }

    #endregion
}
