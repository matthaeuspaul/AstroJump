using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Movement : MonoBehaviour
{ 
    Rigidbody rb;
    private Vector2 _movementInput; // Input for player movement, using Vector2 for 2D input (x, y)
    private Transform camTransform; // Reference to the camera transform for movement direction
    private float _speed; // Default walking speed
    bool isGrounded; // Flag to check if the player is on the ground
    bool isAirborne; // Flag to check if the player is in the air (not grounded)

    [Header("Movement")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _runSpeed;

    [Header("Jumping")]
    [SerializeField] public float _jumpForce; // Jump force for the player
    [SerializeField] public float _airMultiplier;

    [Header("Ground Detection")]
    public float height; // Height of the player for ground detection, used to determine how far down to check for ground
    public LayerMask Ground; // LayerMask for ground detection

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen and hide it
        camTransform = Camera.main.transform; // Get the main camera's transform for movement direction
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player
        _speed = _movementSpeed; // Initialize walking speed

    }
    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, height / 2 + 0.1f, Ground);
        SpeedControl(); // Control the speed of the player
        if (isGrounded)
        {
            isAirborne = false; // Reset airbound state when grounded
        }
        else
        {
            isAirborne = true; // Set airbound state when not grounded
        }

    }
    private void FixedUpdate()
    {
            Vector3 movementDirection = _movementInput.x * camTransform.right + _movementInput.y * Vector3.ProjectOnPlane(camTransform.forward, Vector3.up).normalized;
            movementDirection.Normalize(); // Normalize the movement direction to ensure consistent speed in all directions
            Vector3 targetVelocity = movementDirection * _speed; // Calculate the target velocity based on movement direction and speed

            movementDirection = _movementInput.x * camTransform.right + _movementInput.y * Vector3.ProjectOnPlane(camTransform.forward, Vector3.up).normalized;
            if (isGrounded)
            {
               rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z); // Apply the target velocity while keeping the vertical component unchanged
            }
            else if (isAirborne)
            {
                Vector3 airVel = targetVelocity * _airMultiplier; // Calculate air velocity with multiplier
                rb.linearVelocity = new Vector3(airVel.x, rb.linearVelocity.y, airVel.z); // Apply the air velocity while keeping the vertical component unchanged
            }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Get the horizontal velocity

        if (flatVel.magnitude > _speed) // If the horizontal velocity exceeds the speed limit
        {
            Vector3 limitedVel = flatVel.normalized * _speed; // Limit the velocity to the speed
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z); // Apply the limited velocity while keeping the vertical component
        }
    }

    public void PlayerMovement(CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
    }

    public void Jump(CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
    public void Run(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _speed = _runSpeed; // Set movement speed to run speed
        }
        else if (ctx.canceled)
        {
            _speed = _movementSpeed; // Reset to normal movement speed
        }
    }
}
