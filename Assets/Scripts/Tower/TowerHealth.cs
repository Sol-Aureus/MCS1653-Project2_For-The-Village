using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthBar healthBar;

    [Header("Attributes")]
    [SerializeField] private float baseHealth;
    [SerializeField] private float healRate;
    [SerializeField] private float rampUp;
    [SerializeField] private float maxRampUp;
    [SerializeField] private bool isBase;

    private float health;
    private float currentHealRate;

    private bool isDestroyed = false;
    public Plots towerPlot;

    private float healCounter;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        health = baseHealth;
        currentHealRate = healRate;
        healCounter = 0;
        healthBar.UpdateHealthBar(health, baseHealth);
    }

    // FixedUpdate is called a fixed number of times per second
    private void FixedUpdate()
    {
        // Counts down the timer
        healCounter -= Time.fixedDeltaTime;

        // Checks if the timer is up
        if (healCounter < 0)
        {
            if (health < baseHealth)
            {
                // Resest the timer
                healCounter = 0.25f;

                // Heals the tower (percent based)
                health += (baseHealth * currentHealRate);

                // Ramps up the heal rate
                currentHealRate += rampUp;

                // Checks if the ramp up is too high
                if (currentHealRate > (healRate * maxRampUp))
                {
                    // Sets it back to the max
                    currentHealRate = (healRate * maxRampUp);
                }
            }
            else
            {
                health = baseHealth;
            }
            healthBar.UpdateHealthBar(health, baseHealth);
        }
    }

    // Deals damage to the enemy
    public void TakeDamage(float damage)
    {
        Debug.Log("Tower took damage");

        // Updates the health
        health -= damage;
        healthBar.UpdateHealthBar(health, baseHealth);
        healCounter = 2;
        currentHealRate = healRate;

        if (health <= 0 && !isDestroyed)
        {
            if (!isBase)
            {
                // Calls the event to notify the spawner that a tower has died
                towerPlot.TowerDestroyed();
            }
            else
            {
                // Calls the event to end the game
                Debug.Log("Called game over!");
            }
            
            // Destroys the object
            isDestroyed = true;
            Destroy(gameObject);
        }
    }


    // Sets the plot the tower is on
    public void SetPlot(Plots plot)
    {
        towerPlot = plot;
    }
}
