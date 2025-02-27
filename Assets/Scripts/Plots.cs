using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Plots : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Color hoverColor;

    private SpriteRenderer sr;
    private GameObject tower;
    private Color startColor;

    // Start is called before the first frame update
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;
    }

    // OnMouseEnter is called when the mouse entered the GUIElement or Collider
    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    // OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider
    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    private void OnMouseDown()
    {
        if (tower == null)
        {
            // Get the current tower
            GameObject towerToBuild = BuildManager.instance.GetCurrentTower();

            // Instantiate the tower in the plot with the same position and rotation
            Instantiate(towerToBuild, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            tower = towerToBuild;

            // Tells the tower what plot it's on
            tower.GetComponent<TowerHealth>().SetPlot(this);
        }
    }

    // Removes the tower from the plot
    public void TowerDestroyed() 
    {
        tower = null;
    }
}
