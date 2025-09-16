using UnityEngine;
using System;


public class Consumable : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData ItemData; // Reference to the ItemData ScriptableObject

    // This method is called when the player interacts with the wip_Item
    public void Interact() 
    {
        if(!PlayerStatsManager.instance.Use(ItemData)) return;
        // Destroy wip_Item in scene
        Destroy(gameObject);

        Debug.Log("Cube interacted with at " + DateTime.Now);
    }


    // This method checks if the item can be interacted with
    public bool CanInteract()
    {
        // For simplicity, we always return true here
        return true;
    }
}