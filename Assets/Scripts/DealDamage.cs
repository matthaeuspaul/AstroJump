using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            Target target = other.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}


