using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int[] enemiesPerWave; // e.g., [5, 10, 15]
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private GameObject waveInfoUI; // UI-Element zur Anzeige der Welleninformationen

    private int currentWave = 0;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;
    private float waveTimer = 0f;

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (enemiesAlive == 0 && currentWave < enemiesPerWave.Length)
        {
            waveTimer += Time.deltaTime;
            if (waveTimer >= timeBetweenWaves)
            {
                waveTimer = 0f;
                StartNextWave();
            }
        }
        ShowWaveInfo();
    }
    
    void StartNextWave()
    {
        StartCoroutine("ShowWaveInfo");
        waveInfoUI.SetActive(true);
        enemiesSpawned = 0;
        enemiesAlive = enemiesPerWave[currentWave];
        for (int i = 0; i < enemiesPerWave[currentWave]; i++)
        {
            SpawnEnemy();
        }
        currentWave++;
    }

    public IEnumerator ShowWaveInfo()
    {
        waveInfoUI.SetActive(true);
        // Hier können Sie den Text des UI-Elements aktualisieren, z.B. "Wave X"
        waveInfoUI.GetComponentInChildren<TMPro.TMP_Text>().text = $"Wave {currentWave + 1} incoming. Enemies left: {enemiesAlive}";
        yield return new WaitForSeconds(10f); // Anzeigezeit
        waveInfoUI.SetActive(false);
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = enemyPool.GetEnemy(spawnPoint.position, spawnPoint.rotation, playerTransform);

        // Optional: Callback für Enemy-Death registrieren
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
            enemyScript.OnDeath = OnEnemyDeath;
    }

    void OnEnemyDeath()
    {
        enemiesAlive--;
    }
}
