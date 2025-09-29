using UnityEngine;

public class PersistanceManager : MonoBehaviour
{
    public static PersistanceManager instance { get; private set; }

    public GameObject interactionPromptObject; // UI element to show interaction prompts    
    // Start is called before the first frame update
    void Start()
    {
        // Ensure that only one instance of PersistanceManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
