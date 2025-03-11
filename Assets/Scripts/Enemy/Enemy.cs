using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private Transform rotatePoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pathOffset;
    [SerializeField] private float baseHealth;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;
    [SerializeField] private float pierce;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    private float currentMoveSpeed;
    private Transform pathTarget;
    private Vector3 pathTargetPosition;
    private int pathIndex = 0;
    private float pathTolerance = 0.2f;

    private float scaledHealth;
    private float health;
    private bool isDestroyed = false;

    private Transform target;
    private float timeUntilFire = 0;
    private float targetDistance;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        health = baseHealth;
        healthBar.UpdateHealthBar(health, baseHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pathTarget = LevelManager.instance.points[pathIndex];
        pathTargetPosition = pathTarget.position;
        transform.position = pathTargetPosition;
        currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards the pathTarget
        if (Vector2.Distance(transform.position, pathTargetPosition) < pathTolerance)
        {
            // Change pathTarget
            pathIndex++;

            // Temporary solution as the enemy has no way to stop moving
            if (pathIndex >= LevelManager.instance.points.Length)
            {
                // Calls the event to notify the spawner that an enemy has died
                EnemySpawner.onEnemyDeath.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                // Set the new pathTarget
                pathTarget = LevelManager.instance.points[pathIndex];
                pathTargetPosition = pathTarget.position + new Vector3(Random.Range(-pathOffset * 100, pathOffset * 100) / 100, (Random.Range(-pathOffset * 100, pathOffset * 100) + pathOffset) / 100, 0);
            }
        }

        // Counts the time until the next shot
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= fireRate)
        {
            FindTarget();

            // If there is a target, rotate towards it
            if (target == null)
            {
                currentMoveSpeed = moveSpeed;
            }
            else
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
                    // Fires if the time until the next shot is greater than the fire rate
                    currentMoveSpeed = 0;
                    Fire();
                    timeUntilFire = 0;
                }
            }
        }


        if (health == baseHealth)
        {
            healthBarObject.SetActive(false);
        }
        else
        {
            healthBarObject.SetActive(true);
        }
    }

    // FixedUpdate is called a fixed number of times per second
    private void FixedUpdate()
    {
        // Move towards the pathTarget
        Vector2 direction = (pathTargetPosition - transform.position).normalized; // Get the direction to the pathTarget between 0 and 1

        rb.velocity = direction * currentMoveSpeed;
    }

    // Deals damage to the enemy
    public void TakeDamage(float damage)
    {
        // Updates the health
        health -= damage;
        healthBar.UpdateHealthBar(Mathf.RoundToInt(health), scaledHealth);
        LevelManager.instance.IncreaseCurrency((int)damage);

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
                    target = hit.transform;
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
        EnemyProjectileManager projectileScript = projectileObject.GetComponent<EnemyProjectileManager>();
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
}
