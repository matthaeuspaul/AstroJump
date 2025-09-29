using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private AsyncOperation currentOperation; // current async operation for scene loading
    public bool isLoadingDone; // shows if loading is done

    //<summary>
    // scene is loaded asynchronously in additive mode
    //</summary>
    public IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoadingDone = false;
        currentOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); // scene is loaded additively
        currentOperation.allowSceneActivation = false; // hold activation until explicitly allowed

        // wait until the loading is almost done (90%)
        while (currentOperation.progress < 0.9f)
        {
            yield return new WaitForEndOfFrame();
        }

        isLoadingDone = true;
    }

    //<summary>
    // activates the loaded scene and unloads the previous one
    //</summary>
    public IEnumerator ActivateLoadedScene(string sceneName)
    {
        if (currentOperation != null)
        {            
            var prevScene = SceneManager.GetActiveScene(); // save the previous scene
            currentOperation.allowSceneActivation = true; // allow scene activation
            yield return new WaitForEndOfFrame(); // wait until the end of the frame
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName)); // set the new scene as active        
            currentOperation = null; // set current operation to null
            SceneManager.UnloadSceneAsync(prevScene); // unload the previous scene
        }
    }
}