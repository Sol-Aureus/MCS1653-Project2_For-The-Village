using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float Health;

    // Deals damage to the enemy
    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy took damage");

        Health -= damage;
        if (Health <= 0)
        {
            // Calls the event to notify the spawner that an enemy has died
            EnemySpawner.onEnemyDeath.Invoke();
            Destroy(gameObject);
        }
    }
}
