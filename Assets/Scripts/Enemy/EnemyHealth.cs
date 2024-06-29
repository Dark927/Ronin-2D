using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    ParticleSystem bloodSplash;

    Enemy enemy;
    DamageFlash damageFlash;
    Rigidbody2D rb;

    float _basicHP;
    float _actualHP;


    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        damageFlash = GetComponent<DamageFlash>();
        rb = GetComponent<Rigidbody2D>();

        bloodSplash = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        _basicHP = enemy.Stats.basicHP;
        _actualHP = _basicHP;
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void TakeDamage(float damage, float pushForce = 1f, float stopTime = 0.5f)
    {
        _actualHP -= damage;
        damageFlash.Flash();

        enemy.BecomeStunned(stopTime);

        if (_actualHP <= 0)
        {
            damageFlash.Flash(true);
            bloodSplash.Play();
            enemy.Die();
            _actualHP = _basicHP;
        }

        rb.AddForce(Vector2.right * -enemy.LookDirectionX * pushForce, ForceMode2D.Impulse);
    }

    #endregion
}
