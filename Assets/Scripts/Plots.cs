using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Plots : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Color hoverColor;
    
    private Tilemap tilemap;
    private TilemapRenderer sr;
    private GameObject tower;
    private Color startColor;

    

    // Start is called before the first frame update
    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        sr = GetComponent<TilemapRenderer>();
        //startColor = sr.color;
    }

    // OnMouseEnter is called when the mouse entered the GUIElement or Collider
    private void OnMouseEnter()
    {
        Debug.Log("Mouse entered");
        Vector3Int tilemapPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log(tilemapPos);
        //sr.color = hoverColor;
    }

    // OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider
    private void OnMouseExit()
    {
        //sr.color = startColor;
    }
}
