using UnityEngine;
using System;


public class Cube : MonoBehaviour, IInteractable
{
    public void Interact() 
    { 
        Debug.Log("Cube interacted with at " + DateTime.Now);
    }
        

    public bool CanInteract()
    {
        // Implementieren Sie hier die Logik, um zu prüfen, ob Interaktion möglich ist
        return true;
    }
}