using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Menu menuToggle;

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
                menuToggle.ToggleMenu();
            }
        }
    }

    // Sets the tower that will be placed
    public void SetTowerToPlace(GameObject tower)
    {
        // Checks which tower to place
        if (tower.name == "Tower1")
        {
            // Checks if you have enough
            if (LevelManager.instance.SpendCurrency(250))
            {
                menuToggle.ToggleMenu();
                currentPlacingTower = Instantiate(tower, Vector2.zero, Quaternion.identity);
            }
        }
        else if (tower.name == "Tower2")
        {
            if (LevelManager.instance.SpendCurrency(200))
            {
                menuToggle.ToggleMenu();
                currentPlacingTower = Instantiate(tower, Vector2.zero, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("Not enough");
        }
    }
}
