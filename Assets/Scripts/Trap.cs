using System.Runtime.CompilerServices;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 20f; // Force to push the player back

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit the trap!");
            // Add logic to damage the player or trigger an effect
            // push player back to direction they came from
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                pushDirection.y = 0; // Keep the push direction horizontal
                rb.linearVelocity = Vector3.zero; // Reset current velocity
                rb.AddForce(pushDirection * knockbackForce, ForceMode.Impulse);

            }

        }
    }
}
