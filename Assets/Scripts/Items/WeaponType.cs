using Unity.Cinemachine;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    [SerializeField] private ItemData pistol;
    [SerializeField] private ItemData sword;
    public GameObject pistolPrefab;
    [SerializeField] private GameObject swordPrefab;

    public bool isPistolActive { get; private set; }
    public bool isSwordActive { get; private set; }

    private InventoryManager inventoryManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();

        // Find the pistolPrefab in the scene even if its inactive
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();

        foreach (Transform t in allTransforms)
            {
            if (t.name == "Pistol")
            {
                // Make sure it's a child of the CinemachineCamera
                if(t.parent != null && t.parent.GetComponent<Unity.Cinemachine.CinemachineCamera>() != null)
                {
                    pistolPrefab = t.gameObject;
                    break;
                }

            }
        }
        if (pistolPrefab == null)
        {
            Debug.LogError("Pistol prefab not found in the scene. Please ensure there is a GameObject named 'Pistol' under a CinemachineCamera.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var selectedItem = inventoryManager.selectedSlot.currentItem; // Get the currently selected item in the inventory
        if (selectedItem == null)
        {
            // Deactivate both weapons if no item is selected
            pistolPrefab.SetActive(false);
            swordPrefab.SetActive(false);
            isPistolActive = false;
            isSwordActive = false;
            Debug.Log("No weapon equipped");
            return;
        }
        if (selectedItem.itemType != ItemData.ItemType.Weapon)
        { 
            // Deactivate both weapons if the selected item is not a weapon
            pistolPrefab.SetActive(false);
            swordPrefab.SetActive(false);
            isPistolActive = false;
            isSwordActive = false;
            Debug.Log("No weapon equipped");
        }
        if (selectedItem == pistol && !isPistolActive)
        {
            // Activate pistol, deactivate sword
            pistolPrefab.SetActive(true);
            swordPrefab.SetActive(false);
            isPistolActive = true;
            isSwordActive = false;
            Debug.Log("Pistol equipped");
        }
        else if (selectedItem == sword && !isSwordActive)
        {
            // Activate sword, deactivate pistol
            swordPrefab.SetActive(true);
            pistolPrefab.SetActive(false);
            isSwordActive = true;
            isPistolActive = false;
            Debug.Log("Sword equipped");
        }
    }

    public bool IsWeaponActive()
    {
        return isPistolActive || isSwordActive;
    }
}
