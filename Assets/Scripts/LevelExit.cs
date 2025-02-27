using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
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
        Debug.Log("Base took damage");

        health -= damage;
        if (health <= 0 && !isDestroyed)
        {
            // Calls the event to notify the spawner that a tower has died
            Debug.Log("Called game over!");

            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
