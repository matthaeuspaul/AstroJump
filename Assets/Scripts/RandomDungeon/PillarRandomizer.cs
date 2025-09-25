using System.Collections.Generic;
using UnityEngine;

public class PillarRandomizer : MonoBehaviour
{
    public GameObject[] pillarPrefabs;
    public int[] ticketCount;

    private void Awake()
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

        for (int i = 0; i < pillarPrefabs.Length; i++)
        {
            if (i == randomTicket)
            {
                pillarPrefabs[i].SetActive(true);
            }
            else
            {
                pillarPrefabs[i].SetActive(false);
            }
        }
    }
}
