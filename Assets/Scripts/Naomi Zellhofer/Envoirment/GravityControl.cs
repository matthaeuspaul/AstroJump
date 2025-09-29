using UnityEngine;

public class GravityControl : MonoBehaviour
{
    // Gravity control script to change unity intern gravity strength
    [SerializeField] private float gravityScale = 1f;
    private void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f * gravityScale, 0);
    }

}
