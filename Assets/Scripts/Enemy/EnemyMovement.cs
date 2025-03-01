using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pathOffset;

    public float currentMoveSpeed;
    private Transform target;
    private Vector3 targetPosition;
    private int pathIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = LevelManager.instance.points[pathIndex];
        targetPosition = target.position;
        transform.position = targetPosition;
        currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards the target
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Change target
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
                // Set the new target
                target = LevelManager.instance.points[pathIndex];
                targetPosition = target.position + new Vector3(Random.Range(-pathOffset * 100, pathOffset * 100) / 100, (Random.Range(-pathOffset * 100, pathOffset * 100) + pathOffset) / 100, 0);
            }
        }
    }

    // FixedUpdate is called a fixed number of times per second
    private void FixedUpdate()
    {
        // Move towards the target
        Vector2 direction = (targetPosition - transform.position).normalized; // Get the direction to the target between 0 and 1

        rb.velocity = direction * currentMoveSpeed;
    }

    public void ResetSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }
}
