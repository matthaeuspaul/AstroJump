using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy(Vector3 position, Quaternion rotation, Transform playerTransform)
    {
        GameObject enemy;
        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
            enemy.transform.position = position;
            enemy.transform.rotation = rotation;
            enemy.SetActive(true);
        }
        else
        {
            enemy = Instantiate(enemyPrefab, position, rotation, transform);
        }

        // Reset-Logik aufrufen, falls vorhanden
        var enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.ResetEnemy();
            enemyScript.target = playerTransform; // Hier wird das Ziel gesetzt!
        }

        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}
