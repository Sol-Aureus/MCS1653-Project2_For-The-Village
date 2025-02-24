using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float baseHealth;

    private float health;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        health = baseHealth;
    }

    // Deals damage to the enemy
    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy took damage");

        health -= damage;
        if (health <= 0)
        {
            // Calls the event to notify the spawner that an enemy has died
            EnemySpawner.onEnemyDeath.Invoke();
            Destroy(gameObject);
        }
    }

    public void UpdateHealth(float scaling)
    {
        health = Mathf.RoundToInt(baseHealth * scaling);
    }
}
