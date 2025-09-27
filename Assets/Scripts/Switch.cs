using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    private Animator switchAnimator;
    private bool isActive = false;
    private Animator switchAnimatorTarget;

    private void Start()
    {
        switchAnimator = GetComponentInChildren<Animator>();
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
        LinkItems link = GetComponent<LinkItems>();
        Collider[] colliders = link.linkedPartner.GetComponentsInChildren<Collider>();
        colliders = System.Array.FindAll(colliders, col => col.gameObject.transform != link.linkedPartner.transform); // Exclude root object's collider
       // Collider[] colliders = link.linkedPartner.GetComponentsInChildren<Collider>();
        if (!isActive)
        {
            Debug.Log("Switch activated");
            isActive = true;
            switchAnimator.SetBool("isActive", true);
            if (link != null && link.linkedPartner != null)
            {
                switchAnimatorTarget = link.linkedPartner.GetComponentInChildren<Animator>();
                if (switchAnimatorTarget != null)
                {
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
            Debug.Log("Switch deactivated");
            isActive = false;
            switchAnimator.SetBool("isActive", false);

            if (link != null && link.linkedPartner != null)
            {
                switchAnimatorTarget = link.linkedPartner.GetComponentInChildren<Animator>();
                if (switchAnimatorTarget != null)
                {
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
