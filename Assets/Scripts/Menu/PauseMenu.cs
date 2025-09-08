using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Player _player; // Reference to the Player script for accessing player state

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
    public void LoadTitlescreen()
    {
        Destroy(GameObject.Find("PersistanceManager")); // Destroy the PersistanceManager to reset game state
        SceneManager.LoadScene("Titlescreen"); // Load the Title Screen scene
        Time.timeScale = 1f; // Ensure the game is unpaused
        
    }
}
