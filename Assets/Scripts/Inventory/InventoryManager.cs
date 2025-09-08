using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private InventorySlot selectedSlot;
    private Transform cameraTransform;

    private void Start()
    {
        SelectSlot(0) ;
        cameraTransform = Camera.main.transform;
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
        if (selectedSlot.slotIsOccupied)
        {
            selectedSlot.UseItem();
        }
        else
        {
            Debug.Log("No item in the selected slot to use.");
        }
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
        if (selectedSlot.slotIsOccupied)
        {
            selectedSlot.ClearItem();
        }
        else
        {
            Debug.Log("No item in the selected slot to use.");
        }

    }
}
