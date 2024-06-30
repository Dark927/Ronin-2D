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

    float _currentDefaultSpeed;
    float _speed;
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
    [Header("Run away Settings")]
    [Space]

    [SerializeField] float runAwayOffsetX = 20f;
    [SerializeField] float runAwayTimeDelay = 2f;
    [SerializeField] float runAwayDistance = 10f;


    [Space]
    [Header("Configuration Settings")]
    [Space]

    [SerializeField] EnemyWeapon weapon;

    EnemyStatesController statesController;
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

        if (_isDead || isStunned)
        {
            StopMovement();
            return;
        }
        else if (GameManager.instance.IsBlockedInput())
        {
            return;
        }

        FollowPlayer();
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
        _currentDefaultSpeed = Random.Range(_stats.minSpeed, _stats.maxSpeed);

        _speed = _currentDefaultSpeed;
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

    private void FollowPlayer()
    {
        ConfigureSpeed();

        if (!isAttacking)
        {
            CalculatePosition(targetPlayer.transform.position);
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
                _speed = _currentDefaultSpeed;
            }
        }
    }

    private void StopMovement()
    {
        _speed = 0;
    }

    private void CalculatePosition(Vector2 targetPosition)
    {
        _lookDirectionX = (targetPosition.x - transform.position.x) > 0 ? 1 : -1;
        Vector2 movement = Vector2.right * _lookDirectionX * _speed * Time.deltaTime;

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
        }
    }

    private IEnumerator BecomeStunnedRoutine(float time, bool swappingSide = false)
    {
        StopMovement();
        isStunned = true;

        yield return new WaitForSeconds(time);

        isStunned = false;
        _speed = _currentDefaultSpeed;

        if (swappingSide)
        {
            SwapLookDirection();
            activeSideSwap = null;
        }
    }

    private IEnumerator RunAwayRoutine(Vector2 offset, float runAwayTimeDelay = 1f)
    {
        StopMovement();

        yield return new WaitForSeconds(runAwayTimeDelay);

        Vector2 startPosition = transform.position;
        _speed = _currentDefaultSpeed;

        while (true)
        {
            bool farFromPlayer = Mathf.Abs(transform.position.x - targetPlayer.transform.position.x) > runAwayDistance;

            if (farFromPlayer)
            {
                gameObject.SetActive(false);
                break;
            }

            CalculatePosition(startPosition + offset);
            ConfigureLookDirection();

            yield return null;
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
        get { return _speed; }
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

    public void RunAway()
    {
        if (!gameObject.activeInHierarchy || _isDead)
        {
            return;
        }

        StopAllCoroutines();
        isStunned = false;

        int runAwayDirection = (Random.Range(0, 2) == 0) ? 1 : -1;
        Vector2 runAwayOffset = new((runAwayOffsetX * runAwayDirection), transform.position.y);

        StartCoroutine(RunAwayRoutine(runAwayOffset, runAwayTimeDelay));
    }

    #endregion
}
