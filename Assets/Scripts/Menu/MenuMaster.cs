using UnityEngine;

public class MenuMaster : MonoBehaviour
{ 
    public void Settings()
    {
        // Implement settings functionality here
        Debug.Log("Settings button clicked");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the Unity Editor
#else
        Application.Quit(); // Quit the application
#endif
    }
}
