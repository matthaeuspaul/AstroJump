using UnityEngine;

public class Mud : MonoBehaviour
{
    private Player player;

    [SerializeField] public float slwowFactor = 0.5f;
    [SerializeField] public float stickyFactor = 0.5f;
    public bool isMud = false;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerFoot"))
        {
            player._speed *= slwowFactor;
            player._jumpForce *= stickyFactor;
            isMud = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerFoot"))
        {
            player._speed /= slwowFactor;
            player._jumpForce /= stickyFactor;
            isMud = false;
        }
    }

    public bool IsMud()
    {
        return isMud;
    }
}
