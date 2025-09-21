using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isActivated = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActivated = true;
            Debug.Log("Pressure Plate Activated");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActivated = false;
            Debug.Log("Pressure Plate Deactivated");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}