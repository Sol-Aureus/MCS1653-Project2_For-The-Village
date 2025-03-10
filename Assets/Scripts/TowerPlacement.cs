using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private GameObject currentPlacingTower;
    private Vector3 mouseWorldPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the tower exists
        if (currentPlacingTower != null)
        {
            // Casts a ray from the camera to the mouse
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Move the tower there
            currentPlacingTower.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, -4);

            // If click
            if (Input.GetMouseButtonDown(0) && currentPlacingTower.GetComponent<Tower>().canPlace)
            {
                // Sets the tower to the propper layer
                currentPlacingTower.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);

                // Allows the tower to attack
                currentPlacingTower.GetComponent<Tower>().canAttack = true;

                // Place tower
                currentPlacingTower = null;
            }
        }
    }

    // Sets the tower that will be placed
    public void SetTowerToPlace(GameObject tower)
    {
        currentPlacingTower = Instantiate(tower, Vector2.zero, Quaternion.identity);
    }
}
