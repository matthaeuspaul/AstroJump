using UnityEngine;

[CreateAssetMenu(menuName = "Random Dungeon/Tile Data")]
public class TileData : ScriptableObject
{
    // Basic tile properties

    public GameObject prefab; // Prefab for the tile
    public bool topOpen;      // Is the top side open
    public bool bottomOpen;   // Is the bottom side open
    public bool leftOpen;     // Is the left side open
    public bool rightOpen;    // Is the right side open

    // Interactive elements

    public bool hasTrap;            // Does the tile have a trap
    public bool hasLever;           //  Does the tile have a lever
    public bool hasDoor;            // Does the tile have a door
    public bool hasPortal;          // Does the tile have a portal
    public bool hasGenerator;       // Does the tile have a generator
    public bool hasLightActive;     // Does the tile have an active light
    public bool hasLightInactive;   // Does the tile have an inactive light
}
