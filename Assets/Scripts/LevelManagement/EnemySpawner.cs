using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI waveUI;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] wave1;
    [SerializeField] private GameObject[] wave2;
    [SerializeField] private GameObject[] wave3;
    [SerializeField] private GameObject[] wave4;
    [SerializeField] private GameObject[] wave5;
    [SerializeField] private GameObject[] wave6;
    [SerializeField] private GameObject[] wave7;
    [SerializeField] private GameObject[] wave8;
    [SerializeField] private GameObject[] wave9;
    [SerializeField] private GameObject[] wave10;

    [Header("Atributes")]
    [SerializeField] private int baseEnemies;
    [SerializeField] private float spawnRate;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float difficultyScaling;
    [SerializeField] private int baseMoney;
    [SerializeField] private float moneyScaling;

    [Header("Events")]
    public static UnityEvent onEnemyDeath = new UnityEvent();

    private float currentSpawnRate;
    public int currentWave = 1;
    private float timeToSpawn = 0;
    private int enemiesAlive;
    private int enemiesToSpawn;
    private bool isSpawning = false;
    private int increment = 0;

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
        if (currentWave == 1)
        {
            return wave1.Length;
        }
        else if (currentWave == 2)
        {
            return wave2.Length;
        }
        else if (currentWave == 3)
        {
            return wave3.Length;
        }
        else if (currentWave == 4)
        {
            return wave4.Length;
        }
        else if (currentWave == 5)
        {
            return wave5.Length;
        }
        else if (currentWave == 6)
        {
            return wave6.Length;
        }
        else if (currentWave == 7)
        {
            return wave7.Length;
        }
        else if (currentWave == 9)
        {
            return wave9.Length;
        }
        else if (currentWave == 10)
        {
            return wave10.Length;
        }
        else
        {
            return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScaling));
        }
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
        GameObject prefabToSpawn;
        if (currentWave == 1)
        {
            prefabToSpawn = wave1[increment];
            increment++;
        }
        else if (currentWave == 2)
        {
            prefabToSpawn = wave2[increment];
            increment++;
        }
        else if (currentWave == 3)
        {
            prefabToSpawn = wave3[increment];
            increment++;
        }
        else if (currentWave == 4)
        {
            prefabToSpawn = wave4[increment];
            increment++;
        }
        else if (currentWave == 5)
        {
            prefabToSpawn = wave5[increment];
            increment++;
        }
        else if (currentWave == 6)
        {
            prefabToSpawn = wave6[increment];
            increment++;
        }
        else if (currentWave == 7)
        {
            prefabToSpawn = wave7[increment];
            increment++;
        }
        else if (currentWave == 8)
        {
            prefabToSpawn = wave8[increment];
        }
        else if (currentWave == 9)
        {
            prefabToSpawn = wave9[increment];
            increment++;
        }
        else if (currentWave == 10)
        {
            prefabToSpawn = wave10[increment];
            increment++;
        }
        else
        {
            // Spawns a random enemy prefab at the first point
            prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        }
        GameObject prefabSpawned = Instantiate(prefabToSpawn, LevelManager.instance.points[0].position, Quaternion.identity); // Quaternion.identity means no rotation
        prefabSpawned.GetComponent<Enemy>().UpdateHealth(Mathf.Pow(currentWave, difficultyScaling));
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
        LevelManager.instance.IncreaseCurrency(Mathf.RoundToInt(baseMoney * Mathf.Pow(currentWave, moneyScaling)));
        
        if (currentWave == 10)
        {
            LevelManager.instance.Win();
        }

        // Increases the wave number and starts the next wave
        currentWave++;
        increment = 0;
        StartCoroutine(StartWave());
    }

    // Runs the script to change the GUI
    private void OnGUI()
    {
        waveUI.text = "Wave: " + currentWave;
    }
}
