using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    private Player _player; // Reference to the Player script for accessing player state

    private void Awake()
    {
        _player = GetComponent<Player>();
        //_player = GameObject.FindWithTag("Player").GetComponent<Player>(); // Find the Player object and get its Player script component
    }

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