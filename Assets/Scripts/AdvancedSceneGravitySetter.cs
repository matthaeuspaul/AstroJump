using UnityEngine;

public class AdvancedSceneGravitySetter : SceneGravitySetter
{
    [SerializeField]
    private Vector3 changedGravity = new Vector3(0, -19.62f, 0);
    private bool gravityChanged = false;

    public void ChangeGravity()
    {
        if (!gravityChanged)
        {
            Physics.gravity = changedGravity;
            gravityChanged = true;
        }
        else
        {
            Physics.gravity = gravity;
            gravityChanged = false;
        }
    }
}