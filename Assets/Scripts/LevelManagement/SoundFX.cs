using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    // Makes functions easy to access
    public static SoundFX instance;

    [Header("References")]
    [SerializeField] AudioSource audioSourcePrefab;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Play sound effect
    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn in game object
        AudioSource audioSource = Instantiate(audioSourcePrefab, spawnTransform.position, Quaternion.identity);

        // Set the audio clip
        audioSource.clip = audioClip;

        // Set the volume
        audioSource.volume = volume;

        // Play the sound
        audioSource.Play();

        // Get length of audio clip
        float clipLength = audioSource.clip.length;

        // Destroy the audio source after the clip has finished playing
        Destroy(audioSource.gameObject, clipLength);
    }

    // Play random sound effect
    public void PlayRandomSound(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        // Assign random audio clip
        int randomIndex = Random.Range(0, audioClip.Length);

        // Spawn in game object
        AudioSource audioSource = Instantiate(audioSourcePrefab, spawnTransform.position, Quaternion.identity);

        // Set the audio clip
        audioSource.clip = audioClip[randomIndex];

        // Set the volume
        audioSource.volume = volume;

        // Play the sound
        audioSource.Play();

        // Get length of audio clip
        float clipLength = audioSource.clip.length;

        // Destroy the audio source after the clip has finished playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
