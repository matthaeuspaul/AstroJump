using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitZone : MonoBehaviour
{
    //[SerializeField] private string nextScene;

    // detects when another collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // detects if the player enters the exit zone
       if (other.CompareTag("Player"))
        {
             //SceneManager.LoadScene(nextScene);
           // starts the level transition to the next scene
            //  LevelLoadingManagerer.instance.StartLevelTransition(nextScene);
            
            LevelTracker levelTracker = FindFirstObjectByType<LevelTracker>();


             if (levelTracker.ReachedFinalLevel())
             {
                 Debug.Log("Reached final level, loading Endscreen");
                 return;
             }

             string nextScene = levelTracker.NextSceneName();
            LevelLoadingManagerer.instance.StartLevelTransition(nextScene);
            levelTracker.IncrementLevel();
        }
    }
}