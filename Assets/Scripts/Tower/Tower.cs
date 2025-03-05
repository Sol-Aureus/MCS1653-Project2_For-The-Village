using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private Transform rotatePoint;
    [SerializeField] private int towerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private CapsuleCollider2D cantPlaceCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Attributes")]
    [SerializeField] private float baseHealth;
    [SerializeField] private float healRate;
    [SerializeField] private float rampUp;
    [SerializeField] private float maxRampUp;
    [SerializeField] private float range;
    [SerializeField] private float lifeTime;
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;
    [SerializeField] private float pierce;
    [SerializeField] private float speed;
    [SerializeField] private bool isBase;

    private float health;
    private float currentHealRate;
    private float healCounter;

    private bool isDestroyed = false;

    private Transform target;
    private float timeUntilFire = 0;
    private float targetDistance;

    public bool canPlace = true;
    public bool canAttack = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        health = baseHealth;
        currentHealRate = healRate;
        healCounter = 0;
        healthBar.UpdateHealthBar(Mathf.RoundToInt(health), baseHealth);
        healthBarObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Stops the tower from doing any calculations while being placed
        if (!canAttack)
        {
            gameObject.layer = default;
            return;
        }

        gameObject.layer = towerLayer;

        // Counts the time until the next shot
        timeUntilFire += Time.deltaTime;

        // Counts down the timer
        healCounter -= Time.deltaTime;

        // Only checks for enemies when about to fire
        if (timeUntilFire >= fireRate)
        {
            FindTarget();

            // If there is a target, rotate towards it
            if (target != null)
            {
                RotateTowardsTarget();

                // Check if the target is in range
                if (!CheckTargetInRange())
                {
                    // Removing the target
                    target = null;
                }
                else
                {
                    Fire();
                    timeUntilFire = 0;
                }
            }
        }

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
            if (health > baseHealth)
            {
                health = baseHealth;
            }
            healthBar.UpdateHealthBar(Mathf.RoundToInt(health), baseHealth);
        }

        if (health == baseHealth && !isBase)
        {
            healthBarObject.SetActive(false);
        }
        else
        {
            healthBarObject.SetActive(true);
        }
    }

    // Deals damage to the enemy
    public void TakeDamage(float damage)
    {
        // Updates the health
        health -= damage;
        healthBar.UpdateHealthBar(Mathf.RoundToInt(health), baseHealth);
        healCounter = 2;
        currentHealRate = healRate;

        if (health <= 0 && !isDestroyed)
        {
            if (isBase)
            {
                // Calls the event to end the game
                Debug.Log("Called game over!");
            }

            // Destroys the object
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    // Gets all the enemies in range and sets the closest one as the target
    private void FindTarget()
    {
        // Find all enemies in range
        RaycastHit2D[] hits = Physics2D.CircleCastAll(rotatePoint.position, range, (Vector2)rotatePoint.position, 0, enemyLayer);

        // If there are enemies in range, set the first one as the target
        if (hits.Length > 0)
        {
            target = null;

            // Check if there are closer enemies
            foreach (RaycastHit2D hit in hits)
            {
                if (target == null)
                {
                    // Set the first enemy to enter the range as the target
                    target = hits[0].transform;
                    targetDistance = Vector2.Distance(rotatePoint.position, target.position);
                    continue;
                }

                if (Vector2.Distance(rotatePoint.position, hit.transform.position) < targetDistance)
                {
                    target = hit.transform;
                    targetDistance = Vector2.Distance(rotatePoint.position, target.position);
                }
            }
        }
    }

    // Rotates the tower towards the target
    private void RotateTowardsTarget()
    {
        // Get the angle between the target and the tower
        float angle = Mathf.Atan2(target.position.y - rotatePoint.position.y, target.position.x - rotatePoint.position.x) * Mathf.Rad2Deg;

        // Rotate the tower towards the target
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rotatePoint.rotation = targetRotation;
    }

    // Checks if the target is in range and removes the target if it is not
    private bool CheckTargetInRange()
    {
        return Vector2.Distance(rotatePoint.position, target.position) <= range;
    }

    // Fires a projectile at the target
    private void Fire()
    {
        // Instantiates a projectile at the spawn point
        GameObject projectileObject = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Gets the projectile manager component and sets the target
        TowerProjectileManager projectileScript = projectileObject.GetComponent<TowerProjectileManager>();
        projectileScript.SetTarget(target);
        projectileScript.SetDamage(damage);
        projectileScript.SetPierce(pierce);
        projectileScript.SetSpeed(speed);
        projectileScript.SetlifeTime(range * lifeTime);
        projectileScript.SetRotation(rotatePoint.rotation);
    }

    // Draws the range of the tower
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(rotatePoint.position, Vector3.forward, range);
    }

    // Detects when an object is colliding with it
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("CantPlace") && !isBase)
        {
            spriteRenderer.color = Color.red;
            canPlace = false;
        }
    }

    // Detects when an object is colliding with it
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CantPlace") && !isBase)
        {
            spriteRenderer.color = Color.white;
            canPlace = true;
        }
    }
}
