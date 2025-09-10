using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

//<summary>
// this script manages level loading with a loading screen and video playback
//</summary>

public class LevelLoadingManagerer : MonoBehaviour
{
    public static LevelLoadingManagerer instance { get; private set; }

    [SerializeField] private SceneLoader sceneLoader; // reference to the scene loader script

    //<summary>
    // singleton pattern to ensure only one instance exists
    //</summary>
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //<summary>
    // starts the level transition process
    //</summary>
    public void StartLevelTransition(string sceneName)
    {
        // starts the transition routine
        StartCoroutine(TransitionRoutine(sceneName));
    }

    //<summary>
    // routine to handle the transition process
    //</summary>
    private System.Collections.IEnumerator TransitionRoutine(string sceneName)
    {

        SceneManager.LoadScene("LoadingScene");

        yield return new WaitForSeconds(1f);

        // find the videoplayer in the loading scene
        VideoPlayer videoPlayer = FindFirstObjectByType<VideoPlayer>();

        // start loading the new scene asynchronously
        StartCoroutine(sceneLoader.LoadSceneAsync(sceneName));

        // wait until the video is done playing and the scene is loaded
        while (videoPlayer.isPlaying || !sceneLoader.isLoadingDone)
        {
            // wait until the end of the frame
            yield return new WaitForEndOfFrame();
        }
        // activate the loaded scene
        StartCoroutine( sceneLoader.ActivateLoadedScene(sceneName));
    }
}