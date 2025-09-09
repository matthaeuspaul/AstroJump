using UnityEngine;
using System;


public class Cube : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData ItemData;
    private InventoryManager InventoryManager;

    private void Start()
    {   // Find the InventoryManager in the scene if not already assigned
        if (InventoryManager == null)
        {
            InventoryManager = FindFirstObjectByType<InventoryManager>();
        }
    }

    // This method is called when the player interacts with the "Cube"(Item)
    public void Interact() 
    {
        // Add the item to the inventory
        InventoryManager.AddItem(ItemData);
        // Destroy cube object (item) in scene
        Destroy(gameObject);

        Debug.Log("Cube interacted with at " + DateTime.Now);
    }


    // This method checks if the item can be interacted with
    public bool CanInteract()
    {
        // For simplicity, we always return true here
        return true;
    }
}