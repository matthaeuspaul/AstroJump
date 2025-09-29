using UnityEngine;

public class Rotate : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(25f, 25f, 0f) * Time.deltaTime);
    }
}
