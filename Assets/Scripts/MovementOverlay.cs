
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementOverlay : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase availableTile;
    [SerializeField] private TileBase unavailableTile;

    private int movementRange = 3;

    private Vector3Int currentTile;
    private bool[,] availableTiles;
    public List<Transform> obstacles;


    private void Update()
    {
        // Get the current tile position of the player
        currentTile = tilemap.WorldToCell(transform.position);

        // Calculate available tiles within movement range
        availableTiles = new bool[movementRange * 2 + 1, movementRange * 2 + 1];
        for (int x = -movementRange; x <= movementRange; x++)
        {
            for (int y = -movementRange; y <= movementRange; y++)
            {
                Vector3Int tilePos = new Vector3Int(currentTile.x + x, currentTile.y + y, currentTile.z);
                if (tilemap.HasTile(tilePos))
                {
                    availableTiles[x + movementRange, y + movementRange] = true;
                }
            }
        }

        // Mark unavailable tiles
        foreach (Transform obstacle in obstacles)
        {
            Vector3Int obstacleTile = tilemap.WorldToCell(obstacle.position);
            int dx = Mathf.Abs(currentTile.x - obstacleTile.x);
            int dy = Mathf.Abs(currentTile.y - obstacleTile.y);
            if (dx + dy <= movementRange)
            {
                availableTiles[obstacleTile.x - currentTile.x + movementRange, obstacleTile.y - currentTile.y + movementRange] = false;
            }
        }

        // Update overlay
        for (int x = -movementRange; x <= movementRange; x++)
        {
            for (int y = -movementRange; y <= movementRange; y++)
            {
                Vector3Int tilePos = new Vector3Int(currentTile.x + x, currentTile.y + y, currentTile.z);
                if (tilemap.HasTile(tilePos))
                {
                    if (availableTiles[x + movementRange, y + movementRange])
                    {
                        tilemap.SetTile(tilePos, availableTile);
                    }
                    else
                    {
                        tilemap.SetTile(tilePos, unavailableTile);
                    }
                }
            }
        }
    }

}
