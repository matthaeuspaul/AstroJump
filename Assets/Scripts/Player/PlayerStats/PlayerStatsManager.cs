using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager instance;

    [Header("Player Stats")]
    public float maxHealth = 100;
    public float currentHealth;
    public float maxOxygen = 100;
    public float currentOxygen;

    [Header("UI References")]
    public GameObject GOS; // Reference to the Game Over Screen
    [SerializeField] private GameObject UI;  // Reference to the main UI
    [SerializeField] private Image lifeBarFilled; // Reference to the health bar UI element
    [SerializeField] private Image oxygenBarFilled; // Reference to the oxygen bar UI element

    public bool isDead { get; private set; }
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
        GOS = FindObjectsInactiveByName("GameOverScreen");
        if (GOS == null) Debug.LogWarning("GameOverScreen reference not set in PlayerStatsManager.");
        else Debug.Log("GameOverScreen reference found.");
        currentHealth = maxHealth;
        currentOxygen = maxOxygen;

        Invoke("UpdateHealthUI", 1f);
        Invoke("UpdateOxygenUI", 1f);

    }

    private GameObject FindObjectsInactiveByName(string name)
    {
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.name == name && !obj.hideFlags.HasFlag(HideFlags.NotEditable) && !obj.hideFlags.HasFlag(HideFlags.HideAndDontSave))
            {
                return obj;
            }
        }
        return null;
    }

    private void Update()
    {
        UpdateOxygenUI();
        UpdateHealthUI();
    }

    public bool Use(ItemData itemData)
    {
        if (itemData.itemType == ItemData.ItemType.Consumable)
        {
            switch (itemData.actionType)
            {
                case ItemData.ActionType.Heal:
                    Health(20);
                    Debug.Log($"Used {itemData.itemName}, healed 20 health points.");
                    return true;
                case ItemData.ActionType.Damage:
                    Health(-20);
                    Debug.Log($"Used {itemData.itemName}, took 20 damage.");
                    return true;
                case ItemData.ActionType.FillOxygen:
                    FillOxygen(50);
                    Debug.Log($"Used {itemData.itemName}, filled 20 oxygen points.");
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
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player health changed by {amount} points.");
        UpdateHealthUI();
    }

    private void FillOxygen(float amount)
    {
        currentOxygen += amount;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
        Debug.Log($"Player oxygen changed by {amount} points.");
        UpdateOxygenUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player took {damage} damage. Health: {currentHealth / maxHealth}");
        UpdateHealthUI();
        CheckForDeath();
    }

    private void UpdateHealthUI()
    {
        if (lifeBarFilled != null)
            lifeBarFilled.fillAmount = currentHealth / maxHealth;
    }

    private void UpdateOxygenUI()
    {
        if (oxygenBarFilled != null)
            oxygenBarFilled.fillAmount = currentOxygen / maxOxygen;
    }

    public void CheckForDeath()
    {
        if (currentHealth <= 0)
        {
            // GameObject UI = GameObject.Find("UI");
            if(GOS == null)
            {
                GOS = FindObjectsInactiveByName("GameOverScreen");
                if (GOS == null) Debug.LogWarning("GameOverScreen reference not set in PlayerStatsManager.");
                else Debug.Log("GameOverScreen reference found.");
            }
            if (GOS != null) GOS.SetActive(true); else Debug.Log("GameOverScreen not found.");
            if (UI != null) UI.SetActive(false); else Debug.Log("UI not found.");

            Debug.Log("Player has died.");
            Player.Destroy(gameObject);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}
