using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3.0f;
    public LayerMask interactableLayer;
    private IInteractable interactable;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteraction();
    }

    public void CheckForInteraction()
        {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red);
        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // Optionally, you can highlight the interactable object here
                Debug.Log("Interactable object detected: " + hit.collider.name);
            }
        }
        else
        {
            interactable = null;
        }
    }

    public void Interact(CallbackContext ctx)
        {
        if (ctx.performed) { }     
    }
}