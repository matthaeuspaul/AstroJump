using UnityEngine;
using System;


public class Cube : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData ItemData;
    private InventoryManager InventoryManager;
    private void Start()
    {
        if (InventoryManager == null)
        {
            InventoryManager = FindFirstObjectByType<InventoryManager>();
        }
    }
    public void Interact() 
    { 
        InventoryManager.AddItem(ItemData);
        Debug.Log("Cube interacted with at " + DateTime.Now);
    }
        

    public bool CanInteract()
    {
        return true;
    }

}