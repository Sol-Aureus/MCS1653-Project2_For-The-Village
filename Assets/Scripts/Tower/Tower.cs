using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private GameObject targetingObject;
    [SerializeField] private Transform rotatePoint;
    [SerializeField] private int towerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private CircleCollider2D cantPlaceCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI sellText;

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
    [SerializeField] public int price;
    [SerializeField] private bool isBase;
    [SerializeField] private AudioClip[] soundFX;

    private float health;
    private float currentHealRate;
    private float healCounter;

    private bool isDestroyed = false;

    private Transform target;
    private float timeUntilFire = 0;
    private float targetDistance;
    private float targetDistanceTraveled;
    private float targetHealth;

    public bool canPlace = true;
    public bool canAttack = false;

    private int targetingType = 0;
    public bool delete = false;
    public float countDown = 0;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        health = baseHealth;
        currentHealRate = healRate;
        healCounter = 0;
        healthBar.UpdateHealthBar(Mathf.RoundToInt(health), baseHealth);
        healthBarObject.SetActive(false);
        if (!isBase)
        {
            LevelManager.instance.allTowers.Add(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
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

        // Updates the sell text
        sellText.text = "Sell: $" + GetSellPrice();

        // Clears the screen of towers
        if (delete && countDown < 0)
        {
            Sell();
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
                LevelManager.instance.Die(spawner.currentWave);
            }

            // Destroys the object
            SoundFX.instance.PlaySound(soundFX[1], transform, 1);
            isDestroyed = true;
            if (!isBase)
            {
                LevelManager.instance.allTowers.Remove(gameObject);
            }
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

            if (targetingType == 0)
            {
                // Check if there are farther along enemies
                foreach (RaycastHit2D hit in hits)
                {
                    if (target == null)
                    {
                        // Set the first enemy to enter the range as the target
                        target = hits[0].transform;
                        targetDistanceTraveled = target.GetComponent<Enemy>().distanceTraveled;
                        continue;
                    }

                    Transform temporaryTarget = hit.transform;
                    if (temporaryTarget.GetComponent<Enemy>().distanceTraveled > targetDistanceTraveled)
                    {
                        target = hit.transform;
                        targetDistanceTraveled = target.GetComponent<Enemy>().distanceTraveled;
                    }
                }
            }
            else if (targetingType == 1)
            {
                // Check if there are farther along enemies
                foreach (RaycastHit2D hit in hits)
                {
                    if (target == null)
                    {
                        // Set the first enemy to enter the range as the target
                        target = hits[0].transform;
                        targetDistanceTraveled = target.GetComponent<Enemy>().distanceTraveled;
                        continue;
                    }

                    Transform temporaryTarget = hit.transform;
                    if (temporaryTarget.GetComponent<Enemy>().distanceTraveled < targetDistanceTraveled)
                    {
                        target = hit.transform;
                        targetDistanceTraveled = target.GetComponent<Enemy>().distanceTraveled;
                    }
                }
            }
            else if (targetingType == 2)
            {
                // Check if there are farther along enemies
                foreach (RaycastHit2D hit in hits)
                {
                    if (target == null)
                    {
                        // Set the first enemy to enter the range as the target
                        target = hits[0].transform;
                        targetHealth = target.GetComponent<Enemy>().health;
                        continue;
                    }

                    Transform temporaryTarget = hit.transform;
                    if (temporaryTarget.GetComponent<Enemy>().health > targetHealth)
                    {
                        target = hit.transform;
                        targetHealth = target.GetComponent<Enemy>().health;
                    }
                }
            }
            else if (targetingType == 3)
            {
                // Check if there are farther along enemies
                foreach (RaycastHit2D hit in hits)
                {
                    if (target == null)
                    {
                        // Set the first enemy to enter the range as the target
                        target = hits[0].transform;
                        targetHealth = target.GetComponent<Enemy>().health;
                        continue;
                    }

                    Transform temporaryTarget = hit.transform;
                    if (temporaryTarget.GetComponent<Enemy>().health < targetHealth)
                    {
                        target = hit.transform;
                        targetHealth = target.GetComponent<Enemy>().health;
                    }
                }
            }
            else if (targetingType == 4)
            {
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
            else if (targetingType == 5)
            {
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

                    if (Vector2.Distance(rotatePoint.position, hit.transform.position) > targetDistance)
                    {
                        target = hit.transform;
                        targetDistance = Vector2.Distance(rotatePoint.position, target.position);
                    }
                }
            }
            else if (targetingType == 6)
            {
                target = hits[Random.Range(0, hits.Length)].transform;
            }
        }
    }

    // Rotates the tower towards the target
    private void RotateTowardsTarget()
    {
        // Get the angle between the target and the tower
        float angle = Mathf.Atan2(target.position.y - rotatePoint.position.y, target.position.x - rotatePoint.position.x) * Mathf.Rad2Deg;
        if (angle < 90 && angle > -90)
        {
            spriteRenderer.flipX = false;
        }
        else if (angle > 90 || angle < -90)
        {
            spriteRenderer.flipX = true;
        }
        animator.SetFloat("Angle", angle);

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
        // Plays the sound effect
        SoundFX.instance.PlaySound(soundFX[0], transform, 0.8f);

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

    // Runs when a collider is clicked
    private void OnMouseDown()
    { 
        if (canPlace)
        {
            targetingObject.SetActive(true);
            // Close other menus
            LevelManager.instance.CloseTowerMenus(gameObject);
        }
    }

    // Allows the tower to change targeting
    public void ChangeTargeting(int newTargeting)
    {
        targetingType = newTargeting;
    }

    // Closes the tower menu
    public void Close()
    {
        targetingObject.SetActive(false);
    }

    // Sells the tower
    public void Sell()
    {
        // Gives money proportial to the tower's health
        LevelManager.instance.IncreaseCurrency(GetSellPrice());
        Destroy(gameObject);
    }

    private int GetSellPrice()
    {
        return (int)((price * 0.25f) + (((price * 0.75f) * (health / baseHealth))));
    }
}
