using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private GameObject _characterSelection;

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

    public void CharacterSelection()
    {
        _characterSelection.SetActive(true);
    }

    public void SelectMalePalayer()
    {
        PlayerPrefs.SetInt("SelectedPlayerGender", 1);
        PlayerPrefs.Save();
        Debug.Log("Männlicher Spieler ausgewählt");

        StartGame();

    }

    public void SelectFemalePlayer()
    {
        PlayerPrefs.SetInt("SelectedPlayerGender", 0);
        PlayerPrefs.Save();
        Debug.Log("Weiblicher Spieler ausgewählt");

        StartGame();
    }
}