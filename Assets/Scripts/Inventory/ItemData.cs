using UnityEngine;


[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite image;
    public bool isStackable;
    public int maxStackSize;

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