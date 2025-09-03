using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    private bool slotIsOccupied = false;
    public void AddItem(ItemData itemData)
    {
        foreach (var slot in inventorySlots)
        {
            if (!slotIsOccupied) 
            {
                SpawnNewItem(itemData, slot);
                slotIsOccupied = true; 
                break;
            }
        }

        Debug.Log($"Item {itemData.itemName} added to inventory.");
    }

    void SpawnNewItem(ItemData itemData, InventorySlot slot)
    {
        // Zeige das entsprechende Sprite im Slot
        Debug.Log($"Spawning item {itemData.image } in slot {slot.slotIndex}");
    }
   
    public void RemoveItem(ItemData itemData) // einfach weg machen
    {
        Debug.Log($"Item {itemData.itemName} removed from inventory.");
    }

    public void UseItem(ItemData itemData) // aus inventar nehmen und benutzen
    {
        Debug.Log($"Item {itemData.itemName} used.");
    }
}
