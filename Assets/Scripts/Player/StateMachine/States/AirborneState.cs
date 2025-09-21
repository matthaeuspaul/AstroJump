using UnityEngine;

public class AirborneState : IPlayerState
{
    private Player _player; // Reference to the Player class

    public AirborneState(Player player) // Constructor to initialize the state with a reference to the Player
    {
        _player = player; // Initialize the player reference
    }

    public void Enter()
    {
        Debug.Log("Entered Airborne State"); // Log message for debugging purposes
    }

    public void Exit()
    {
        // Nothing to clean up when exiting the walking state yet
        // for future use
    }

    public void FixedUpdate()
    {
        MoveInAir(); // Call the method to handle movement while in the air
    }

    public void Update()
    {
        // Check if the player has landed on the ground
        if (_player.isGrounded)
        {
            // Transition to the appropriate state based on whether the player is running or walking
            if (_player.isRunning)
            {
                _player.TransitionToState(_player.runningState);
            }
            else
            {
                _player.TransitionToState(_player.walkingState);
            }
        }
    }

    private void MoveInAir()
    {
        // Calculate the movement direction based on player input and camera orientation
        Vector3 movementDirection = _player.movementInput.x * _player.camTransform.right + _player.movementInput.y * Vector3.ProjectOnPlane(_player.camTransform.forward, Vector3.up).normalized;
        movementDirection.Normalize(); // Normalize the movement direction to ensure consistent speed in all directions
        Vector3 targetVelocity = movementDirection *(_player._speed * _player._airMultiplier);  // Calculate the target velocity based on movement direction and adjusted speed for air movement
        _player.rb.linearVelocity = new Vector3(targetVelocity.x, _player.rb.linearVelocity.y, targetVelocity.z);  // Apply the target velocity while keeping the vertical component unchanged
    }
}
