using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [Header("Stats Settings")]
    [Space]
    [SerializeField] float basicSpeed = 4f;
    [SerializeField] float basicJumpForce = 10f;

    float actualSpeed = 0;
    bool isLookRight = true;
    bool isJumped = false;

    Rigidbody2D rb;
    RoninBlade roninBlade;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        roninBlade = GetComponentInChildren<RoninBlade>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!roninBlade.IsAttack)
        {
            Movement();
        }

    }

    private void Movement()
    {
        Run();
        Jump();
    }

    private void Run()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        bool isSwapRight = (horizontalInput > 0) && !isLookRight;
        bool isSwapLeft = (horizontalInput < 0) && isLookRight;

        if (isSwapRight || isSwapLeft)
        {
            isLookRight = isSwapRight ? true : false;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        actualSpeed = horizontalInput * basicSpeed;
        transform.Translate(Vector2.right * actualSpeed * Time.deltaTime);
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

    public bool IsJumped
    {
        get { return isJumped; }
        set { isJumped = value; }
    }

    #endregion
}
