using System.Collections.Generic;
using UnityEngine;

public class PillarRandomizer : MonoBehaviour
{

    /// <summary>
    /// PillarRandomizer:
    /// Uses ticket counts to influence spawn chance
    /// selects one pillar prefab from an array of pillar prefabs to activate and deactivates the rest
    /// </summary>
    public GameObject[] pillarPrefabs; // array of pillar prefabs to choose from
    public int[] ticketCount; // ticket counts to influence spawn chance

    private void Awake()
    {
        // Create a ticket pool based on the ticket counts
        List<int> ticketPool = new List<int>();
        for (int i = 0; i < ticketCount.Length; i++)
        {
            for (int j = 0; j < ticketCount[i]; j++)
            {
                ticketPool.Add(i); // Add the index to the ticket pool based on its ticket count
            }
        }

        // Select a random ticket from the pool
        int randomTicket = ticketPool[Random.Range(0, ticketPool.Count)];

        for (int i = 0; i < pillarPrefabs.Length; i++)
        {
            if (i == randomTicket)
            {
                // Activate the selected pillar prefab and deactivate the others
                pillarPrefabs[i].SetActive(true);
            }
            else
            {
                pillarPrefabs[i].SetActive(false);
            }
        }
    }
}
