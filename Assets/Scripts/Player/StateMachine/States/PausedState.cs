using UnityEngine;
using UnityEngine.InputSystem;

public class PausedState : IPlayerState
{
    private Player _player;
    private GameObject _pauseMenu;
    private PlayerInput _playerInput;
    public PausedState(Player player, GameObject pauseMenu, PlayerInput playerInput)
    {
        _player = player;
        _pauseMenu = pauseMenu;
        _playerInput = playerInput;
    }
    public void Enter()
    {
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        _pauseMenu.SetActive(true); // Show the pause menu
        _playerInput.SwitchCurrentActionMap("UI"); // Switch to UI action map
        Debug.Log("Game Paused");
    }

    public void Exit()
    {
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        _pauseMenu.SetActive(false); // Hide the pause menu
        _playerInput.SwitchCurrentActionMap("Player"); // Switch back to Player action map
        Debug.Log("Game Resumed");
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
    }

    public void HandleAnimation() { /* No animations to handle in paused state */ }

}
