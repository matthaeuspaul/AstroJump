using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public bool slotIsOccupied = false; // Is the slot occupied
    public int slotIndex; // Index of the slot
    public ItemData currentItem; // Current item in the slot

    [SerializeField] Image itemImage; // UI Image to display the item
    [SerializeField] Sprite defaultSprite;

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
        itemImage.sprite = defaultSprite;
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