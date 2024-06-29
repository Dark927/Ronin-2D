using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Enemy_goblin_default = 0,
    Enemy_goblin_fast = 0,

    Enemy_firstIndex = 0,
    Enemy_lastIndex = Enemy_goblin_fast,
    Enemy_typesCount = Enemy_lastIndex + 1
}

public class Enemy : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

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
    EnemyCollisionsController collisionsController;

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
        collisionsController = GetComponent<EnemyCollisionsController>();
        rb = GetComponent<Rigidbody2D>();
        damageFlash = GetComponent<DamageFlash>();
    }

    private void Start()
    {
        targetPlayer = FindObjectOfType<PlayerController>().gameObject;
        ConfigureStats();
    }

    private void OnEnable()
    {
        if (_isDead)
        {
            Arise();
        }
    }

    private void Update()
    {
        if (!IsDead)
        {
            Movement();
            Attack();
        }
    }

    private void Arise()
    {
        _isDead = false;
        statesController.ResetState();
        rb.simulated = true;

        ConfigureStats();
    }

    private void ConfigureStats()
    {
        _actualHP = stats.basicHP;
        _actualSpeed = stats.basicSpeed;
        _actualDamage = stats.basicDamage;

        weapon.SetReloadTime(stats.attackReloadTime);
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
            StopMovement();
            return;
        }

        // Configure checking player and weapon conditions

        if (!activeSwapSide)
        {
            if(weapon.IsInAttackRange() || IsNearPlayer() || isAttacking)
            {
                StopMovement();
            }
            else
            {
                _actualSpeed = stats.basicSpeed;
            }
        }
    }

    private void StopMovement()
    {
        _actualSpeed = 0;
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
        StopMovement();
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

    private void Die()
    {
        damageFlash.Flash(true);

        _isDead = true;
        statesController.SetDeadState();
        rb.simulated = false;
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
            Die();
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
