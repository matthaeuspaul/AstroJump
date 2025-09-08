using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    public void StartGame()
    { 
       SceneManager.LoadScene(_sceneName);
    }
    
    public void OpenCredits()
    {
       Debug.Log("Credits Menu Opened");
    }

    public void OpenControls()
    {
        Debug.Log("Controls Menu Opened");
    }
}
