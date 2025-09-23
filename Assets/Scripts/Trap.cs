using UnityEngine;

public class Trap : MonoBehaviour
{
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
                Vector3 currentVelocity = rb.linearVelocity;
                Vector3 pushDirection = -currentVelocity.normalized; // Push in the opposite direction of current movement
                pushDirection.y = 0; // Keep the push direction horizontal
                rb.linearVelocity = Vector3.zero; // Reset current velocity
                rb.AddForce(pushDirection * 10f, ForceMode.VelocityChange); 
            }

        }
    }
}
