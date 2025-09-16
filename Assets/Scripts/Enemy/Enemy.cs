using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private int maxHealth = 100;
    private int damage = 20;
    private int currentHealth;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public System.Action OnDeath;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        ResetEnemy();
    }

    private void Start()
    {
       agent = GetComponent<NavMeshAgent>();
       agent.destination = target.position;
    }

    private void Update()
    {
        if (target != null)
        {
            agent.destination = target.position;
        }
    }

    public void ResetEnemy()
    {
        currentHealth = maxHealth;
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void DealDamage(GameObject player)
    {
        var playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            // playerScript.TakeDamage(damage);
        }
    }

    public void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
}
