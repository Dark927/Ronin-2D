using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [SerializeField] float basicHP = 5f;
    [SerializeField] float basicSpeed = 4f;
    [SerializeField] float basicDamage = 2f;

    float _actualHP;
    float _actualSpeed;
    float _actualDamage;
    bool _isDead = false;

    int lookDirectionX = 1;

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
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void TakeDamage(float damage, float pushForce = 1f)
    {
        _actualHP -= damage;
        statesController.SetDamagedState();

        if (_actualHP <= 0)
        {
            _isDead = true;
            statesController.SetDeadState();
            rb.simulated = false;
        }

        rb.AddForce(Vector2.right * lookDirectionX * pushForce, ForceMode2D.Impulse);
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
