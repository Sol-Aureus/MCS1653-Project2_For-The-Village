using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Menu menuToggle;
    [SerializeField] private TextMeshProUGUI cashText;

    private GameObject currentPlacingTower;
    private Vector3 mouseWorldPos;
    private float redFlashTime = 0;

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

        // Flashes the cash red to show you can't afford it
        redFlashTime -= Time.deltaTime;
        if (redFlashTime <= 0)
        {
            cashText.color = new Color(255, 255, 0, 255);
        }
    }

    // Sets the tower that will be placed
    public void SetTowerToPlace(GameObject tower)
    {
        // Checks which tower to place
        if (tower.name == "Tower1")
        {
            // Checks if you have enough
            if (LevelManager.instance.SpendCurrency(tower.GetComponent<Tower>().price))
            {
                menuToggle.ToggleMenu();
                currentPlacingTower = Instantiate(tower, Vector2.zero, Quaternion.identity);
            }
            else
            {
                cashText.color = new Color(255, 0, 0, 255);
                redFlashTime = 0.5f;
            }
        }
        else if (tower.name == "Tower2")
        {
            if (LevelManager.instance.SpendCurrency(tower.GetComponent<Tower>().price))
            {
                menuToggle.ToggleMenu();
                currentPlacingTower = Instantiate(tower, Vector2.zero, Quaternion.identity);
            }
            else
            {
                cashText.color = new Color(255, 0, 0, 255);
                redFlashTime = 0.5f;
            }
        }
    }
}
