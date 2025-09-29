using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelTracker : MonoBehaviour
{
    /// <summary>
    /// LevelTracker:
    /// Tracks the current level index and provides the next scene name from a predefined list of level names.
    /// expendedable in the inspector
    /// </summary>

    [Header("List of SceneNames")]
    [SerializeField] private List<string> levelNames = new List<string>(); // List of level scene names

    private int levelIndex = 0; // Current level index

    public void IncrementLevel()
    {
        levelIndex++; // Increment the level index
    }
    public string NextSceneName()
    {
        if (levelIndex >= levelNames.Count)
        {
            return null ; // for now (no Endscreen implemented)
        }
        return levelNames[levelIndex]; // returns the next scene name
    }

    public bool ReachedFinalLevel()
    {
        return levelIndex >= levelNames.Count; // Check if the final level has been reached
    }


}
