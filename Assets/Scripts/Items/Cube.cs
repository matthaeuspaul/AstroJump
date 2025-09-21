using UnityEngine;
using System;

public class Cube : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData ItemData; // Reference to the ItemData ScriptableObject
    private InventoryManager InventoryManager; // Reference to the InventoryManager

    private void Start()
    {   
        // Find the InventoryManager in the scene if not already assigned
        if (InventoryManager == null)
        {
            InventoryManager = FindFirstObjectByType<InventoryManager>();
        }
    }

    // This method is called when the player interacts with the "Cube"(wip_Item)
    public void Interact() 
    {
        // Add the item to the inventory
        InventoryManager.AddItem(ItemData);

        // Destroy "Cube"(wip_Item) in scene
        Destroy(gameObject);
    }


    // This method checks if the item can be interacted with
    public bool CanInteract()
    {
        // For simplicity, we always return true here
        return true;
    }
}