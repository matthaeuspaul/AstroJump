using UnityEngine;

public class FloorRandomizer : MonoBehaviour
{
    /// <summary>
    /// FloorRandomizer:
    /// Activates one random floor prefab from an array of floor prefabs and deactivates the rest
    /// uses Unity's Random.Range to select a random index
    /// </summary>

    // create a public array of game objects called floorPrefabs
    public GameObject[] floorPrefabs;
    // choose a random floor prefab from the array to set it to active and deactivate the rest in the awake method
    private void Awake()
    {
        // Select a random index from the array
        int randomIndex = Random.Range(0, floorPrefabs.Length);
        for (int i = 0; i < floorPrefabs.Length; i++)
        {
            if (i == randomIndex)
            {
                // Activate the selected floor prefab and deactivate the others
                floorPrefabs[i].SetActive(true);
            }
            else
            {
                floorPrefabs[i].SetActive(false);
            }
        }
    }
}
