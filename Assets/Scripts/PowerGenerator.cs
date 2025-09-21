using System;
using Unity.VisualScripting;
using UnityEngine;

public class PowerGenerator : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredItem; // Item required to activate the generator
    [SerializeField] private GameObject generatorObject; // The generator object to activate

    private bool isActivated = false;

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

            LinkItems link = GetComponent<LinkItems>();
            if (link != null && link.linkedPartner != null)
            {
                Collider[] colliders = link.linkedPartner.GetComponents<Collider>();
                bool triggerFound = false;

                foreach (var col in colliders)
                {
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
            }
            else
            {
                Debug.LogWarning("No linked partner found for generator");
            }

        }
        else
        {
            Debug.Log("Cannot interact with Power Generator. Requires EnergyOrb.");
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
}