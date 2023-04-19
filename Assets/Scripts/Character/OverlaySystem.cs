using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverlaySystem : MonoBehaviour
{
    public Tilemap overlayTilemap;
    public Tilemap obstaclesTilemap;
    public Tilemap enemiesTilemap;
    public Tile validMoveTile;
    public Tile validAttackTile;
    public Tile invalidMoveTile;
    private Player currentPlayer;
    [HideInInspector]
    public Dictionary<GameObject, Vector3> occupiedTiles = new Dictionary<GameObject, Vector3>();
    private GameObject[] playableCharacters;
    private GameObject[] enemyCharacters;

    private void Start()
    {
        overlayTilemap.ClearAllTiles();
        playableCharacters = GameObject.FindGameObjectsWithTag("Player");
        enemyCharacters = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject player in playableCharacters)
        {
            occupiedTiles.Add(player, player.GetComponent<Player>().transform.position);
        }
        foreach (GameObject enemy in enemyCharacters)
        {
            occupiedTiles.Add(enemy, enemy.GetComponent<EnemyAI>().transform.position);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentPlayer != null && !currentPlayer.hasMoved)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedTilePosition = overlayTilemap.WorldToCell(mouseWorldPosition);

            if (overlayTilemap.GetTile(clickedTilePosition) == validMoveTile)
            {
                currentPlayer.MoveToTile(clickedTilePosition);
                overlayTilemap.ClearAllTiles();
                currentPlayer = null;
            }
        }
        else if (Input.GetMouseButton(0) && currentPlayer != null && !currentPlayer.hasAttacked)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedTilePosition = overlayTilemap.WorldToCell(mouseWorldPosition);

            if (occupiedTiles.ContainsValue(clickedTilePosition))
            {
                overlayTilemap.ClearAllTiles();
            }

        }
    }


    public bool IsValidMove(Vector3Int position)
    {
        bool noObstacle = obstaclesTilemap == null || !obstaclesTilemap.HasTile(position);
        bool noEnemy = enemiesTilemap == null || !enemiesTilemap.HasTile(position);
        bool noOtherCharacter = !occupiedTiles.ContainsValue(position);

        return noObstacle && noEnemy && noOtherCharacter;
    }
    public Vector3Int GetClosestValidMoveLocation(Vector3Int startPosition, Vector3Int targetPosition, int moveRange)
    {
        List<Vector3Int> validMoveLocations = new List<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        Dictionary<Vector3Int, int> visited = new Dictionary<Vector3Int, int>();

        queue.Enqueue(startPosition);
        visited[startPosition] = 0;

        Vector3Int[] directions = new Vector3Int[]
        {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
        };

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();
            int currentDistance = visited[current];

            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighbor = current + direction;
                int neighborDistance = currentDistance + 1;

                if (IsValidMove(neighbor) && !visited.ContainsKey(neighbor) && neighborDistance <= moveRange)
                {
                    queue.Enqueue(neighbor);
                    visited[neighbor] = neighborDistance;
                    validMoveLocations.Add(neighbor);
                }
            }
        }

        Vector3Int closestValidMoveLocation = startPosition;
        float closestDistance = float.MaxValue;

        foreach (Vector3Int location in validMoveLocations)
        {
            float distance = Vector3Int.Distance(location, targetPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestValidMoveLocation = location;
            }
        }

        return closestValidMoveLocation;
    }

    public void ShowValidMoveLocations(Player player)
    {
        //Debug.Log("Showing valid move locations");
        overlayTilemap.ClearAllTiles();
        currentPlayer = player;

        List<Vector3Int> validMoveLocations = player.GetValidMoveLocations();
        Vector3Int currentPlayerTile = overlayTilemap.WorldToCell(player.transform.position);

        for (int x = -player.moveRange; x <= player.moveRange; x++)
        {
            for (int y = -player.moveRange; y <= player.moveRange; y++)
            {
                Vector3Int location = currentPlayerTile + new Vector3Int(x, y, 0);

                if (location == currentPlayerTile)
                {
                    continue;
                }

                if (validMoveLocations.Contains(location))

                {
                    overlayTilemap.SetTile(location, validMoveTile);
                    //Debug.Log("Setting valid move tile at: " + location);
                }
                else if (!IsValidMove(location))
                {
                    overlayTilemap.SetTile(location, invalidMoveTile);
                }
            }
        }
    }
    public List<Vector3Int> GetValidMoveLocations(Vector3Int currentPosition, int moveRange)
    {
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        Dictionary<Vector3Int, int> visited = new Dictionary<Vector3Int, int>();

        queue.Enqueue(currentPosition);
        visited[currentPosition] = 0;

        List<Vector3Int> validMoveLocations = new List<Vector3Int>();

        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();
            int currentDistance = visited[current];

            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighbor = current + direction;
                int neighborDistance = currentDistance + 1;

                if (IsValidMove(neighbor) && !visited.ContainsKey(neighbor) && neighborDistance <= moveRange)
                {
                    queue.Enqueue(neighbor);
                    visited[neighbor] = neighborDistance;
                    validMoveLocations.Add(neighbor);
                }
            }
        }

        return validMoveLocations;
    }

    // Generates a path from the player's current position to the target tile position
    public List<Vector3Int> Path(Vector3 startPos, Vector3Int targetTilePos)
    {
        Vector3Int startTilePos = overlayTilemap.WorldToCell(startPos);
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        queue.Enqueue(startTilePos);
        cameFrom[startTilePos] = startTilePos;

        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();

            if (current == targetTilePos)
            {
                break;
            }

            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighbor = current + direction;

                if (IsValidMove(neighbor) && !cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }
        // Construct the path by backtracking through the cameFrom dictionary
        List<Vector3Int> path = new List<Vector3Int>();

        if (cameFrom.ContainsKey(targetTilePos))
        {
            Vector3Int current = targetTilePos;

            while (current != startTilePos)
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Reverse();
        }
        return path;
    }

    public void ShowValidAttackLocations(Player player)
    {
        overlayTilemap.ClearAllTiles();
        currentPlayer = player;
        Vector3Int currentPlayerTile = overlayTilemap.WorldToCell(player.transform.position);
        Debug.Log("Player's attack range: " + player.attackRange);
        for (int x = -player.attackRange; x <= player.attackRange; x++)
        {
            for (int y = -player.attackRange; y <= player.attackRange; y++)
            {
                Vector3Int location = currentPlayerTile + new Vector3Int(x, y, 0);
                int distance = Mathf.Abs(x) + Mathf.Abs(y); // Manhattan distance

                if (location == currentPlayerTile || distance > player.attackRange)
                {
                    continue;
                }

                overlayTilemap.SetTile(location, validAttackTile);

            }
        }
    }

}
