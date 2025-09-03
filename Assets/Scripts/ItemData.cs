using UnityEngine;


[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    public bool isStackable;
    public int maxStackSize;

    public enum ItemType
    {
        Consumable,
        Useable,
        Weapon
    }
}