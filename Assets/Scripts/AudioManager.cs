using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance of the AudioManager
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Set the instance to this object if it doesn't exist
            DontDestroyOnLoad(gameObject); // Make this object persist across scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy this object if an instance already exists to enforce the singleton pattern
        }
    }


}
