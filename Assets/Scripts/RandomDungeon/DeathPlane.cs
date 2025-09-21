using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    /// <summary>
    ///     DeathPlane:
    ///     Moves player back to the spawn point in case he falls of the map
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            // Move Player back to spawn point
            other.transform.position = DungeonGenerator.globalSpawnPosition;

            // reset velocity of rigidbody

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.angularVelocity = Vector3.zero;
                rb.linearVelocity = Vector3.zero;
            }

        }
    }

}
