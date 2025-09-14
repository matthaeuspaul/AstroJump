using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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
    [SerializeField] private float tileSize; // Size of each tile (for positioning)
    [SerializeField] private TileData[] tileOptions; // Available tile types with prefab and connection data

    private TileInstance[,] tileGrid;      // Internal representation of the grid
    private bool[,] visited;       // Tracks visited cells during generation

    [Header("Spawn and Exit Settings")]
    // never change minDistanceBetweenSpawnAndExit to a value higher than width or height (unity crash)
    [SerializeField] private int minDistanceBetweenSpawnAndExit = 20; // Minimum distance between spawn and exit points
    [SerializeField] private List<TileData> spawnTiles; // List of tiles suitable for spawn point
    [SerializeField] private List<TileData> exitTiles;  // List of tiles suitable for exit point
    private TileInstance spawnTile;    // TileInstance for the spawn point
    private TileInstance exitTile;     // TileInstance for the exit point

    [Header("Interactable Settings")]
    [SerializeField] private List<TileData> interactableTiles; // List of tiles suitable for interactive elements

    [Header("Wal List")]
    [SerializeField] private List<TileData> wallTiles; // List of wall tiles to block paths

    public void Start()
    {
        // Generate the level only on the server
        GenerateLevel();
        SelectSpawnAndExit();
        VisualizePath();
        PlaceInteractableElements();
        //FindMainPath(spawnTile, exitTile);

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
                for (int i = 0; i < item.spawnChanceTicket; i++) // Add the tile based on its spawn chance tickets
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

    private Vector2Int Negate(Vector2Int direction)
    {
        // Return the opposite direction
        return new Vector2Int(-direction.x, -direction.y);
    }

    private bool AreTilesConnected(TileData tileA, TileData tileB, Vector2Int direction)
    {
        return HasOpening(tileA, direction) && HasOpening(tileB, Negate(direction));
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
                if (IsInBounds(newPos.x, newPos.y) && !visited[newPos.x, newPos.y] && tileGrid[x, y]?.tileData.topOpen == true)
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
                    Vector3 position = new Vector3(x * tileSize, 0, y * tileSize); // Calculate world position
                    tileInstance.worldPosition = position; // Store world position in the tile instance
                    // Instantiate the tile prefab at the calculated position
                    tileInstance.instance = Instantiate(tileInstance.tileData.prefab, position, Quaternion.identity, transform);
                    tileInstance.instance.name = $"Tile_{x}_{y}_{tileInstance.tileData.name}"; // Name the instance for easier identification (Ki)
                }
            }
        }
    }
    private bool isTileEmpty(TileInstance tile)
    {
        var data = tile.tileData;

        return !data.hasDoor &&
                !data.hasGenerator &&
                !data.hasLever &&
                !data.hasLightActive &&
                !data.hasLightInactive &&
                !data.hasPortal &&
                !data.hasSpawnPoint &&
                !data.hasPressurePlate &&
                !data.hasTrap;
    }

    private TileData GetMatchingTile(TileData originalTile, List<TileData> tileList)
    {
        foreach (var tile in tileList)
        {
            if (tile.topOpen == originalTile.topOpen &&
                tile.bottomOpen == originalTile.bottomOpen &&
                tile.leftOpen == originalTile.leftOpen &&
                tile.rightOpen == originalTile.rightOpen)
            {
                return tile; // Return the first matching tile found
            }
        }
        return null; // No matching tile found
    }
    #region Spawn and Exit Selection
    private void SelectSpawnAndExit()
    {

        List<TileInstance> emptyTile = new List<TileInstance>();
        int attempts = 0;
        int maxAttempts = 100;

        // Collect all empty tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tile = tileGrid[x, y];
                if (tile != null && isTileEmpty(tile))
                {
                    emptyTile.Add(tile);
                }
            }
        }
        if (emptyTile.Count < 2)
        {
            Debug.LogWarning("Not enough empty tiles to place spawn and exit.");
            return;
        }
        // Randomly select spawn and exit tiles and ensuring minimum distance

        do
        {
            spawnTile = emptyTile[Random.Range(0, emptyTile.Count)];
            exitTile = emptyTile[Random.Range(0, emptyTile.Count)];
            attempts++;
        }
        while (Vector2Int.Distance(spawnTile.gridPosition, exitTile.gridPosition) < minDistanceBetweenSpawnAndExit && attempts < maxAttempts);

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Could not find suitable spawn and exit tiles with the required distance.");
        }

        // Replace spawn tile with a matching spawn tile from the spawnTiles list
        ReplaceWithMatchingTile(spawnTile, spawnTiles);
        // Replace exit tile with a matching exit tile from the exitTiles list
        ReplaceWithMatchingTile(exitTile, exitTiles);
    }

    private void ReplaceWithMatchingTile(TileInstance tileInstance, List<TileData> replaceList)
    {
        var matchingTile = GetMatchingTile(tileInstance.tileData, replaceList);
        if (matchingTile != null)
        {
            tileInstance.tileData = matchingTile; // Update the tile data to the matching tile
            if (tileInstance.instance != null)
            {
                Destroy(tileInstance.instance); // Remove the old instance
            }

            tileInstance.instance = Instantiate(tileInstance.tileData.prefab, tileInstance.worldPosition, Quaternion.identity, transform);
            tileInstance.instance.name = $"Tile_{tileInstance.gridPosition.x}_{tileInstance.gridPosition.y}_{tileInstance.tileData.name}";
        }
        else
        {
            Debug.LogWarning($"No matching tile found for {tileInstance.tileData.name}");
        }
    }

    #endregion

    #region A* Pathfinding
    /// <summary> Pathfinding with A* algorithm</summary>
    /// Find shortest path between two tiles using A* algorithm
    /// Each tile is a node, connections are based on open sides
    /// Movement cost between adjacent tiles is 1
    /// list of TileInstance to represent the path
    /// Placement of traps, levers, doors, etc. can be done after pathfinding

    // keep track of nodes to explore
    // keep track of explored nodes
    // track best path to each node
    private List<TileInstance> FindMainPath(TileInstance startTile, TileInstance endTile)
    {
        List<TileInstance> openList = new List<TileInstance>(); // Tiles to explore
        HashSet<TileInstance> closedSet = new HashSet<TileInstance>(); // Explored tiles
        Dictionary<TileInstance, TileInstance> cameFrom = new Dictionary<TileInstance, TileInstance>(); // Track best path
        Dictionary<TileInstance, int> gScore = new Dictionary<TileInstance, int>(); // Cost from start to current tile
        Dictionary<TileInstance, int> fScore = new Dictionary<TileInstance, int>(); // Estimated cost from start to end through current tile
        foreach (var tile in tileGrid) // Initialize scores
        {
            if (tile != null)
            {
                gScore[tile] = int.MaxValue;
                fScore[tile] = int.MaxValue;
            }
        }
        gScore[startTile] = 0; // Cost from start to start is 0
        fScore[startTile] = HeuristicCostEstimate(startTile, endTile); // Estimated cost from start to end
        openList.Add(startTile); // Start exploring from the start tile
        while (openList.Count > 0) // While there are tiles to explore
        {
            TileInstance current = GetLowestFScore(openList, fScore); // Get tile with lowest fScore
            if (current == endTile) // If we reached the end tile
            {
                return ReconstructPath(cameFrom, current); // Reconstruct and return the path
            }
            openList.Remove(current); // Remove current tile from open list
            closedSet.Add(current); // Add current tile to closed set
            foreach (var neighbor in GetNeighbors(current)) // Explore each neighbor of the current tile
            {
                if (closedSet.Contains(neighbor)) // If neighbor has already been explored
                    continue; // Skip it
                int tentativeGScore = gScore[current] + 1; // Tentative cost from start to neighbor
                if (!openList.Contains(neighbor)) // If neighbor is not in open list
                    openList.Add(neighbor); // Add it for exploration
                /*else if (tentativeGScore >= gScore[neighbor]) // If this path is not better
                    continue; // Skip it
                cameFrom[neighbor] = current; // Best path to neighbor is through current

            } */
                if (tentativeGScore < gScore[neighbor]) // If this path to neighbor is better
                {
                    cameFrom[neighbor] = current; // Best path to neighbor is through current
                    gScore[neighbor] = tentativeGScore; // Update gScore for neighbor
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, endTile); // Update fScore for neighbor

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor); // Add neighbor to open list if not already present
                    }
                }
            }
        }
        return null; // No path found   
    }

    private void VisualizePath()
    {         // Example usage of FindMainPath to visualize path between spawn and exit
        if (spawnTile != null && exitTile != null)
        {
            List<TileInstance> path = FindMainPath(spawnTile, exitTile);
            if (path != null)
            {
                foreach (var tile in path)
                {
                    if (tile.instance != null)
                    {
                        var renderer = tile.instance.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material.color = Color.blue; // Mark path tiles in blue
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("No path found between spawn and exit.");
            }
        }
    }

    private int HeuristicCostEstimate(TileInstance a, TileInstance b)
    {
        // Use Manhattan distance as heuristic
        return Mathf.Abs(a.gridPosition.x - b.gridPosition.x) + Mathf.Abs(a.gridPosition.y - b.gridPosition.y);
    }

    private TileInstance GetLowestFScore(List<TileInstance> openList, Dictionary<TileInstance, int> fScore)
    {
        TileInstance lowest = openList[0]; // Start with the first tile
        foreach (var tile in openList) // Check each tile in the open list
        {
            if (fScore[tile] < fScore[lowest]) // If this tile has a lower fScore
            {
                lowest = tile; // Update lowest
            }
        }
        return lowest; // Return the tile with the lowest fScore
    }

    private List<TileInstance> GetNeighbors(TileInstance tile)
    {
        List<TileInstance> neighbors = new List<TileInstance>(); // List to hold neighboring tiles

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        foreach (var dir in directions) // Check each direction
        {
            int newX = tile.gridPosition.x + dir.x; // Calculate neighbor's x coordinate
            int newY = tile.gridPosition.y + dir.y; // Calculate neighbor's y coordinate
            if (IsInBounds(newX, newY)) // If neighbor is within bounds
            {
                TileInstance neighbor = tileGrid[newX, newY]; // Get the neighboring tile
                if (neighbor != null && AreTilesConnected(tile.tileData, neighbor.tileData, dir)) // If tiles are connected
                {
                    neighbors.Add(neighbor); // Add to neighbors list
                }
            }
        }
        return neighbors; // Return the list of neighboring tiles
    }

    private List<TileInstance> ReconstructPath(Dictionary<TileInstance, TileInstance> cameFrom, TileInstance current)
    {
        List<TileInstance> totalPath = new List<TileInstance> { current }; // Start with the end tile
        while (cameFrom.ContainsKey(current)) // While there is a path back to the start
        {
            current = cameFrom[current]; // Move to the previous tile
            totalPath.Insert(0, current); // Insert it at the beginning of the path
        }
        return totalPath; // Return the reconstructed path
    }

    #endregion

    #region Interactive Elements Placement

    // After finding the main path, replace tiles with interactive elements tiles like traps, levers, doors, etc.
    // Use unique IDs to link elements like levers and doors
    // Ensure elements are placed logically (e.g., doors on walls, traps in open areas)
    // Prioritize placing elements on the main path for player interaction, but also consider side paths for added complexity
    // Ensure that the placement of interactive elements does not block the main path or make it impossible to navigate
    // Replace some floor tiles with wall tiles to prevent player from walking around traps or other elements
    private void PlaceInteractableElements()
    {

        List<TileInstance> mainPath = FindMainPath(spawnTile, exitTile);
        if (mainPath == null || mainPath.Count == 0)
        {
            Debug.LogWarning("No main path found for placing interactive elements.");
            return;
        }

        HashSet<TileInstance> occupiedTiles = new HashSet<TileInstance>(mainPath); // Track tiles already occupied by interactive elements
        System.Random rand = new System.Random(); // Use System.Random for better randomness control
        List<TileInstance> sidePath = new List<TileInstance>();

        foreach (var tile in tileGrid)
        {
            if (tile != null && !occupiedTiles.Contains(tile) && isTileEmpty(tile))
            {
                sidePath.Add(tile); // Collect side tiles that are empty and not occupied
            }
        }

        // Placement rule
        // Traps: Place on empty floor tiles, avoid walls and tiles adjacent to other interact
        // Levers: Place on tiles with walls, ensure they are not too close to other levers or doors
        // Doors: Place on wall tiles, ensure they connect to open paths
        // Generators: Place on walls, avoid clustering
        // Pressure Plates: Place on empty floor tiles, avoid clustering

        // After placing all interactable elements, pair activators and receivers
        PlaceActivatorReceiverInPairs(mainPath);
    }
    private void PlaceActivatorReceiverInPairs(List<TileInstance> mainPath)
    {
        // get all activators and receivers
        var activators = interactableTiles.FindAll(t => t.isActivator);
        var receivers = interactableTiles.FindAll(t => t.isReceiver);

        // Collect vaild tiles (only empty, has walls, no interactables nearby)
        List<TileInstance> usedTiles = new List<TileInstance>();
        int minPairs = 10; // Minimum number of pairs to place
        int maxPairs = 20; // Limit the number of pairs to avoid overpopulation
        int pairCount = Random.Range(minPairs, maxPairs + 1);

        Debug.Log($" Pair count to place: {pairCount}");

        int placed = 0;
        int maxAttempts = 100;

        while (placed < pairCount && maxAttempts-- > 0)
        {
            TileData activatorTileData = activators[Random.Range(0, activators.Count)];
            TileInstance activatorTile = FindMatchingMapTile(activatorTileData, usedTiles, mainPath);
            if (activatorTile == null)
                continue;
            TileData receiverTileData = receivers[Random.Range(0, receivers.Count)];
            TileInstance receiverTile = FindMatchingMapTile(receiverTileData, usedTiles, mainPath, avoidAdjacentTo: activatorTile);

            if (receiverTile == null)
                continue;
            // Assign unique ID to both tiles
            string uniqueID = System.Guid.NewGuid().ToString(); // Generate a unique ID for the pair
            activatorTile.uniqueID = uniqueID;
            receiverTile.uniqueID = uniqueID;

            // Replace tiles with matching activator and receiver tiles
            ReplaceTileWith(activatorTile, activatorTileData);
            ReplaceTileWith(receiverTile, receiverTileData);
            placed++;

            LinkItems activatorLink = activatorTile.instance.GetComponent<LinkItems>();
            LinkItems receiverLink = receiverTile.instance.GetComponent<LinkItems>();

            if (activatorLink != null && receiverLink != null)
            {
                activatorLink.linkedItemID = uniqueID;
                receiverLink.linkedItemID = uniqueID;

                activatorLink.linkedPartner = receiverLink;
                receiverLink.linkedPartner = activatorLink;
            }
            else
            {
                Debug.LogWarning("LinkItems component missing on activator or receiver prefab.");
            }

            usedTiles.Remove(activatorTile); // Remove activator from valid tiles to avoid re-selection
            usedTiles.Remove(receiverTile); // Remove receiver from valid tiles to avoid re-selection
        }
    }
    private bool AreTilesAdjacent(TileInstance a, TileInstance b)
    {
        int dx = Mathf.Abs(a.gridPosition.x - b.gridPosition.x);
        int dy = Mathf.Abs(a.gridPosition.y - b.gridPosition.y);
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }
    private bool IsEdgeTile(TileInstance tile)
    {
        int x = tile.gridPosition.x;
        int y = tile.gridPosition.y;
        return !IsInBounds(x - 1, y) || !IsInBounds(x + 1, y) || !IsInBounds(x, y - 1) || !IsInBounds(x, y + 1);
    }

    private bool IsNearInteractable(int x, int y)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in directions)
        {
            int checkX = x + dir.x;
            int checkY = y + dir.y;

            if (!IsInBounds(checkX, checkY)) continue;

            var neighbor = tileGrid[checkX, checkY];
            if (neighbor == null || neighbor.tileData == null) continue;

            var data = neighbor.tileData;

            if (data.hasDoor || data.hasLever || data.hasTrap
                || data.hasGenerator || data.hasPortal || data.hasSpawnPoint || data.hasPressurePlate)

            {
                return true; // Found an interactable element nearby
            }
        }
        return false; // No interactable elements nearby
    }
    private bool HasSameWalls(TileData a, TileData b)
    {
        return a.topOpen == b.topOpen &&
               a.bottomOpen == b.bottomOpen &&
               a.leftOpen == b.leftOpen &&
               a.rightOpen == b.rightOpen;
    }
    private void ReplaceTileWith(TileInstance tile, TileData data)
    {
        if (tile.instance != null)
            Destroy(tile.instance);

        tile.tileData = data;
        tile.instance = Instantiate(data.prefab, tile.worldPosition, Quaternion.identity, transform);
        tile.instance.name = $"Tile_{tile.gridPosition.x}_{tile.gridPosition.y}_{data.name}";
    }
    private TileInstance FindMatchingMapTile(TileData prefab, List<TileInstance> exclude, List<TileInstance> mainPath, TileInstance avoidAdjacentTo = null)
    {
        List<TileInstance> candidates = new List<TileInstance>();
        List<float> weights = new List<float>();

        bool isDoor = prefab.hasDoor; // Prevent doors from being placed on edge tiles

        foreach (var tile in tileGrid)
        {
            if (tile == null || tile.tileData == null || exclude.Contains(tile))
                continue;

            bool distanceIsAcceptable = true;
            if (avoidAdjacentTo != null)
            {
                int distance = HeuristicCostEstimate(tile, avoidAdjacentTo);
                if (distance < 5) // Minimum distance of 4 tiles
                {
                    distanceIsAcceptable = false;
                }
            }

            if (HasSameWalls(tile.tileData, prefab) &&
                isTileEmpty(tile) &&
                !IsNearInteractable(tile.gridPosition.x, tile.gridPosition.y) &&
                distanceIsAcceptable &&
                (!isDoor || !IsEdgeTile(tile)))
            {
                candidates.Add(tile);
                weights.Add(TileWeight(tile, mainPath));
            }
        }

        if (candidates.Count == 0)
            return null;

        float totalWeight = 0f;
        foreach (var weight in weights)
        {
            totalWeight += weight;
        }
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        for (int i = 0; i < candidates.Count; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue <= cumulativeWeight)
            {
                return candidates[i];
            }
        }

        return candidates[candidates.Count - 1];
    }
    private float TileWeight(TileInstance tile, List<TileInstance> mainPath)
    {
        // Higher weight for tiles closer to the main path
        float minDistance = float.MaxValue;
        foreach (var pathTile in mainPath)
        {
            float distance = Vector2Int.Distance(tile.gridPosition, pathTile.gridPosition);
            if (distance < minDistance)
                minDistance = distance;
        }
        float weight = Mathf.Max(0.3f, 1 - (minDistance / 10f)); // Weight decreases with distance, adjust divisor for range
        return weight;
    }
    #endregion

    #region Block of paths

    private void PlaceBlockingWalls()
    {
        // Block some paths by replacing certain floor tiles with wall tiles
        // Ensure Player cant walk around traps or other interactive elements
        // Make sure Player cant be softlocked because activator or receiver is blocked
        // block most of the open paths, but leave some open for navigation
        // look over the whole map and replace some floor tiles with wall tiles
        // dont replace randomly, but look for tiles that have multiple open sides
        foreach (var tile in tileGrid)
        {
            if (tile == null || tile.tileData == null || !isTileEmpty(tile))
                continue;
            int openSides = 0;
            if (tile.tileData.topOpen) openSides++;
            if (tile.tileData.bottomOpen) openSides++;
            if (tile.tileData.leftOpen) openSides++;
            if (tile.tileData.rightOpen) openSides++;
            // Only consider tiles with 3 or more open sides for wall placement
            if (openSides >= 3 && Random.value < 0.3f) // 30% chance to replace
            {
                TileData wallTile = GetMatchingTile(tile.tileData, wallTiles);
                if (wallTile != null)
                {
                    ReplaceTileWith(tile, wallTile);
                }
            }
        }
    }

    #endregion

}