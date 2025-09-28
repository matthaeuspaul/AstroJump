using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    /// <summary>
    /// DeathPlane:
    /// Moves player back to the spawn point in case he falls of the map
    /// Moves items back to the player position + offset in case they fall of the map
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Move Player back to spawn point
            Transform playerRoot = other.transform.root;
            playerRoot.position = DungeonGenerator.globalSpawnPosition;
            // other.transform.position = DungeonGenerator.globalSpawnPosition;

            // reset velocity of rigidbody

            Rigidbody rb = playerRoot.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.angularVelocity = Vector3.zero;
                rb.linearVelocity = Vector3.zero;
            }
        }
        if (other.CompareTag("Item"))
        {
            // Move Item back to Player position + offset
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 forwadOffset = player.transform.forward * 1.5f;
                // add offset in front of player and 1 unit up
                other.transform.position = player.transform.position + forwadOffset + new Vector3(0, 1, 0); 
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

}
