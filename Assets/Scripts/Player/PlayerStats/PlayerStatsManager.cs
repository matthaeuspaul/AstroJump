using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager instance;

    [Header("Player Stats")]
    public float maxHealth = 100;
    public float currentHealth;
    public float maxOxygen = 100;
    public float currentOxygen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentOxygen = maxOxygen;
    }

    public bool Use(ItemData itemData)
    {
        if (itemData.itemType == ItemData.ItemType.Consumable)
        {
            switch (itemData.actionType)
            {
                case ItemData.ActionType.Heal: 
                    Health(20); // Heal 20 health points
                    Debug.Log($"Used {itemData.itemName}, healed 20 health points.");
                    return true;
                case ItemData.ActionType.Damage:
                    Health(-20); // Damage 20 health points
                    Debug.Log($"Used {itemData.itemName}, took 20 damage.");
                    return true;
                case ItemData.ActionType.FillOxygen:
                    FillOxygen(20); // Fill 20 oxygen points
                    Debug.Log($"Used {itemData.itemName}, filled 30 oxygen points.");
                    return true;
                default:
                    Debug.LogWarning($"Action type {itemData.actionType} not implemented.");
                    return false;
            }
        }
        else
        {
            Debug.LogWarning($"Item type {itemData.itemType} not usable.");
            return false;
        }
    }

    private void Health(float amount)
    {
        // if player consumes a healing item, health is increased by the specified amount
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player health changed by {amount} points.");

    }

    private void FillOxygen(float amount)
    {
        // if player collects an oxygen item, oxygen is increased by the specified amount
        currentOxygen += amount;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
        Debug.Log($"Player oxygen changed by {amount} points.");

    }

    public void CheckForDeath()
    {
        if (currentHealth <= 0 && currentOxygen <= 0 )
        {
            Debug.Log("Player has died.");
            // Implement death logic here (e.g., respawn, game over screen)
        }
    }
}