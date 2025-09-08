using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public void AddItem(ItemData itemData)
    {
        // Finde den ersten freien Slot
        foreach (var slot in inventorySlots)
        {
            // Wenn der Slot frei ist, füge das Item hinzu
            if (!slot.slotIsOccupied) 
            {
                SpawnNewItem(itemData, slot); 
                break;
            }
        }

        Debug.Log($"Item {itemData.itemName} added to inventory.");
    }

    void SpawnNewItem(ItemData itemData, InventorySlot slot)
    {
        // Zeige das entsprechende Sprite im Slot
        slot.SpawnItem(itemData);
    }

    public void RemoveItem(ItemData itemData) // einfach weg machen
    {
        // Finde den Slot mit dem entsprechenden Item
        foreach (var slot in inventorySlots)
        {
            // Wenn der Slot das Item enthält, entferne es
            if (slot.slotIsOccupied && slot.currentItem == itemData)
            {
                ClearItem(itemData, slot);
                break;
            }
        }
    }

    public void UseItem(ItemData itemData) // aus inventar nehmen und benutzen
    {
        Debug.Log($"Item {itemData.itemName} used.");
    }

    void ClearItem(ItemData itemData, InventorySlot slot)
    {
        // Zeige das entsprechende Sprite im Slot
        slot.ClearItem(itemData);
    }
}
