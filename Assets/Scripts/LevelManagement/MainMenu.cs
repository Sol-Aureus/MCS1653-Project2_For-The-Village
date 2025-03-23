using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject optionsMenu;
    [SerializeField] private AudioClip[] soundFX;

    private void Awake()
    {
        optionsMenu.SetActive(false);
    }

    public void PlayGame()
    {
        SoundFX.instance.PlaySound(soundFX[0], transform, 0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Options()
    {
        SoundFX.instance.PlaySound(soundFX[0], transform, 0.5f);
        optionsMenu.SetActive(true);
    }

    public void Back()
    {
        SoundFX.instance.PlaySound(soundFX[0], transform, 0.5f);
        optionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
