using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileManager : ProjectileManager
{
    private bool isDestroyed = false;

    // Called when the projectile collides with something
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Stops all calulations once the projectile is destroyed
        if (isDestroyed) return;

        // Deal damage to the enemy
        other.GetComponent<Tower>().TakeDamage(damage);

        // Check if the projectile has pierce
        if (pierce <= 0)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
        else
        {
            pierce--;
        }
    }
}
