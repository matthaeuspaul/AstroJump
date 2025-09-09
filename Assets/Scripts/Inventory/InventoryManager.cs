using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private InventorySlot selectedSlot;
    private Transform cameraTransform;
    private PlayerInput playerInput;

    private void Start()
    {
        SelectSlot(0) ;
        cameraTransform = Camera.main.transform;
        Subscribe();
    }

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

    private void Unsubscribe()
    {
        playerInput.actions["ItemSelectionSlot1"].performed -= ctx => SelectSlot(0);
        playerInput.actions["ItemSelectionSlot2"].performed -= ctx => SelectSlot(1);
        playerInput.actions["ItemSelectionSlot3"].performed -= ctx => SelectSlot(2);
        playerInput.actions["ItemSelectionSlot4"].performed -= ctx => SelectSlot(3);
        playerInput.actions["ItemSelectionSlot5"].performed -= ctx => SelectSlot(4);
        playerInput.actions["ItemSelectionSlot6"].performed -= ctx => SelectSlot(5);
        playerInput.actions["ItemSelectionSlot7"].performed -= ctx => SelectSlot(6);
        playerInput.actions["DropItem"].performed -= ctx => DropItem();


    }

    // Add item to inventory
    public void AddItem(ItemData itemData)
    {
        // find the first free slot
        foreach (var slot in inventorySlots)
        {
            // if slot is free, spawn item there
            if (!slot.slotIsOccupied) 
            {
                SpawnNewItem(itemData, slot); 
                break;
            }
        }

        Debug.Log($"Item {itemData.itemName} added to inventory.");
    }

    // Spawn item in slot
    void SpawnNewItem(ItemData itemData, InventorySlot slot)
    {
        // show the corresponding sprite in the slot
        slot.SpawnItem(itemData);
    }

    public void SelectSlot(int slotIndex)
    {
        selectedSlot?.DebugSelect(false); // deselect previous slot
        selectedSlot = inventorySlots[slotIndex];
        selectedSlot.DebugSelect(true); // select new slot
    }

    public void UseSelectedItem() // use item from inventory
    {
        if (!selectedSlot.slotIsOccupied)
        {
            Debug.Log("No item in the selected slot to use.");
            return;
        }

        selectedSlot.UseItem();
    }

    public void DropItem()
    {
        if (!selectedSlot.slotIsOccupied)
        {
            Debug.Log("No item in the selected slot to drop.");
            return;
        }

        // Here you would typically instantiate the item in the game world
        Instantiate(selectedSlot.currentItem.itemPrefab, cameraTransform.position + cameraTransform.forward * 3f, cameraTransform.rotation); 

        Debug.Log($"Item {selectedSlot.currentItem.itemName} dropped from inventory.");
        ClearItem();
    }

    private void ClearItem()
    {
        if (!selectedSlot.slotIsOccupied)
        {
            Debug.Log("No item in the selected slot to use.");
            return;
        }

        selectedSlot.ClearItem();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}