using System.Runtime.CompilerServices;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 20f; // Force to push the player back
    PlayerStatsManager PlayerStatsManager;

    private void Start()
    {
        PlayerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
        if (PlayerStatsManager == null)
        {
            Debug.LogError("PlayerStatsManager not found in the scene. Please ensure it is present.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit the trap!");
            // Add logic to damage the player or trigger an effect
            // push player back to direction they came from
            Transform playerRoot = other.transform.root;

            Rigidbody rb = playerRoot.GetComponent<Rigidbody>();
            // Take Damage
            PlayerStatsManager.TakeDamage(5f);

            if (rb != null)
            {
                Vector3 pushDirection = (playerRoot.transform.position - transform.position).normalized;
                pushDirection.y = 0; // Keep the push direction horizontal
                rb.linearVelocity = Vector3.zero; // Reset current velocity
                rb.AddForce(pushDirection * knockbackForce, ForceMode.Impulse);

            }

        }
    }
}
