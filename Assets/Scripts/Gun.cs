using UnityEngine;
using System.Collections;
using static UnityEngine.InputSystem.InputAction;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public Camera fpsCam;
    public Transform muzzlePoint;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    [Header("Tracer Settings")]
    [SerializeField] private GameObject tracerPrefab;
    [SerializeField] private float tracerDuration = 0.05f;

    private bool isShooting = false;
    private Coroutine fireCoroutine;

    public void Attack(CallbackContext context)
    {
        Debug.Log($"Attack called. Phase: {context.phase}");

        if (context.started || context.performed)
        {
            Debug.Log("Start continuous shooting");
            isShooting = true;

            // Stoppe vorherige Coroutine falls sie noch läuft
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }

            // Starte Coroutine für kontinuierliches Schießen
            fireCoroutine = StartCoroutine(ContinuousFire());
        }
        else if (context.canceled)
        {
            Debug.Log("Stop shooting");
            isShooting = false;

            // Stoppt die Coroutine
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }
    }

    private IEnumerator ContinuousFire()
    {
        while (isShooting)
        {
            Shoot();

            float waitTime = 1f / fireRate;
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        Debug.Log("Shoot called");

        RaycastHit hit;
        Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 targetPoint;
        Vector3 tracerStartPoint = GetMuzzlePosition();

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

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

            targetPoint = hit.point;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
            Debug.Log("No hit");
            targetPoint = ray.origin + ray.direction * range;
        }

        // Tracer von Mündung zum Zielpunkt spawnen
        SpawnTracer(tracerStartPoint, targetPoint);
    }

    private Vector3 GetMuzzlePosition()
    {
        if (muzzlePoint != null)
        {
            return muzzlePoint.position;
        }
        return fpsCam.transform.position + fpsCam.transform.forward * 0.5f;
    }

    private void SpawnTracer(Vector3 start, Vector3 end)
    {
        if (tracerPrefab != null)
        {
            GameObject tracer = Instantiate(tracerPrefab, start, Quaternion.identity);
            LineRenderer lr = tracer.GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.positionCount = 2;
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
            }

            Destroy(tracer, tracerDuration);
        }
    }
}
