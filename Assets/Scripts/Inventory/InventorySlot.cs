using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public bool slotIsOccupied = false;
    public int slotIndex;
    public ItemData currentItem;

    [SerializeField] Image itemImage;
    public void SpawnItem(ItemData itemData)
    {
        slotIsOccupied = true;
        currentItem = itemData;
        itemImage.sprite = itemData.image;
    }

    public void ClearItem()
    {
        slotIsOccupied = false;
        currentItem = null;
        itemImage.sprite = null;
    }

    public void UseItem() // use item from inventory
    {
        Debug.Log($"Item {currentItem.itemName} used.");
        ClearItem();
    }

    public void DebugSelect(bool isSelected)
    {
        if (isSelected)
            GetComponent<Image>().color = Color.yellow;
        else
            GetComponent<Image>().color = Color.white;
    }
}