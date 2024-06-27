using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [Space]
    [Header("Stats Settings")]
    [Space]

    [SerializeField] float basicHP = 25f;
    [SerializeField] float basicSpeed = 4f;
    [SerializeField] float basicJumpForce = 10f;

    float actualSpeed = 0;
    float actualHP;
    bool isLookRight = true;
    bool isJumped = false;
    bool deadState = false;

    Rigidbody2D rb;
    RoninBlade roninBlade;
    DamageFlash damageFlash;

    Vector2 movementBounds = new(-22, 10);


    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        roninBlade = GetComponentInChildren<RoninBlade>();
        damageFlash = GetComponent<DamageFlash>();
    }

    private void Start()
    {
        actualHP = basicHP;
    }

    // Update is called once per frame
    private void Update()
    {
        if (deadState || GameManager.instance.IsBlockedInput())
        {
            return;
        }

        BladeAttack();

        if (!roninBlade.IsAttack)
        {
            Movement();
        }
    }

    private void BladeAttack()
    {
        // Check if pointer not over UI element

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            roninBlade.Attack(RoninAttackType.Attack_fast);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            roninBlade.Attack(RoninAttackType.Attack_heavy);
        }
    }

    private void Movement()
    {
        Run();
        Jump();
    }

    private void Run()
    {
        // Get horizontal input from player

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        horizontalInput = CheckOutOfBounds(horizontalInput);
        ConfigureLookDirection(horizontalInput);

        // Set new speed and move player

        actualSpeed = horizontalInput * basicSpeed;
        transform.Translate(Vector2.right * actualSpeed * Time.deltaTime);
    }

    private void ConfigureLookDirection(float horizontalInput)
    {
        // Check if swap is needed and swap look direction 

        bool isSwapRight = (horizontalInput > 0) && !isLookRight;
        bool isSwapLeft = (horizontalInput < 0) && isLookRight;

        if (isSwapRight || isSwapLeft)
        {
            isLookRight = isSwapRight ? true : false;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    private float CheckOutOfBounds(float horizontalInput)
    {
        bool outLeftBound = (transform.position.x <= movementBounds.x) && horizontalInput < 0;
        bool outRightBound = (transform.position.x >= movementBounds.y) && horizontalInput > 0;

        if (outLeftBound || outRightBound)
        {
            horizontalInput = 0;
        }

        return horizontalInput;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumped)
        {
            rb.AddForce(Vector2.up * basicJumpForce, ForceMode2D.Impulse);
            IsJumped = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (IsGroundCollider2D(col))
        {
            IsJumped = false;
        }
    }

    private bool IsGroundCollider2D(Collider2D col)
    {
        return col.GetComponent<EdgeCollider2D>() != null;
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public bool IsJumped
    {
        get { return isJumped; }
        set { isJumped = value; }
    }

    public float GetActualSpeed()
    {
        return Mathf.Abs(actualSpeed);
    }

    public bool IsFalling()
    {
        if (isJumped)
        {
            return rb.velocity.y < 0;
        }

        return false;
    }

    public void TakeDamage(float damage)
    {
        if (deadState)
        {
            return;
        }

        // Apply damage if player not dead yet 

        actualHP -= damage;
        damageFlash.Flash();

        if (IsDead())
        {
            deadState = true;
            actualSpeed = 0;
            rb.simulated = false;

            return;
        }
    }

    public bool IsDead()
    {
        return actualHP <= 0;
    }

    public float GetHpRatio()
    {
        return actualHP / basicHP;
    }

    #endregion
}
