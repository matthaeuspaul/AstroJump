using System;
using Unity.VisualScripting;
using UnityEngine;

public class PowerGenerator : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredItem; // Item required to activate the generator
    [SerializeField] private GameObject generatorObject; // The generator object to activate

    private bool isActivated = false;
    private bool showRequiredMessage = false;
    private float messageTimer = 0f;
    private float messageDuration = 2f;

    private void Update()
    {
        if (showRequiredMessage)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f)
            {
                showRequiredMessage = false;
            }
        }
    }

    //<summary>
    // This method is called when the player interacts with the Power Generator
    //</summary>
    public void Interact()
    {
        if (isActivated) return;

        // Check if the player has the required item
        if (CanInteract()) 
        {   
            isActivated = true; // activate only once
            generatorObject.SetActive(true); // Activate the generator object
///TODO: set exit zone active
            var InvMan = FindFirstObjectByType<InventoryManager>(); // Find the InventoryManager in the scene
            InvMan.ClearItem(); // Remove the used item from inventory
        }
        else
        {
            // Show required message temporarily
            showRequiredMessage = true;
            messageTimer = messageDuration;

            // Alternative: Direct UI access (if InteractionManager has public references)
            var interactionManager = FindFirstObjectByType<InteractionManager>();
            if (interactionManager != null)
            {
                // Access the text component directly
                var textComponent = interactionManager.GetComponentInChildren<TMPro.TMP_Text>();
                if (textComponent != null)
                {
                    textComponent.text = $"{requiredItem.itemName} required!";
                }
            }
        }
    }

    // <summary>
    // This method checks if the power generator can be interacted with
    // </summary>
    public bool CanInteract()
    {
        if (requiredItem == null) return true; // No item required, always interactable
        var InvMan = FindFirstObjectByType<InventoryManager>(); // Find the InventoryManager in the scene
        var selectedItem = InvMan.selectedSlot.currentItem; // Get the currently selected item in the inventory
        return selectedItem != null && selectedItem.itemName == requiredItem.itemName; // Check if the selected item matches the required item
    }

    public string GetInteractionPrompt()
    {
        // If generator is already activated
        if (isActivated)
            return "Generator activated.";

        // If showing required message temporarily (after failed interaction)
        if (showRequiredMessage && requiredItem != null)
            return $"{requiredItem.itemName} required!";

        // If no item is required
        if (requiredItem == null)
            return "Activate generator";

        // Check if player has the required item
        if (CanInteract())
        {
            return $"Insert {requiredItem.itemName} to activate power supply!";
        }
        else
        {
            return "Activate generator";
        }
    }
}