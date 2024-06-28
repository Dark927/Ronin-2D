using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Enemy_goblin = 0,

    Enemy_firstIndex = 0,
    Enemy_lastIndex = Enemy_goblin,
    Enemy_typesCount = Enemy_lastIndex + 1
}

public class Enemy : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [Space]
    [Header("Stats Settings")]
    [Space]

    [SerializeField] EnemyData stats;


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
    DamageFlash damageFlash;
    GameObject targetPlayer;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        statesController = GetComponent<EnemyStatesController>();
        rb = GetComponent<Rigidbody2D>();
        damageFlash = GetComponent<DamageFlash>();
    }

    private void Start()
    {
        targetPlayer = FindObjectOfType<PlayerController>().gameObject;

        _actualHP = stats.basicHP;
        _actualSpeed = stats.basicSpeed;
        _actualDamage = stats.basicDamage;

        weapon.SetReloadTime(stats.attackReloadTime);
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
            weapon.ChargeAttack();
            statesController.SetAttackState();
        }
    }

    private void Movement()
    {
        ConfigureSpeed();

        if (!isAttacking)
        {
            CalculatePosition();
            ConfigureLookDirection();
        }
    }

    private void ConfigureSpeed()
    {
        // Configure checking input conditions

        if (GameManager.instance.IsBlockedInput())
        {
            _actualSpeed = 0;
            return;
        }

        // Configure checking player and weapon conditions

        if (!activeSwapSide)
        {
            _actualSpeed = weapon.IsInAttackRange() ? 0 : stats.basicSpeed;
            _actualSpeed = IsNearPlayer() ? 0 : _actualSpeed;
        }
    }

    private void CalculatePosition()
    {
        lookDirectionX = (targetPlayer.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        Vector2 movement = Vector2.right * lookDirectionX * _actualSpeed * Time.deltaTime;

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

    private bool IsNearPlayer()
    {
        return Mathf.Abs(targetPlayer.transform.position.x - transform.position.x) < 0.05;
    }

    IEnumerator StopMovementDelay(float stopTime, bool isSideSwap = false)
    {
        _actualSpeed = 0;
        activeSwapSide = true;

        yield return new WaitForSeconds(stopTime);

        _actualSpeed = stats.basicSpeed;

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

    // -------------------------
    // Getters and Setters
    // -------------------------

    #region Getters and Setters

    public float ActualSpeed
    {
        get { return _actualSpeed; }
    }

    public bool IsDead
    {
        get { return _isDead; }
    }

    public EnemyType Type
    {
        get { return stats.type; }
    }

    public bool IsAttack
    {
        get { return isAttacking; }
    }

    #endregion

    public void TakeDamage(float damage, float pushForce = 1f, float stopTime = 0.5f)
    {
        _actualHP -= damage;
        damageFlash.Flash();

        StartCoroutine(StopMovementDelay(stopTime));

        if (_actualHP <= 0)
        {
            damageFlash.Flash(true);

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

    #endregion
}
