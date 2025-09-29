using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEditor.Experimental.GraphView;
using System.Runtime.InteropServices.WindowsRuntime;

public class Player : MonoBehaviour
{
    // Player components

    private Rigidbody _rb; // Reference to the Rigidbody component for physics-based movement
    private PlayerInput _playerinput; // Reference to the PlayerInput component for handling input actions
    private Vector2 _movementInput; // Input for player movement, using Vector2 for 2D input (x, y)
    private Transform _camTransform => Camera.main.transform; // Reference to the camera transform for movement direction
    private Animator animator; // Reference to the Animator component for handling animations
    private string currentAnimation = ""; // Current animation state
    [SerializeField] private Collider weaponCollider; // Reference to the weapon collider
    [SerializeField] GameObject _pauseMenu; // Reference to the pause menu GameObject
    /*[HideInInspector]*/ public float _speed; // Default walking speed
    public Gun gun; // Reference to the Gun component for shooting mechanic
    private bool _isGun = false;
    private bool _isSword = false;
    private bool _attacking = false;
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
    [SerializeField] private Transform groundCheck; // Position from where to check for ground
    [SerializeField] private float groundCheckRadius = 0.3f; // Radius for ground detection sphere
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
        // _camTransform = Camera.main.transform; // Get the main camera's transform for movement direction
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

        AssignGunReference();
    }

    // this method is written by Matthäus Paul
    private void AssignGunReference()
    {
        // Find all Gun components in the scene even if they are inactive
        Gun[] allGuns = Resources.FindObjectsOfTypeAll<Gun>();

        foreach (Gun g in allGuns)
        {
            // only assign the gun if parent is a CinemachineCamera

            if(g.transform.name == "Pistol" && g.transform.parent != null && g.transform.parent.GetComponent<Unity.Cinemachine.CinemachineCamera>() != null)
            {
                gun = g;
                break;
            }
        }
        if (gun == null)
        {
            Debug.LogWarning("Gun component not found in the scene. Please ensure there is a GameObject named 'Pistol' with a Gun component under a CinemachineCamera.");
        }
    }

    private void Update()
    {
        HandlePlayerRotation(); // added by Matthäus Paul

        // Ground dectection with SphereCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, Ground);
        // Debug visualization
        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckRadius, isGrounded ? Color.green : Color.red);

        _currentState?.Update();
    }

    private void FixedUpdate()
    {
        SpeedControl(); // Control the player's speed to not exceed the maximum speed
        _currentState?.FixedUpdate(); // Call the FixedUpdate method of the current state
    }

    // This Part is written by Matthäus Paul
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

    // End of Part written by Matthäus Paul

    public void TransitionToState(IPlayerState newState)
    {
        Debug.Log($"Transitioning from {_currentState?.GetType().Name} to {newState.GetType().Name}");

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

    // this part is written by Matthäus Paul
    public void EnableWeaponCollider()
    {
        // Enable the weapon collider during the attack animation
        weaponCollider.enabled = true;
        Debug.Log("Weapon collider enabled");
    }

    public void DisableWeaponCollider()
    {
        // Disable the weapon collider after the attack animation
        weaponCollider.enabled = false;
        Debug.Log("Weapon collider disabled");
    }
    // end of part written by Matthäus Paul

    public void Jump(CallbackContext ctx)
    {
        if (ctx.performed && isGrounded && !_attacking )
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            if (isRunning)
            {
                ChangeAnimation("Running_Jump", 0.1f); // added by Matthäus Paul
            }
            else
            {
                ChangeAnimation("Jumping", 0.1f); // added by Matthäus Paul
            }
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
        if (ctx.performed && !_attacking)
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
        Debug.Log($"Pause() called, ctx.phase={ctx.phase}, performed={ctx.performed}");
        PlayerStatsManager playerStats = FindFirstObjectByType<PlayerStatsManager>();
        playerStats.IsDead(); // Update the _isDead flag based on player's health
        if (ctx.performed)
        {
            if(playerStats.isDead) return; // Do not allow pausing if the player is dead
            // Prevent rapid toggling of pause state caused by the action Map switch and holding the escape Button
            if (Time.unscaledTime - _lastPause < pauseCooldown)
                // Debug.Log("Pause ignored due to cooldown");
                return;
                _lastPause = Time.unscaledTime;
            // Toggle between paused and previous gameplay state
            if (_currentState == pausedState)
            {
                Debug.Log("Resuming from paused state");
                TransitionToState(_previousGameplayState);
            }
            else
            {
                Debug.Log("Switching to paused state");
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
    // this part is written by Matthäus Paul
    public void Attack(CallbackContext ctx)
    {
        if (ctx.performed && !isRunning)
        {
            WeaponType weaponType = FindFirstObjectByType<WeaponType>();
            weaponType.IsWeaponActive();
            if (weaponType == null)  return;
            if(!weaponType.isPistolActive && !weaponType.isSwordActive) return;
            if (weaponType.IsWeaponActive() == weaponType.isPistolActive)
            {
                _isGun = true;
                _isSword = false;
            }
            else if (weaponType.IsWeaponActive() == weaponType.isSwordActive)
            {
                _isGun = false;
                _isSword = true;
            }
            if (_isGun && isGrounded && !isRunning)
            {
                _attacking = true;
                gun.Attack(ctx); // Call the Attack method of the Gun component when the attack input is performed
            }
            else if (_isSword /*&& isGrounded && !isRunning*/)
            {
                _attacking = true;
                Debug.Log("Sword attack animation not implemented yet");
                // Trigger attack animation
                // ChangeAnimation("Sword_Attack1", 0.1f);
            }
        }
        else if (ctx.canceled)
        {
            if (_isGun && isGrounded && !isRunning)
            {
                gun.Attack(ctx); // Stop the Attack method of the Gun component when the attack input is canceled
            }
            _attacking = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (PlayerStatsManager.instance != null)
        {
            PlayerStatsManager.instance.TakeDamage(damage);
        }
        else 
        {
            Debug.LogWarning("PlayerStatsManager instance is null");
        }
    }
    // end of part written by Matthäus Paul
}
