using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3.0f; // Maximum distance for interaction
    public LayerMask interactableLayer; // Layer mask for interactable objects
    private IInteractable interactable; // Currently focused interactable object
    private Camera mainCamera; // Reference to the main camera

    // Update is called once per frame
    void Update()
    {
        CheckForInteraction();
    }

    // <summary>
    // Check for interactable objects in front of the player
    // </summary>
    public void CheckForInteraction()
        {
        if (mainCamera == null) mainCamera = Camera.main; // Cache the main camera reference

        // Cast a ray from the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red); // Visualize the ray in the editor
        // Perform the raycast
        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            // Check if the hit object has an IInteractable component
            interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                Debug.Log("Interactable object detected: " + hit.collider.name);
            }
        }
        else
        {
            interactable = null;
        }
    }

    // <summary>
    // Handle interaction input
    // </summary>
    public void Interact(CallbackContext ctx)
    {
        if (ctx.performed && interactable != null) 
        { 
            interactable.Interact();
        }     
    }
}