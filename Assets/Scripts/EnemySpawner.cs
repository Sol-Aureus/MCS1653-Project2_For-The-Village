using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Atributes")]
    [SerializeField] private int baseEnemies;
    [SerializeField] private float spawnRate;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float difficultyScaling;

    [Header("Events")]
    public static UnityEvent onEnemyDeath = new UnityEvent();

    private float currentSpawnRate;
    private int currentWave = 1;
    private float timeToSpawn = 0;
    private int enemiesAlive;
    private int enemiesToSpawn;
    private bool isSpawning = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize the event
        onEnemyDeath.AddListener(EnemyDestroyed);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Starts the first wave
        StartCoroutine(StartWave());
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the spawner is currently spawning
        if (!isSpawning)
        {
            return;
        }

        // Counting the timer
        timeToSpawn += Time.deltaTime;

        // Spawns an enemy if the time to spawn is greater than the spawn rate and there are still enemies to spawn
        if (timeToSpawn >= (currentSpawnRate + (Random.Range(-100, 100) / 1000)) && enemiesToSpawn > 0)
        {
            SpawnEnemy();
            enemiesToSpawn--;
            enemiesAlive++;
            timeToSpawn = 0;
        }

        if (enemiesToSpawn <= 0 && enemiesAlive <= 0)
        {
            EndWave();
        }
    }

    // Determines the number of enemies to spawn based on the current wave
    private int EnemiesPerWave()
    {
        // Sets the number of enemies to spawn based on the current wave
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScaling));
    }

    // Determines the spawn rate based on the number of enemies to spawn
    private float SpawnRatePerWave()
    {
        // Sets the rate of spawning based on the current wave
        return spawnRate / Mathf.Pow(currentWave, difficultyScaling);
    }

    // Starts the wave
    private IEnumerator StartWave()
    {
        // Waits for the time between waves
        yield return new WaitForSeconds(timeBetweenWaves);

        // Sets the number of enemies to spawn based on the current wave
        enemiesToSpawn = EnemiesPerWave();
        currentSpawnRate = SpawnRatePerWave();
        isSpawning = true;
    }

    // Spawns an enemy
    private void SpawnEnemy()
    {
        // Spawns a random enemy prefab at the first point
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject prefabSpawned = Instantiate(prefabToSpawn, LevelManager.instance.points[0].position, Quaternion.identity); // Quaternion.identity means no rotation
        prefabSpawned.GetComponent<EnemyHealth>().UpdateHealth(Mathf.Pow(currentWave, difficultyScaling));
    }

    // Called when an enemy is destroyed
    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void EndWave()
    {
        // Resets the variables
        isSpawning = false;
        timeToSpawn = 0;

        // Increases the wave number and starts the next wave
        currentWave++;
        StartCoroutine(StartWave());
    }
}
