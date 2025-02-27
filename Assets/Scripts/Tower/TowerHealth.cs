using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float baseHealth;
    [SerializeField] private bool isBase;

    private float health;
    private bool isDestroyed = false;
    public Plots towerPlot;

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
