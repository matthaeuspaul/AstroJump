using UnityEngine;

public interface IInteractable 
{
    void Interact();
    bool CanInteract();
    string GetInteractionPrompt() => "Press 'E' to interact"; // Default interaction prompt
}