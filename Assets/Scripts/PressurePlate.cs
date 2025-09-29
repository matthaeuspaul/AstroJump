using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    /// <summary>
    /// PressurePlate:
    /// Activates when a player or item is on top of it
    /// Deactivates when no player or item is on top of it
    /// checks for colliders with tag "Player" or "Item"
    /// Links to another object via LinkItems script and activates its animator adn deactivates its colliders trigger
    /// if Gameobjects is destroyed while on plate it will be removed from the list and plate will deactivate if no other object is on it
    /// </summary>

    private LinkItems link; // Reference to LinkItems script
    private Animator pressureAnimator; // Reference to this object's Animator
    private bool isActivated = false; // State of the pressure plate
    private Animator pressureAnimatorTarget; // Reference to linked partner's Animator

    // Use a HashSet to track objects currently on the pressure plate
    private HashSet<Collider> objectInside = new HashSet<Collider>();  

    private void Start()
    {
        link = GetComponent<LinkItems>(); // Get the LinkItems component
        pressureAnimator = GetComponent<Animator>(); // Get this object's Animator component
        if (pressureAnimator == null)
        {
            Debug.LogError("No Animator component found on Pressure Plate object.");
        }
        // Get the Animator component from the linked partner
        if (link != null && link.linkedPartner != null)
        {
            pressureAnimatorTarget = link.linkedPartner.GetComponentInChildren<Animator>();
            if (pressureAnimatorTarget == null)
            {
                Debug.LogWarning("No Animator component found on linked partner.");
            }
        }
        else
        {
            Debug.LogWarning("No linked partner found for pressure plate.");
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is tagged as "Player" or "Item"
        if (other.CompareTag("PlayerFoot") || other.CompareTag("Item"))
        {
            objectInside.Add(other); // Add the object to the HashSet
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is tagged as "Player" or "Item"
        if (objectInside.Contains(other))
        {
           objectInside.Remove(other); // Remove the object from the HashSet
        }
    }
    private void Update()
    {
        objectInside.RemoveWhere(item => item == null); // Clean up any destroyed objects
        bool currentlyActive = objectInside.Count > 0; // Check if there are any objects on the plate
        // Only update state if it has changed
        if (currentlyActive != isActivated)
        {
            isActivated = currentlyActive; // Update the activation state
            pressureAnimator.SetBool("isActive", isActivated); // Update this object's animator
            //Debug.Log("Pressure Plate " + (isActivated ? "Activated" : "Deactivated"));
            if (pressureAnimatorTarget != null)
            {
                pressureAnimatorTarget.SetBool("isActive", isActivated);
                // deactivate linked objects collider trigger if deactivated
                Collider[] colliders = link.linkedPartner.GetComponentsInChildren<Collider>();
                foreach (Collider col in colliders)
                {
                    if (col.isTrigger)
                    {
                        col.enabled = !isActivated;
                    }
                }
            }
        }
    }
}