using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Player _player; // Reference to the Player script for accessing player state

    public void ResumeGame()
    {
        _player.Resume(); // Call the Resume method on the Player script to unpause the game
    }
    public void Restart()
    {
        Destroy(GameObject.Find("PersistanceManager")); // Destroy the PersistanceManager to reset game state
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene to restart the game
        Time.timeScale = 1f; // Ensure the game is unpaused
    }
    public void Settings()
    {
        // Implement settings functionality here
        Debug.Log("Settings button clicked");
    }
    public void LoadTitlescreen()
    {
        Debug.Log("Loading Title Screen");
        /*
        SceneManager.LoadScene("TitleScreen"); // Load the Title Screen scene
        Time.timeScale = 1f; // Ensure the game is unpaused
        */
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the Unity Editor
#else
        Application.Quit(); // Quit the application
#endif
    }
}
