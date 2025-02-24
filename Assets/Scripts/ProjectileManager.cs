using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Atributes")]
    public float speed;
    public float damage;
    public float pierce;
    public float lifeTime;

    private Transform target;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FixedUpdate is called a fixed number of times per second
    private void FixedUpdate()
    {
        // Reduces the lifeTime of the projectile
        lifeTime -= Time.fixedDeltaTime * speed;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }

        // Check if there is a target
        if (!target)
        {
            return;
        }

        // Moves the projectile towards the target
        rb.velocity = direction * speed;
    }

    // Sets the target of the projectile
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        direction = (target.position - transform.position).normalized;
    }

    // Sets the damage of the projectile
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    // Sets the pierce of the projectile
    public void SetPierce(float newPierce)
    {
        pierce = newPierce;
    }

    // Sets the lifeTime of the projectile
    public void SetlifeTime(float newlifeTime)
    {
        lifeTime = newlifeTime;
    }

    // Sets the speed of the projectile
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Called when the projectile collides with something
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Deal damage to the enemy
        other.GetComponent<EnemyMovement>().TakeDamage(damage);
        // Check if the projectile has pierce
        if (pierce <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            pierce--;
        }
    }
}
