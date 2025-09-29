using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Switch:
    /// Activates or deactivates when interacted with
    /// Links to another object via LinkItems script and toggles its animator and colliders
    /// </summary>

    private Animator switchAnimator; // Reference to this object's Animator
    private bool isActive = false; // State of the switch
    private Animator switchAnimatorTarget; // Reference to linked partner's Animator

    private void Start()
    {
        switchAnimator = GetComponentInChildren<Animator>(); // Get this object's Animator component
        if (switchAnimator == null)
        {
            Debug.LogError("No Animator component found on Switch object.");
        }
    }
    public bool CanInteract()
    {
        return true; // always interactable
    }

    public void Interact()
    {
        LinkItems link = GetComponent<LinkItems>(); // Get the LinkItems component
        // get all colliders from linked partner except the root object's collider
        Collider[] colliders = link.linkedPartner.GetComponentsInChildren<Collider>();
        colliders = System.Array.FindAll(colliders, col => col.gameObject.transform != link.linkedPartner.transform); // Exclude root object's collider
        if (!isActive)
        {
            // activate switch and change animator state
            Debug.Log("Switch activated");
            isActive = true;
            switchAnimator.SetBool("isActive", true);
            if (link != null && link.linkedPartner != null)
            {
                // get the Animator component from the linked partner
                switchAnimatorTarget = link.linkedPartner.GetComponentInChildren<Animator>();
                if (switchAnimatorTarget != null)
                {
                    // toggle the linked partner's animator state
                    switchAnimatorTarget.SetBool("isActive", true);
                    Debug.Log("Linked object state toggled");
                    // deactivate all colliders on linked partner
                    foreach (Collider col in colliders)
                    {
                            col.enabled = false;
                    }
                }
                else
                {
                    Debug.LogWarning("No Animator component found on linked partner.");
                }
            }
            else
            {
                Debug.LogWarning("No linked partner found for switch.");
            }
        }
        else if (isActive)
        {
            // deactivate switch and change animator state
            Debug.Log("Switch deactivated");
            isActive = false;
            switchAnimator.SetBool("isActive", false);

            if (link != null && link.linkedPartner != null)
            {
                // get the Animator component from the linked partner
                switchAnimatorTarget = link.linkedPartner.GetComponentInChildren<Animator>();
                if (switchAnimatorTarget != null)
                {
                    // toggle the linked partner's animator state
                    switchAnimatorTarget.SetBool("isActive", false);
                    Debug.Log("Linked object state toggled");
                    // activate all colliders on linked partner
                    foreach (Collider col in colliders)
                    {   
                            col.enabled = true;
                    }
                }
                else
                {
                    Debug.LogWarning("No Animator component found on linked partner.");
                }
            }
            else
            {
                Debug.LogWarning("No linked partner found for switch.");
            }
        }
    }
}
