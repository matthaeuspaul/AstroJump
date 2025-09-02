using UnityEngine;

public class SceneGravitySetter : MonoBehaviour
{
    [SerializeField]
    protected Vector3 gravity = new Vector3(0, -9.81f, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Physics.gravity = gravity;
    }
}