using UnityEngine;

public class ExitZone : MonoBehaviour
{
    [SerializeField] private string nextScene;

    // detects when another collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // detects if the player enters the exit zone
        if (other.CompareTag("Player"))
        {
            // starts the level transition to the next scene
            LevelLoadingManagerer.instance.StartLevelTransition(nextScene);
        }
    }
}