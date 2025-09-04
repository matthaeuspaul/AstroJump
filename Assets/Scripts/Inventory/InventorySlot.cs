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

    public void ClearItem(ItemData itemData)
    {
        slotIsOccupied = false;
        currentItem = null;
        itemImage.sprite = null;
    }
}