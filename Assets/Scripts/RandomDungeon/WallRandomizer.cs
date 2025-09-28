using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WallRandomizer : MonoBehaviour
{
    /// <summary>
    /// wallRandomizer:
    /// Randomly activates one wall prefab from an array of wall prefabs and deactivates the rest
    /// uses ticket counts to influence spawn chance
    /// </summary>

    // create a array of wall prefabs to randomly choose from 
    public GameObject[] wallPrefabs;

    public int[] ticketCount;
    // On awak set a random wall prefab from the array active and deactivate the rest 
    protected virtual void Awake()
    {
        // Create a ticket pool based on the ticket counts
        List<int> ticketPool = new List<int>();
        for (int i = 0; i < ticketCount.Length; i++)
        {
            for (int j = 0; j < ticketCount[i]; j++)
            {
                ticketPool.Add(i);
            }
        }

        // Select a random ticket from the pool
        int randomTicket = ticketPool[Random.Range(0, ticketPool.Count)];

        for (int i = 0; i < wallPrefabs.Length; i++)
        {
            if (i == randomTicket)
            {
                // Activate the selected wall prefab and deactivate the others
                wallPrefabs[i].SetActive(true);
            }
            else
            {
                wallPrefabs[i].SetActive(false);
            }
        }
    }
}
