using System.Collections.Generic;
using UnityEngine;

public class AdvancedWall_Randomizer : WallRandomizer
{
    /// <summary>
    /// AdvancedWall_Randomizer:
    /// extendion of WallRandomizer to support two arrays of wall prefabs with ticket counts
    /// </summary>

    public GameObject[] wallPrefabs_2; // Second array of wall prefabs to choose from

    public int[] ticketCount_2; // Ticket counts for the second array of wall prefabs to influence spawn chance

    //  On awake set random wall prefab from first and second array active and deactivate the rest in both arrays

    protected override void Awake()
    {
        // First array
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
        // Second array
        // Create a ticket pool based on the ticket counts
        List<int> ticketPool_2 = new List<int>();
        for (int i = 0; i < ticketCount_2.Length; i++)
        {
            for (int j = 0; j < ticketCount_2[i]; j++)
            {
                ticketPool_2.Add(i);
            }
        }
        // Select a random ticket from the pool
        int randomTicket_2 = ticketPool_2[Random.Range(0, ticketPool_2.Count)];
        for (int i = 0; i < wallPrefabs_2.Length; i++)
        {
            // Activate the selected wall prefab and deactivate the others
            if (i == randomTicket_2)
            {
                wallPrefabs_2[i].SetActive(true);
            }
            else
            {
                wallPrefabs_2[i].SetActive(false);
            }
        }
    }

}
