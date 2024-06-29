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

    [SerializeField] EnemyData _stats;

    float _actualSpeed;
    float _actualDamage;
    bool _isDead = false;


    [Space]
    [Header("Main Settings")]
    [Space]

    [SerializeField] float sideSwapDelay = 0.5f;
    int _lookDirectionX = 1;
    bool isLookRight = true;
    bool isAttacking = false;
    bool isStunned = false;

    Coroutine activeSideSwap = null;

    [Space]
    [Header("Configuration Settings")]
    [Space]

    [SerializeField] EnemyWeapon weapon;
    EnemyStatesController statesController;
    EnemyCollisionsController collisionsController;

    Rigidbody2D rb;
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
        // Configure checking input conditions

        if (GameManager.instance.IsBlockedInput() || _isDead || isStunned)
        {
            StopMovement();
            return;
        }

        Movement();
        Attack();
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
        _actualSpeed = _stats.basicSpeed;
        _actualDamage = _stats.basicDamage;

        weapon.SetReloadTime(_stats.attackReloadTime);
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
        // Configure checking player and weapon conditions

        if (activeSideSwap == null)
        {
            if (weapon.IsInAttackRange() || IsNearPlayer() || isAttacking)
            {
                StopMovement();
            }
            else
            {
                _actualSpeed = _stats.basicSpeed;
            }
        }
    }

    private void StopMovement()
    {
        _actualSpeed = 0;
    }

    private void CalculatePosition()
    {
        _lookDirectionX = (targetPlayer.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        Vector2 movement = Vector2.right * _lookDirectionX * _actualSpeed * Time.deltaTime;

        transform.Translate(movement);
    }

    private bool IsNearPlayer()
    {
        return Mathf.Abs(targetPlayer.transform.position.x - transform.position.x) < 0.05;
    }

    private void ConfigureLookDirection()
    {
        bool swapToRight = ((_lookDirectionX < 0) && isLookRight);
        bool swapToLeft = ((_lookDirectionX > 0) && !isLookRight);

        bool isSwapping = !((swapToRight || swapToLeft) && (activeSideSwap == null));

        if (!isSwapping)
        {
            activeSideSwap = StartCoroutine(BecomeStunnedRoutine(sideSwapDelay, true));
            SwapLookDirection();
        }
    }

    IEnumerator BecomeStunnedRoutine(float time, bool swappingSide = false)
    {
        StopMovement();
        isStunned = true;

        yield return new WaitForSeconds(time);

        isStunned = false;
        _actualSpeed = _stats.basicSpeed;

        if (swappingSide)
        {
            activeSideSwap = null;
        }
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
        get { return _stats.type; }
    }

    public bool IsAttack
    {
        get { return isAttacking; }
    }

    public EnemyData Stats
    {
        get { return _stats; }
    }

    public int LookDirectionX
    {
        get { return _lookDirectionX; }
    }

    #endregion

    public void BecomeStunned(float time)
    {
        StopAllCoroutines();
        StartCoroutine(BecomeStunnedRoutine(time));
    }

    public void Die()
    {
        _isDead = true;
        statesController.SetDeadState();
        rb.simulated = false;

        GameManager.instance.AddKill();
    }

    public void HitPlayer()
    {
        weapon.Attack(_actualDamage);
        isAttacking = false;
    }

    #endregion
}
