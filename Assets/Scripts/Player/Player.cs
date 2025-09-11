using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    // Player components

    private Rigidbody _rb; // Reference to the Rigidbody component for physics-based movement
    private PlayerInput _playerinput; // Reference to the PlayerInput component for handling input actions
    private Vector2 _movementInput; // Input for player movement, using Vector2 for 2D input (x, y)
    private Transform _camTransform; // Reference to the camera transform for movement direction
    private Animator animator; // Reference to the Animator component for handling animations
    private string currentAnimation = ""; // Current animation state
    [SerializeField] GameObject _pauseMenu; // Reference to the pause menu GameObject
    [HideInInspector] public float _speed; // Default walking speed
    public bool isGrounded { get; private set; } // Flag to check if the player is on the ground
    public bool isAirborne { get; private set; } // Flag to check if the player is in the air (not grounded)
    public bool isRunning { get; private set; } // Flag to check if the player is running

    private float _lastPause = -1f; // Timestamp of the last pause action to prevent rapid toggling
    private const float pauseCooldown = 0.2f; // Cooldown time between pause actions

    [Header("Movement")]
    [SerializeField] public float _walkSpeed; // Walking speed for the player
    [SerializeField] public float _runSpeed; // Running speed for the player

    [Header("Jumping")]
    [SerializeField] public float _jumpForce; // Jump force for the player
    [SerializeField] public float _airMultiplier; // Multiplier for air control, affects movement speed while in the air

    [Header("Ground Detection")]
    [SerializeField] private float height; // Height of the player for ground detection, used to determine how far down to check for ground
    [SerializeField] private LayerMask Ground; // LayerMask for ground detection

    //State Machine Stuff

    // Current state of the player
    private IPlayerState _currentState;
    private IPlayerState _previousGameplayState; // To store the previous state before pausing
    public WalkingState walkingState { get; private set; } 
    public RunningState runningState { get; private set; }
    public AirborneState airborneState { get; private set; }
    public PausedState pausedState { get; private set; }

    // Read only components for Player States to access
    public Vector2 movementInput => _movementInput;
    public Transform camTransform => _camTransform;
    public Rigidbody rb => _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player
        _playerinput = GetComponent<PlayerInput>(); // Get the PlayerInput component attached to the player
        _camTransform = Camera.main.transform; // Get the main camera's transform for movement direction
        // Initialize states
        walkingState = new WalkingState(this);
        runningState = new RunningState(this);
        airborneState = new AirborneState(this);
        pausedState = new PausedState(this, _pauseMenu, _playerinput);
        _currentState = walkingState; // Set the initial state to walking
        _currentState.Enter(); // Call the Enter method of the initial state
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen and hide it
    }
    private void Update()
    {
        HandlePlayerRotation(); // Handle mouse look for camera movement
        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, height / 2 + 0.1f, Ground);
        _currentState?.Update(); // Call the Update method of the current state

        CheckAnimation(); // Update the player's animation based on movement input
    }
    private void FixedUpdate()
    {
        SpeedControl(); // Control the player's speed to not exceed the maximum speed
        _currentState?.FixedUpdate(); // Call the FixedUpdate method of the current state
    }

    private void CheckAnimation()
    {
        if (_movementInput.y == 1)
            ChangeAnimation("Walking");
        else if (_movementInput.y == -1)
            ChangeAnimation("Walking_Backwards");
        else if (_movementInput.x == 1)
            ChangeAnimation("Walking_Right");
        else if (_movementInput.x == -1)
            ChangeAnimation("Walking_Left");
        else
            ChangeAnimation("Idle");
    }

    public void ChangeAnimation(string animation, float crossfade = 0.2f)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            animator.CrossFade(animation, crossfade);
        }
    }

    private void HandlePlayerRotation()
    {

        if (_camTransform != null)
        {
            // Rotate the player to face the same direction as the camera on the Y axis
            float cameraYRotation = _camTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, cameraYRotation, 0f);
        }
    }

    public void TransitionToState(IPlayerState newState)
    {
        if (_currentState != pausedState)
        {
            // Store the current state as the previous gameplay state before pausing
            _previousGameplayState = _currentState; 
        }
        // Transition to a new state
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z); // Get the horizontal velocity

        if (flatVel.magnitude > _speed) // If the horizontal velocity exceeds the speed limit
        {
            Vector3 limitedVel = flatVel.normalized * _speed; // Limit the velocity to the speed
            _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z); // Apply the limited velocity while keeping the vertical component
        }
    }

    public void PlayerMovement(CallbackContext ctx)
    {
        // Read the movement input from the input system
        _movementInput = ctx.ReadValue<Vector2>();
    }

    public void Jump(CallbackContext ctx)
    {
        // Jump only if the player is grounded
        if (ctx.performed && isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse); // Apply an upward force to the Rigidbody for jumping
            ChangeAnimation("Jumping", 0.1f); // Change to jumping animation
        }
    }
    public void Run(CallbackContext ctx)
    {
        // prevent switching states when in air
        if (!isGrounded)
        { 
            isRunning = ctx.performed;
            return;
        }
        // switch between walking and running states
        if (ctx.performed)
        {
            isRunning = true;
            TransitionToState(runningState);
        }
        else if (ctx.canceled)
        {
            isRunning = false;
            TransitionToState(walkingState);
        }
    }
    public void Pause(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Prevent rapid toggling of pause state caused by the action Map switch and holding the escape Button
            if (Time.unscaledTime - _lastPause < pauseCooldown)
                return;
            _lastPause = Time.unscaledTime;
            // Toggle between paused and previous gameplay state
            if (_currentState == pausedState)
            {
                TransitionToState(_previousGameplayState);
            }
            else
            {
                TransitionToState(pausedState);
            }
        }
    }
    public void Resume()
    {
        if (_currentState == pausedState)
        {
            TransitionToState(_previousGameplayState);
        }
    } 
}
