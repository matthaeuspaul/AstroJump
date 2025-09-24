using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private float maxHealth = 100;
    [SerializeField] private float damage = 1;
    private float currentHealth;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public float attackRange = 3.5f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    public System.Action OnDeath;

    private void Awake()
    {
        // Set initial health and position
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

            // Check if player is in attack range
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                DealDamage(target.gameObject);
                lastAttackTime = Time.time;
            }
        }
    }

    public void ResetEnemy()
    {
        // Reset enemy health and position
        currentHealth = maxHealth;
        transform.position = startPosition;
        transform.rotation = startRotation;
        lastAttackTime = 0f;
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
            Debug.Log($"{name} deals {damage} damage.");
            playerScript.TakeDamage(damage);
        }
    }

    public void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}