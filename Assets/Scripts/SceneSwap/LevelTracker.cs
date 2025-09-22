using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelTracker : MonoBehaviour
{
    [Header("List of SceneNames")]
    [SerializeField] private List<string> levelNames = new List<string>(); // List of level scene names

    private int levelIndex = 0;

    public void IncrementLevel()
    {
        levelIndex++;
    }
    public string NextSceneName()
    {
        if (levelIndex >= levelNames.Count)
        {
            return null ; // for now (not Endscreen implemented)
        }
        return levelNames[levelIndex]; // returns the next scene name
    }

    public bool ReachedFinalLevel()
    {
        return levelIndex >= levelNames.Count;
    }
}
