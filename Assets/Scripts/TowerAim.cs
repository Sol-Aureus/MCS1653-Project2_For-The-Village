using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class TowerAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform rotatePoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [Header("Atributes")]
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;
    [SerializeField] private float pierce;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    private Transform target;
    private float timeUntilFire = 0;

    // Update is called once per frame
    void Update()
    {
        FindTarget();

        // Counts the time until the next shot
        timeUntilFire += Time.deltaTime;

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
                // Fires if the time until the next shot is greater than the fire rate
                if (timeUntilFire >= fireRate)
                {
                    Fire();
                    timeUntilFire = 0;
                }
            }
        }

    }

    // Gets all the enemies in range and sets the closest one as the target
    private void FindTarget()
    {
        // Find all enemies in range
        RaycastHit2D[] hits = Physics2D.CircleCastAll(rotatePoint.position, range, (Vector2) rotatePoint.position, 0, enemyLayer);

        // If there are enemies in range, set the first one as the target
        if (hits.Length > 0)
        {
            // Set the first enemy to enter the range as the target
            target = hits[0].transform;

            // Check if there are closer enemies
            foreach (RaycastHit2D hit in hits)
            {
                if (Vector2.Distance(rotatePoint.position, hit.transform.position) < Vector2.Distance(rotatePoint.position, target.position))
                {
                    target = hit.transform;
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
        ProjectileManager projectileScript = projectileObject.GetComponent<ProjectileManager>();
        projectileScript.SetTarget(target);
        projectileScript.SetDamage(damage);
        projectileScript.SetPierce(pierce);
        projectileScript.SetSpeed(speed);
        projectileScript.SetlifeTime(lifeTime);
        projectileScript.SetRotation(rotatePoint.rotation);
    }

    // Draws the range of the tower
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(rotatePoint.position, Vector3.forward, range);
    }
}
