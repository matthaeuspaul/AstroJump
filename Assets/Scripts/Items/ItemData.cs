using UnityEngine;

// ScriptableObject to hold item data
[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    // Item properties
    public string itemName;
    public ItemType itemType;
    public Sprite image;
    public bool isStackable;
    public int maxStackSize;
    public GameObject itemPrefab;

    public enum ItemType
    {
        Consumable,
        Tool,
        Weapon
    }

    public enum ActionType
    {
        Heal,
        Damage,
        Buff,
        Debuff
    }
}