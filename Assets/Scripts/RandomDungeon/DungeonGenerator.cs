using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    // Directions for tile connections
    enum Direction
    {
        up,
        down,
        left,
        right
    }

    [Header("Grid Settings")]
    [SerializeField] private int width = 100;   // Width of the grid
    [SerializeField] private int height = 100;  // Height of the grid
    [SerializeField] private int minMapSize = 20;

    [Header("Tile Settings")]
    [SerializeField] private TileData[] tileOptions; // Available tile types with prefab and connection data

    private TileInstance[,] tileGrid;      // Internal representation of the grid
    private bool[,] visited;       // Tracks visited cells during generation

    public void Start()
    {
        // Generate the level only on the server
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        int mapSize = 0;
        // Start recursive generation from middle
        do
        {
            // Initialize grid and visited arrays
            tileGrid = new TileInstance[width, height];
            visited = new bool[width, height];

            mapSize = RecursiveGenerate(width / 2, height / 2);
            Debug.Log($"Generatet {mapSize} tiles long map");
        }
        while (minMapSize >= mapSize);

        // Spawn the generated level
        SpawnLevel();
    }

    private int RecursiveGenerate(int x, int y)
    {
        int spawnedTiles = 0;
        // Mark the current cell as visited
        visited[x, y] = true;

        TileData chosenTile = ChooseTile(x, y); // Choose a valid tile for the current position
        if (chosenTile == null) 
            return 0; // No valid tile can be placed here, backtrack

        // Place the chosen tile in the grid
        tileGrid[x, y] = new TileInstance 
        {
            tileData = chosenTile, 
            gridPosition = new Vector2Int(x, y), 
            uniqueID = null // Assign unique ID later for traps or other interactive elements
        };
        spawnedTiles++; // Increment the count of spawned tiles

        List<Vector2Int> posibleDirections = new List<Vector2Int>(); // List to hold possible directions to explore
        posibleDirections = CheckPosibleWays(x, y, Direction.up, posibleDirections);
        posibleDirections = CheckPosibleWays(x, y, Direction.down, posibleDirections);
        posibleDirections = CheckPosibleWays(x, y, Direction.left, posibleDirections);
        posibleDirections = CheckPosibleWays(x, y, Direction.right, posibleDirections);

        // Randomly explore each possible direction
        while (posibleDirections.Count > 0)
        {
            var randomValue = Random.Range(0, posibleDirections.Count);
            var newPos = new Vector2Int(x, y) + posibleDirections[randomValue];
            posibleDirections.RemoveAt(randomValue);
           
            if (!visited[newPos.x, newPos.y]) // If the new position hasn't been visited, recurse into it
            {
                spawnedTiles += RecursiveGenerate(newPos.x, newPos.y);
            }
        }
        return spawnedTiles; // Return the total number of spawned tiles from this recursion
    }
    private TileData ChooseTile(int x, int y) // Choose a valid tile for the given position
    {
        List<TileData> spawnableObjects = new List<TileData>(); // List to hold valid tiles
        foreach (var item in tileOptions) // Check each tile option
        {
            if (IsTileValid(item, x, y))
            {
                spawnableObjects.Add(item); // Add valid tile to the list
            }
        }
        if (spawnableObjects.Count > 0) // If there are valid tiles, randomly select one
        {
            return spawnableObjects[Random.Range(0, spawnableObjects.Count)];
        }
        return null;
    }

    private bool IsTileValid(TileData tile, int x, int y)
    {

        // Direction to check around the tile
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        // Check each direction for valid connections
        foreach (var dir in directions)
        {
            int newX = x + dir.x; // Calculate new coordinates
            int newY = y + dir.y; // Calculate new coordinates

            if (!IsInBounds(newX, newY)) // If out of bounds
            {
                if (HasOpening(tile, dir)) // If the tile has an opening leading outside the grid
                {
                    return false; // Tile has an opening that leads outside the grid
                }
            }
            else if (tileGrid[newX, newY] != null) // If there's an existing tile in the neighboring cell
            {
                bool tileHasOpening = HasOpening(tile, dir); // Check if the current tile has an opening in this direction
                bool neighborHasOpening = HasOpening(tileGrid[newX, newY].tileData, -dir); // Check if the neighboring tile has a corresponding opening
                if (neighborHasOpening != tileHasOpening) // If openings don't match
                {
                    return false; // Mismatched openings between adjacent tiles
                }
            }
        }
        return true; // Tile is valid if all checks pass
    }

    private bool HasOpening(TileData tile, Vector2Int direction)
        {
        // Check if the tile has an opening in the specified direction
            if (direction == Vector2Int.up)
                return tile.topOpen;
            if (direction == Vector2Int.down)
                return tile.bottomOpen;
            if (direction == Vector2Int.left)
                return tile.leftOpen;
            if (direction == Vector2Int.right)
                return tile.rightOpen;
            return false;
        }
    private List<Vector2Int> CheckPosibleWays(int x, int y, Direction direction, List<Vector2Int> possibilities) 
    {
        // Check possible directions to move based on current tile's openings
        var newPos = new Vector2Int(x, y);

        switch (direction)
        {
            // Check each direction and see if it's valid to move there
            case Direction.up:

                newPos += Vector2Int.up;

                // Check if the new position is within bounds, not visited, and the current tile has an opening upwards
                if (IsInBounds(newPos.x, newPos.y) && !visited[newPos.x, newPos.y] && tileGrid[x,y]?.tileData.topOpen == true)
                {
                    possibilities.Add(Vector2Int.up);
                }
                break;
            case Direction.down:

                newPos += Vector2Int.down;

                if (IsInBounds(newPos.x, newPos.y) && !visited[newPos.x, newPos.y] && tileGrid[x, y]?.tileData.bottomOpen == true)
                {
                        possibilities.Add(Vector2Int.down);
                }
                break;

            case Direction.left:

                newPos += Vector2Int.left;

                if (IsInBounds(newPos.x, newPos.y) && !visited[newPos.x, newPos.y] && tileGrid[x, y]?.tileData.leftOpen == true)
                {
                        possibilities.Add(Vector2Int.left);
                }
                break;

            case Direction.right:

                newPos += Vector2Int.right;

                if (IsInBounds(newPos.x, newPos.y) && !visited[newPos.x, newPos.y] && tileGrid[x, y]?.tileData.rightOpen == true)
                {
                        possibilities.Add(Vector2Int.right);
                }
                break;
            default:
                break;
        }
        return possibilities; // Return the updated list of possible directions
    }

    private bool IsInBounds(int x, int y)
    {
        // Check if the coordinates are within the grid boundaries
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    private void SpawnLevel()
    {
        // Spawn all tiles based on the grid data
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileInstance tileInstance = tileGrid[x, y]; // Get the tile instance at the current grid position
                if (tileInstance != null && tileInstance.tileData != null) // If there's a tile to spawn
                {
                    Vector3 position = new Vector3(x, 0, y); // Calculate world position
                    tileInstance.worldPosition = position; // Store world position in the tile instance
                    // Instantiate the tile prefab at the calculated position
                    tileInstance.instance = Instantiate(tileInstance.tileData.prefab, position, Quaternion.identity, transform);
                    tileInstance.instance.name = $"Tile_{x}_{y}_{tileInstance.tileData.name}"; // Name the instance for easier identification (Ki)
                }
            }
        }
    }

    // ask ki to give me a methode to visualize the connections between tiles
    /* private void OnDrawGizmos()
     {
         if (tileGrid == null) return;

         for (int x = 0; x < width; x++)
         {
             for (int y = 0; y < height; y++)
             {
                 var tile = tileGrid[x, y];
                 if (tile != null)
                 {
                     Gizmos.color = Color.green;
                     Vector3 pos = new Vector3(x, 0, y);
                     Gizmos.DrawWireCube(pos, Vector3.one * 0.9f);

                     Gizmos.color = Color.yellow;
                     if (tile.tileData.topOpen)
                     {
                         Gizmos.DrawLine(pos, pos + Vector3.forward * 1f);
                     }
                     if (tile.tileData.bottomOpen)
                     {
                         Gizmos.DrawLine(pos, pos + Vector3.back * 1f);
                     }
                     if (tile.tileData.leftOpen)
                     {
                         Gizmos.DrawLine(pos, pos + Vector3.left * 1f);
                     }
                     if (tile.tileData.rightOpen)
                     {
                         Gizmos.DrawLine(pos, pos + Vector3.right * 1f);
                     }
                 }
             }
         }
     }  */
}