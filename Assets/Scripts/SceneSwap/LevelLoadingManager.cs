using UnityEngine;

// zentrales script, um neues level zu laden
// und den ladebildschirm zu verwalten

public class LevelLoadingManagerer : MonoBehaviour
{
    public static LevelLoadingManagerer instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextLevel()
    {
        // lade den ladebildschirm
        // lade das nächste level
        // entferne den ladebildschirm
        Debug.Log("Lade nächstes Level...");
    }
}