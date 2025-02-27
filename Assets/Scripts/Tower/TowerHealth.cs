using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float baseHealth;

    private float health;
    private bool isDestroyed = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        health = baseHealth;
    }

    // Deals damage to the enemy
    public void TakeDamage(float damage)
    {
        Debug.Log("Tower took damage");

        health -= damage;
        if (health <= 0 && !isDestroyed)
        {
            // Calls the event to notify the spawner that a tower has died
            Plots.onTowerDeath.Invoke();

            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
