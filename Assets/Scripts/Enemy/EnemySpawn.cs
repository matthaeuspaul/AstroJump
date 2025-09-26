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
    private float nextSceneTimer = 0f;

    private enum WaveState
    {
        WaitingForNextWave,
        ShowingWaveInfo,
        WaveActive,
        AllWavesCompleted,
        TransitioningToNextScene
    }

    private WaveState currentState = WaveState.WaitingForNextWave;
    private bool allWavesCompleted = false; // Flag to track if all waves are defeated

    [SerializeField] private string nextScene; // Name of the next scene to load after all waves are defeated
    [SerializeField] private float countdownToNextScene = 5f; // Countdown time before loading next scene

    void Start()
    {
        UpdateWaveInfoUI();
        // Instantly start the first wave
        StartCoroutine(StartNextWaveCoroutine());
    }

    void Update()
    {
        // Check if all waves are completed first
        if (currentWave >= enemiesPerWave.Length && enemiesAlive == 0 && !allWavesCompleted)
        {
            allWavesCompleted = true;
            currentState = WaveState.AllWavesCompleted;
            ShowAllWavesCompleted();
        }

        // Handle countdown to next scene
        if (allWavesCompleted && currentState == WaveState.TransitioningToNextScene)
        {
            nextSceneTimer += Time.deltaTime;
            UpdateNextSceneCountdown();

            if (nextSceneTimer >= countdownToNextScene)
            {
                LevelLoadingManagerer.instance.StartLevelTransition(nextScene);
            }
            return; // Don't process other logic when transitioning
        }

        // If waiting for next wave, update timer and check if it's time to start the next wave
        if (currentState == WaveState.WaitingForNextWave)
        {
            waveTimer += Time.deltaTime;
            UpdateCountdownUI();

            if (waveTimer >= timeBetweenWaves)
            {
                waveTimer = 0f;
                StartCoroutine(StartNextWaveCoroutine());
            }
        }
        // Update UI during active wave
        else if (currentState == WaveState.WaveActive)
        {
            UpdateWaveInfoUI();
        }
    }

    private IEnumerator StartNextWaveCoroutine()
    {
        if (currentWave >= enemiesPerWave.Length) yield break;

        currentState = WaveState.ShowingWaveInfo;
        enemiesSpawned = 0;
        enemiesAlive = enemiesPerWave[currentWave];

        // Display wave start information
        yield return StartCoroutine(ShowWaveStartInfo());

        // Spawn enemies for the current wave
        currentState = WaveState.WaveActive;
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

        yield return new WaitForSeconds(3f); 
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
                waveNumberText.text = "All WAVES COMPLETED";
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

    void UpdateNextSceneCountdown()
    {
        float timeRemaining = countdownToNextScene - nextSceneTimer;

        if (nextWaveCountdownText != null)
            nextWaveCountdownText.text = $"Loading next level in: {timeRemaining:F1}s";
    }

    void ShowAllWavesCompleted()
    {
        currentState = WaveState.TransitioningToNextScene;

        if (waveInfoUI != null)
            waveInfoUI.SetActive(true);

        if (waveNumberText != null)
            waveNumberText.text = "";
        if (enemiesAliveText != null)
            enemiesAliveText.text = "ALL WAVES COMPLETED";
        if (nextWaveCountdownText != null)
            nextWaveCountdownText.text = $"Loading next level in: {countdownToNextScene:F1}s";
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
        if (enemiesAlive <= 0 && currentState == WaveState.WaveActive)
        {
            // Wave completed, wait for next wave if any left
            if (currentWave < enemiesPerWave.Length)
            {
                currentState = WaveState.WaitingForNextWave;
                waveTimer = 0f; // Reset timer for next wave
            }
        }

        // Update UI after enemy death
        UpdateWaveInfoUI();
    }
}