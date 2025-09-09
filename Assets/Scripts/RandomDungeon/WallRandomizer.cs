using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WallRandomizer : MonoBehaviour
{
    // create a array of wall prefabs to randomly choose from 
    public GameObject[] wallPrefabs;

    public int[] ticketCount;
    // On awak set a random wall prefab from the array active and deactivate the rest 
    public void Awake()
    {

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
    }
}
