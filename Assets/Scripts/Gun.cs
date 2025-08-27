using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    LayerMask layerMask;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Default");
    }

    void FixedUpdate()
    {
        void Attack(CallbackContext ctx)
        {
            if (ctx.performed)
                Debug.Log("M1 pressed");
            {
                Shoot();
            }
        }
    }
    
    void Shoot() {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward , out hit, Mathf.Infinity, layerMask))

        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        /* if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        } */
    }
}
