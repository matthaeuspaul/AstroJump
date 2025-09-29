using UnityEngine;

public class Initializer : MonoBehaviour
{

    public GameObject persistanceManagerPrefab;

    void Awake()
    {
        if (PersistanceManager.instance == null)
        {
            Instantiate(persistanceManagerPrefab);
        }
    }
}
