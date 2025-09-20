using UnityEngine;

[CreateAssetMenu(menuName = "Random Dungeon/Tile Data")]
public class TileData : ScriptableObject
{
    // Basic tile properties
    public int spawnChanceTicket = 1;        // Number of tickets for this tile
    public GameObject prefab; // Prefab for the tile
    public bool topOpen;      // Is the top side open
    public bool bottomOpen;   // Is the bottom side open
    public bool leftOpen;     // Is the left side open
    public bool rightOpen;    // Is the right side open

    // Interactive elements

    public bool hasTrap;            // Does the tile have a trap
    public bool hasSwitch;           //  Does the tile have a lever
    public bool hasDoor;            // Does the tile have a door
    public bool hasSpawnPoint;      // Does the tile have a spawn point
    public bool hasPortal;          // Does the tile have a portal
    public bool hasPressurePlate;   // Does the tile have a pressure plate
    public bool hasGenerator;       // Does the tile have a generator
    public bool hasLightActive;     // Does the tile have an active light
    public bool hasLightInactive;   // Does the tile have an inactive light

    // activator or receiver

    public bool isActivator;        // Is the tile an activator (e.g., lever, pressure plate)
    public bool isReceiver;         // Is the tile a receiver (e.g., door, trap)
}
