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
            // origimal made by Naomi
            // changed from this:
            /* SceneManager.LoadScene(nextScene);
               starts the level transition to the next scene
               LevelLoadingManagerer.instance.StartLevelTransition(nextScene);*/
            // to this:

            // changes by Lucas
            // now uses the LevelTracker to determine the next scene
            LevelTracker levelTracker = FindFirstObjectByType<LevelTracker>();

            // checks if Final Level is reached
            if (levelTracker.ReachedFinalLevel())
             {
                 Debug.Log("Reached final level, loading Endscreen");
                 return;
             }
            // gets the next scene name from LevelTracker
            string nextScene = levelTracker.NextSceneName();
            // starts the level transition to the next scene (coded by Naomi)
            LevelLoadingManagerer.instance.StartLevelTransition(nextScene);
            levelTracker.IncrementLevel();
        }
    }
}