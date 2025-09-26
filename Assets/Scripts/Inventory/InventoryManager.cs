using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>(); // List of inventory slots
    public InventorySlot selectedSlot { get; private set; } // Currently selected slot
    private Transform cameraTransform => Camera.main.transform; // Reference to the main camera's transform
    private PlayerInput playerInput; // Reference to PlayerInput component

    private void Start()
    {
        SelectSlot(0); // Select the first slot by default
        //cameraTransform = Camera.main.transform; // Cache the main camera's transform
        Invoke("Subscribe", 0.2f);
    }

    // <summary>
    // Subscribe to input events
    // </summary>
    private void Subscribe()
    {
        
        if (playerInput == null)
        {
            playerInput = FindFirstObjectByType<PlayerInput>();
        }
        playerInput.actions["ItemSelectionSlot1"].performed += ctx => SelectSlot(0);
        playerInput.actions["ItemSelectionSlot2"].performed += ctx => SelectSlot(1);
        playerInput.actions["ItemSelectionSlot3"].performed += ctx => SelectSlot(2);
        playerInput.actions["ItemSelectionSlot4"].performed += ctx => SelectSlot(3);
        playerInput.actions["ItemSelectionSlot5"].performed += ctx => SelectSlot(4);
        playerInput.actions["ItemSelectionSlot6"].performed += ctx => SelectSlot(5);
        playerInput.actions["ItemSelectionSlot7"].performed += ctx => SelectSlot(6);
        playerInput.actions["DropItem"].performed += ctx => DropItem();
    }

    // <summary>
    // Unsubscribe from input events
    // </summary>
    private void Unsubscribe()
    {
        if (playerInput == null) return;
        playerInput.actions["ItemSelectionSlot1"].performed -= ctx => SelectSlot(0);
        playerInput.actions["ItemSelectionSlot2"].performed -= ctx => SelectSlot(1);
        playerInput.actions["ItemSelectionSlot3"].performed -= ctx => SelectSlot(2);
        playerInput.actions["ItemSelectionSlot4"].performed -= ctx => SelectSlot(3);
        playerInput.actions["ItemSelectionSlot5"].performed -= ctx => SelectSlot(4);
        playerInput.actions["ItemSelectionSlot6"].performed -= ctx => SelectSlot(5);
        playerInput.actions["ItemSelectionSlot7"].performed -= ctx => SelectSlot(6);
        playerInput.actions["DropItem"].performed -= ctx => DropItem();
    }

    // <summary>
    // Add item to inventory
    // </summary>
    public void AddItem(ItemData itemData)
    {
        foreach (var slot in inventorySlots)
        {
            if (!slot.slotIsOccupied) 
            {
                SpawnNewItem(itemData, slot); 
                break;
            }
        }

        Debug.Log($"Item {itemData.itemName} added to inventory.");
    }

    // <summary>
    // Spawn new item in the specified slot
    // </summary>
    void SpawnNewItem(ItemData itemData, InventorySlot slot)
    {
        slot.SpawnItem(itemData);
    }

    // <summary>
    // Select inventory slot by index
    // </summary>
    public void SelectSlot(int slotIndex)
    {
        selectedSlot?.DebugSelect(false); // deselect previous slot
        selectedSlot = inventorySlots[slotIndex];
        selectedSlot.DebugSelect(true); // select new slot
    }

    // <summary>
    // Use item from selected slot
    // </summary>
    public void UseSelectedItem() 
    {
        if (!selectedSlot.slotIsOccupied)
        {
            Debug.Log("No item in the selected slot to use.");
            return;
        }

        selectedSlot.UseItem();
    }

    // <summary>
    // Drop item from selected slot
    // </summary>
    public void DropItem()
    {
        if (!selectedSlot.slotIsOccupied)
        {
            Debug.Log("No item in the selected slot to drop.");
            return;
        }

        Instantiate(selectedSlot.currentItem.itemPrefab, cameraTransform.position + cameraTransform.forward * 3f, cameraTransform.rotation); 

        Debug.Log($"Item {selectedSlot.currentItem.itemName} dropped from inventory.");
        ClearItem();
    }

    // <summary>
    // Clear item from selected slot
    // </summary>
    public void ClearItem()
    {
        if (!selectedSlot.slotIsOccupied)
        {
            Debug.Log("No item in the selected slot to use.");
            return;
        }

        selectedSlot.ClearItem();
    }

    // <summary>
    // Cleanup on destroy
    // </summary>
    private void OnDestroy()
    {
        Unsubscribe();
    }
}