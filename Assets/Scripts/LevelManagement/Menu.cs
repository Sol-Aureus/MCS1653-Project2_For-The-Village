using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private Animator anim;

    private bool isMenuOpen = true;

    // Toggles the menu
    public void ToggleMenu()
    {
        // Toggles the bool
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Runs the script to change the GUI
    private void OnGUI()
    {
        currencyUI.text = "Shop - $" + LevelManager.instance.currency.ToString();
    }
}
