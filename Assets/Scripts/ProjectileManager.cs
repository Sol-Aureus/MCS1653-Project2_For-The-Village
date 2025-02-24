using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Atributes")]
    private Transform orientation;
    private float speed;
    private float damage;
    private float pierce;
    private float lifeTime;

    private Transform target;
    private Vector2 direction;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        orientation = GetComponent<Transform>();
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

    public void SetRotation(Quaternion newRotation)
    {
        orientation.rotation = newRotation;
    }

    // Called when the projectile collides with something
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Deal damage to the enemy
        other.GetComponent<EnemyHealth>().TakeDamage(damage);
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
