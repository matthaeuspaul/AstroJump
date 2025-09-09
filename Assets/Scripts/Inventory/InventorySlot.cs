using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public bool slotIsOccupied = false;
    public int slotIndex;
    public ItemData currentItem;

    [SerializeField] Image itemImage;

    // <summary>
    // Spawn item in the inventory slot
    // </summary>
    public void SpawnItem(ItemData itemData)
    {
        slotIsOccupied = true;
        currentItem = itemData;
        itemImage.sprite = itemData.image;
    }

    // <summary>
    // Clear item from the inventory slot
    // </summary>
    public void ClearItem()
    {
        slotIsOccupied = false;
        currentItem = null;
        itemImage.sprite = null;
    }

    // <summary>
    // Use item from the inventory slot
    // </summary>
    public void UseItem() // use item from inventory
    {
        Debug.Log($"Item {currentItem.itemName} used.");
        ClearItem();
    }

    // <summary>
    // Debug method to visually indicate selection state
    // </summary>
    public void DebugSelect(bool isSelected)
    {
        if (isSelected)
            GetComponent<Image>().color = Color.yellow;
        else
            GetComponent<Image>().color = Color.white;
    }
}