using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;

    private int currentTowerIndex = 0;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Gets the current tower
    public GameObject GetCurrentTower()
    {
        return towerPrefabs[currentTowerIndex];
    }
}
