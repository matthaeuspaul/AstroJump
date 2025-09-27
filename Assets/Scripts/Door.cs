using System.Runtime.CompilerServices;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator doorAnimator;
    private bool isOpen = false;


    void Start()
    {
        LinkItems linkItems = GetComponent<LinkItems>();
        doorAnimator = GetComponent<Animator>();
        if (doorAnimator == null)
        {
            Debug.LogError("Animator component not found on the door.");
        }
    }
    public void ToggleDoor()
    {
    }
}
