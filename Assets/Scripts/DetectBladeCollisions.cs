using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBladeCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            RoninBlade blade = GetComponentInParent<RoninBlade>();
            float dmg = blade.GetAttackDmg();

            enemy.TakeDamage(dmg);
        }
    }
}
