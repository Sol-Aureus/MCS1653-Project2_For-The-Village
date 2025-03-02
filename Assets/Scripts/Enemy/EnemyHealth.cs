using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthBar healthBar;

    [Header("Attributes")]
    [SerializeField] private float baseHealth;

    private float scaledHealth;
    private float health;
    private bool isDestroyed = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        health = baseHealth;
        healthBar.UpdateHealthBar(health, baseHealth);
    }

    // Deals damage to the enemy
    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy took damage");

        // Updates the health
        health -= damage;
        healthBar.UpdateHealthBar(health, scaledHealth);

        if (health <= 0 && !isDestroyed)
        {
            // Calls the event to notify the spawner that an enemy has died
            EnemySpawner.onEnemyDeath.Invoke();
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    public void UpdateHealth(float scaling)
    {
        scaledHealth = Mathf.RoundToInt(baseHealth * scaling);
        health = scaledHealth;
        healthBar.UpdateHealthBar(health, scaledHealth);
    }
}
