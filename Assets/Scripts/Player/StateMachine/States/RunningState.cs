using UnityEngine;

public class RunningState : IPlayerState
{
    private Player _player; // Reference to the Player class

    public RunningState(Player player) // Constructor to initialize the state with a reference to the Player
    {
        _player = player; // Initialize the player reference
    }

    public void Enter()
    {
        // Set the player's speed to the running speed when entering the running state
        _player._speed = _player._runSpeed;
        Debug.Log("Entered Running State"); // Log message for debugging purposes
    }

    public void Exit()
    {
        // Nothing to clean up when exiting the walking state yet
        // for future use
    }

    public void FixedUpdate()
    {
        RunningMovement(); // Call the method to handle movement while running
    }

    public void Update()
    {
        // Check if the player is in the air
        if (!_player.isGrounded)
        {
            _player.TransitionToState(_player.airborneState);
        }
    }
    private void RunningMovement()
    {
        // Calculate the movement direction based on player input and camera orientation
        Vector3 movementDirection = _player.movementInput.x * _player.camTransform.right + _player.movementInput.y * Vector3.ProjectOnPlane(_player.camTransform.forward, Vector3.up).normalized;
        movementDirection.Normalize(); // Normalize the movement direction to ensure consistent speed in all directions
        Vector3 targetVelocity = movementDirection * _player._speed; // Calculate the target velocity based on movement direction and speed

        _player.rb.linearVelocity = new Vector3(targetVelocity.x, _player.rb.linearVelocity.y, targetVelocity.z); // Apply the target velocity while keeping the vertical component unchanged
    }
}
