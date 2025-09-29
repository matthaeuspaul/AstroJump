using System;
using Unity.VisualScripting;
using UnityEngine;

public class PowerGenerator : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredItem; // Item required to activate the generator
    [SerializeField] private GameObject generatorObject; // The generator object to activate
    [SerializeField] private Material exitHologramMaterial; // Material to apply to the exit when activated

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
            Debug.Log("Power Generator activated with " + requiredItem.itemName + " at " + DateTime.Now);

            var InvMan = FindFirstObjectByType<InventoryManager>(); // Find the InventoryManager in the scene
            InvMan.ClearItem(); // Remove the used item from inventory
            // Activate the linked exit's trigger collider
            // this part is written by Lucas Pietruschka
            LinkItems link = GetComponent<LinkItems>();
            if (link != null && link.linkedPartner != null)
            {
                // get all colliders from linked partner
                Collider[] colliders = link.linkedPartner.GetComponents<Collider>();
                bool triggerFound = false;

                foreach (var col in colliders)
                {
                    // Enable the trigger collider to allow player to exit
                    if (col.isTrigger)
                    {
                        col.enabled = true;
                        Debug.Log("Exit active");
                        triggerFound = true;
                        break;
                    }
                }
                if (!triggerFound)
                {
                    Debug.LogWarning("Linked exit has no trigger Collider to activate");
                }
                var meshRenderer = link.linkedPartner.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    Material[] materials = meshRenderer.materials;
                    bool materialChanged = false;  
                    // swap the second Material "Barrier_Hologramm" based on name with "Exit_Hologramm"
                    // using exitHologramMaterial assigned in inspector for "Exit_Hologramm"
                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (materials[i].name.Contains("Barrier_Hologramm"))
                        {
                            materials[i] = exitHologramMaterial;
                            materialChanged = true;
                            Debug.Log("Exit hologram material applied");
                        }
                    }
                    if (materialChanged)
                    {
                        meshRenderer.materials = materials; // Apply the modified materials array back to the MeshRenderer
                    }
                    else
                    {
                        Debug.LogWarning("No 'Barrier_Hologramm' material found to replace on linked exit");
                    }
                }
                else
                {
                    Debug.LogWarning("Linked exit has no MeshRenderer to make visible");
                }
            }
            else
            {
                Debug.LogWarning("No linked partner found for generator");
            }
            // end of part written by Lucas Pietruschka
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
            return "Generator activated";

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