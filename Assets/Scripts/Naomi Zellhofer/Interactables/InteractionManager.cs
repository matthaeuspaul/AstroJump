using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3.0f; // Maximum distance for interaction
    public LayerMask interactableLayer; // Layer mask for interactable objects
    private IInteractable interactable; // Currently focused interactable object
    private Camera mainCamera; // Reference to the main camera

    public GameObject interactionPromptObject; // UI element to show interaction prompts
    private TMP_Text interactionText; // Text component for displaying prompts



    private void Start()
    {
       // interactionText = interactionPromptObject.GetComponentInChildren<TMP_Text>();
        Invoke("InitializePrompt", 1f); // invoke to prevent null reference on start
    }

    private void InitializePrompt()
    { 
        mainCamera = Camera.main; // Cache the main camera reference
        // changed to get the interaction prompt object from the PersistanceManager by Lucas Pietruschka
        if (PersistanceManager.instance != null)
        {
            interactionPromptObject = PersistanceManager.instance.interactionPromptObject;
            if (interactionPromptObject != null)
            {
                interactionText = interactionPromptObject.GetComponentInChildren<TMP_Text>();
            }
            else
            {
                Debug.LogError("Interaction prompt object is not assigned in PersistanceManager.");
            }
        }
        else
        {
            Debug.LogError("PersistanceManager instance not found.");
        }
    }

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
        //if (interactionPromptObject == null) return; // Ensure the prompt object is assigned
        //if (mainCamera == null) mainCamera = Camera.main; // Cache the main camera reference
        if (mainCamera == null || interactionPromptObject == null || interactionText == null) return; // Ensure everything is assigned

        // Cast a ray from the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Debug.DrawRay(ray.origin, ray.direction * interactionRange);
        RaycastHit hit;       
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red); // Visualize the ray in the editor
        // Perform the raycast
        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            // Check if the hit object has an IInteractable component
            interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactionText.text = interactable.GetInteractionPrompt();
                interactionPromptObject.SetActive(true);
                Debug.Log("Interactable object detected: " + hit.collider.name);
            }
        }
        else
        {
            interactable = null;
            interactionPromptObject.SetActive(false);
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