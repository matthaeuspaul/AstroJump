using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    public void Attack(CallbackContext context)
    {
        Debug.Log("Attack called. Phase: {ctx.phase}");
        if (context.performed)
        {
            Debug.Log("Left mouse pressed");
            Shoot();
        }
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        Debug.Log("Shoot called");
        RaycastHit hit;
        Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
            Debug.Log($"Hit: {hit.transform.name}");

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                Debug.Log($"Target hit. Damage: {damage}");
                target.TakeDamage(damage);
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
            Debug.Log("No hit");
        }
    }
}
