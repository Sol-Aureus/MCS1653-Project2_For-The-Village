using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Path")]
    public Transform[] points;

    public int currency;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        currency = 250;
    }

    // This will increase the currency
    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    // This will spend the currency
    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            // If able to buy, return true
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough currency");
            return false;
        }
    }
}
