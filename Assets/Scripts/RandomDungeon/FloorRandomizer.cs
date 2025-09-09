using UnityEngine;

public class FloorRandomizer : MonoBehaviour
{
    // create a public array of game objects called floorPrefabs
    public GameObject[] floorPrefabs;
    // choose a random floor prefab from the array to set it to active and deactivate the rest in the awake method
    private void Awake()
    {
        int randomIndex = Random.Range(0, floorPrefabs.Length);
        for (int i = 0; i < floorPrefabs.Length; i++)
        {
            if (i == randomIndex)
            {
                floorPrefabs[i].SetActive(true);
            }
            else
            {
                floorPrefabs[i].SetActive(false);
            }
        }
    }
}
