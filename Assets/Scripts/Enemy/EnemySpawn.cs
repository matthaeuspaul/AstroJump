using System.Collections;
using UnityEngine;
using TMPro;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool; // Reference to the EnemyPool script
    [SerializeField] private Transform[] spawnPoints; // Array of spawn point transforms
    [SerializeField] private int[] enemiesPerWave; // e.g., [5, 10, 15]
    [SerializeField] private float timeBetweenWaves = 5f; // Time between waves in seconds
    [SerializeField] private Transform playerTransform; // Reference to the player's transform

    [Header("UI Elements")]
    [SerializeField] private GameObject waveInfoUI; // UI element to display wave information
    [SerializeField] private TMP_Text waveNumberText; // Text to display current wave number
    [SerializeField] private TMP_Text enemiesAliveText; // Text to display number of alive enemies
    [SerializeField] private TMP_Text nextWaveCountdownText; // Text to display countdown to next wave

    private int currentWave = 0; // Current wave index
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;
    private float waveTimer = 0f;
    private bool waveActive = false;

    void Start()
    {
        UpdateWaveInfoUI();
        StartNextWave();
    }

    void Update()
    {
        // If no enemies are alive and there are more waves to spawn
        if (enemiesAlive == 0 && currentWave < enemiesPerWave.Length)
        {
            if (!waveActive) // Only count down if no wave is active
            {
                waveTimer += Time.deltaTime;
                UpdateCountdownUI();

                if (waveTimer >= timeBetweenWaves)
                {
                    waveTimer = 0f;
                    StartNextWave();
                }
            }
        }
        // Update UI during active wave
        else if (waveActive)
        {
            UpdateWaveInfoUI();
        }

        // Check if all waves are completed
        if (currentWave >= enemiesPerWave.Length && enemiesAlive == 0)
        {
            ShowAllWavesCompleted();
        }
    }

    void StartNextWave()
    {
        waveActive = true;
        enemiesSpawned = 0;
        enemiesAlive = enemiesPerWave[currentWave];

        // Display wave start information
        StartCoroutine(ShowWaveStartInfo());

        // Spawn enemies for the current wave
        for (int i = 0; i < enemiesPerWave[currentWave]; i++)
        {
            SpawnEnemy();
        }

        currentWave++;
        UpdateWaveInfoUI();
    }

    public IEnumerator ShowWaveStartInfo()
    {
        waveInfoUI.SetActive(true);

        // Display wave start information
        if (waveNumberText != null)
            waveNumberText.text = $"WAVE {currentWave + 1} INCOMING!";
        if (enemiesAliveText != null)
            enemiesAliveText.text = $"Enemies: {enemiesAlive}";
        if (nextWaveCountdownText != null)
            nextWaveCountdownText.text = "";

        yield return new WaitForSeconds(5f);

        // Activate wave info UI for the ongoing wave
        waveActive = true;
        UpdateWaveInfoUI();
    }

    void UpdateWaveInfoUI()
    {
        if (waveInfoUI != null)
            waveInfoUI.SetActive(true);

        // Current Wave Number
        if (waveNumberText != null)
        {
            if (currentWave < enemiesPerWave.Length || enemiesAlive > 0)
                waveNumberText.text = $"Wave: {currentWave}/{enemiesPerWave.Length}";
            else
                waveNumberText.text = "All Waves Complete!";
        }

        // Number of alive enemies
        if (enemiesAliveText != null)
            enemiesAliveText.text = $"Enemies Alive: {enemiesAlive}";
    }

    void UpdateCountdownUI()
    {
        float timeRemaining = timeBetweenWaves - waveTimer;

        if (nextWaveCountdownText != null)
            nextWaveCountdownText.text = $"Next Wave in: {timeRemaining:F1}s";
    }

    void ShowAllWavesCompleted()
    {
        waveActive = false;

        if (waveInfoUI != null)
            waveInfoUI.SetActive(true);

        if (waveNumberText != null)
            waveNumberText.text = "ALL WAVES COMPLETED!";
        if (enemiesAliveText != null)
            enemiesAliveText.text = "";
        if (nextWaveCountdownText != null)
            nextWaveCountdownText.text = "";
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = enemyPool.GetEnemy(spawnPoint.position, spawnPoint.rotation, playerTransform);

        // Callback for Enemy Death
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
            enemyScript.OnDeath = OnEnemyDeath;
    }

    void OnEnemyDeath()
    {
        enemiesAlive--;

        // Check if wave is completed
        if (enemiesAlive <= 0)
        {
            waveActive = false;
            waveTimer = 0f; // Reset timer for next wave
        }

        // Update UI after enemy death
        UpdateWaveInfoUI();
    }
}