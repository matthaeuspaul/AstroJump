using System.Collections.Generic;
using UnityEngine;

public class AdvancedWall_Randomizer : WallRandomizer
{
    public GameObject[] wallPrefabs_2;

    public int[] ticketCount_2;

    //  On awake set random wall prefab from first and second array active and deactivate the rest in both arrays

    public void Awake()
    {
        // First array
        List<int> ticketPool = new List<int>();
        for (int i = 0; i < ticketCount.Length; i++)
        {
            for (int j = 0; j < ticketCount[i]; j++)
            {
                ticketPool.Add(i);
            }
        }
        int randomTicket = ticketPool[Random.Range(0, ticketPool.Count)];
        for (int i = 0; i < wallPrefabs.Length; i++)
        {
            if (i == randomTicket)
            {
                wallPrefabs[i].SetActive(true);
            }
            else
            {
                wallPrefabs[i].SetActive(false);
            }
        }
        // Second array
        List<int> ticketPool_2 = new List<int>();
        for (int i = 0; i < ticketCount_2.Length; i++)
        {
            for (int j = 0; j < ticketCount_2[i]; j++)
            {
                ticketPool_2.Add(i);
            }
        }
        int randomTicket_2 = ticketPool_2[Random.Range(0, ticketPool_2.Count)];
        for (int i = 0; i < wallPrefabs_2.Length; i++)
        {
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
