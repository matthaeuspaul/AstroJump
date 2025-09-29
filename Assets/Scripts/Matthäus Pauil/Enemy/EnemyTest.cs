using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] private float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);
    }
}
