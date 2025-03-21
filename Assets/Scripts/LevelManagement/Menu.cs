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

    // Runs the script to change the GUI
    private void OnGUI()
    {
        currencyUI.text = "$" + LevelManager.instance.currency.ToString();
    }
}
