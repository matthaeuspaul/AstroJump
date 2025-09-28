using UnityEngine;

public class TileInstance
{
    /// <summary>
    /// TileInstance:
    /// Information about a specific instance of a tile in the dungeon
    /// </summary>
    public TileData tileData; // Reference to the tile data
    public GameObject instance; // The actual instantiated GameObject
    public Vector2Int gridPosition; // Position in the dungeon grid
    public Vector3 worldPosition; // World position in the scene
    public string uniqueID; // Unique identifier for this tile instance
}