using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectileManager : ProjectileManager
{
    // Called when the projectile collides with something
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Deal damage to the enemy
        other.GetComponent<Enemy>().TakeDamage(damage);
        // Check if the projectile has pierce
        if (pierce <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            pierce--;
        }
    }
}
