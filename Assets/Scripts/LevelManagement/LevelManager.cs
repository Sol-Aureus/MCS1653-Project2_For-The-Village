using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Menus")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject deathMenu;
    [SerializeField] GameObject freeplayMenu;
    [SerializeField] GameObject winMenu;

    [Header("Path")]
    public Transform[] points;

    public int currency;

    private bool isPaused = false;
    private bool otherMenu = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        currency = 300;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !otherMenu)
        {
            Pause();
        }
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

    public void Die()
    {
        otherMenu = true;
        deathMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Win()
    {
        otherMenu = true;
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Pause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Home()
    {
        SceneManager.LoadScene(0);

        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Time.timeScale = 1;
    }
}
