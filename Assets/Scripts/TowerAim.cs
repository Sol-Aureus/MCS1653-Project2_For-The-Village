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

    [Header("Atributes")]
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        }

    }

    // Gets all the enemies in range and sets the closest one as the target
    private void FindTarget()
    {
        // Find all enemies in range
        RaycastHit2D[] hits = Physics2D.CircleCastAll(rotatePoint.position, range, (Vector2) rotatePoint.position, 0);

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
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        rotatePoint.rotation = targetRotation;
    }

    // Checks if the target is in range and removes the target if it is not
    private bool CheckTargetInRange()
    {
        return Vector2.Distance(rotatePoint.position, target.position) <= range;
    }

    // Draws the range of the tower
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(rotatePoint.position, Vector3.forward, range);
    }
}
