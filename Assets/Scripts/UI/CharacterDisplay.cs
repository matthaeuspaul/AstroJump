using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    [Header("Player Character Images")]
    public GameObject malePlayerSprite;
    public GameObject femalePlayerSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowSelectedPlayer();
    }

    private void ShowSelectedPlayer()
    {
        int selectedGender = PlayerPrefs.GetInt("SelectedPlayerGender", 1);

        // set both false
        if (malePlayerSprite != null)
        {
            malePlayerSprite.SetActive(false);
        }
        if (femalePlayerSprite != null)
        {
            femalePlayerSprite.SetActive(false);
        }

        if (selectedGender == 1 && malePlayerSprite != null)
        {
            malePlayerSprite.SetActive(true);
            Debug.Log("Männlicher Charakter wird angezeigt");
        }
        else if (selectedGender == 0 && femalePlayerSprite != null)
        {
            femalePlayerSprite.SetActive(true);
            Debug.Log("Weiblicher Charakter wird angezeigt");
        }
    }
}
