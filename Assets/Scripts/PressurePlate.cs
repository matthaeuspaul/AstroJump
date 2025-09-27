using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private LinkItems link;
    private Animator pressureAnimator;
    private bool isActivated = false;
    private Animator pressureAnimatorTarget;

    private HashSet<Collider> objectInside = new HashSet<Collider>();  

    private void Start()
    {
        link = GetComponent<LinkItems>();
        pressureAnimator = GetComponent<Animator>();
        if (pressureAnimator == null)
        {
            Debug.LogError("No Animator component found on Pressure Plate object.");
        }
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
        if (other.CompareTag("Player") || other.CompareTag("Item"))
        {
            objectInside.Add(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (objectInside.Contains(other))
        {
           objectInside.Remove(other);
        }
    }
    private void Update()
    {
        objectInside.RemoveWhere(item => item == null); // Clean up any destroyed objects
        bool currentlyActive = objectInside.Count > 0;
        if (currentlyActive != isActivated)
        {
            isActivated = currentlyActive;
            pressureAnimator.SetBool("isActive", isActivated);
            Debug.Log("Pressure Plate " + (isActivated ? "Activated" : "Deactivated"));
            if (pressureAnimatorTarget != null)
            {
                pressureAnimatorTarget.SetBool("isActive", isActivated);
                // deactivate linked objects collider trigger if deactivated
                // deactivate all colliders on linked partner   
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