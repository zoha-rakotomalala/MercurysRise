using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    // Reference to the tilemaps for the level and obstacles
    public Tilemap tilemap;
    public Tilemap obstacleTilemap;

    // The current, target, and next tile positions for the character
    private Vector3Int currentTile;
    private Vector3Int targetTile;
    private Vector3Int nextTile;

    private void Start()
    {
        // Set the current tile position based on the character's starting position
        currentTile = tilemap.WorldToCell(transform.position);
    }

    private void Update()
    {
        // When the left mouse button is clicked, update the target tile position
        if (Input.GetMouseButtonDown(0))
        {
            targetTile = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            nextTile = GetNextTile(currentTile, targetTile);
        }

        // If the current tile position is not the same as the target tile position, move towards it
        if (currentTile != targetTile)
        {
            Vector3 targetPosition = tilemap.GetCellCenterWorld(nextTile);

            // If the next tile is not an obstacle tile, move towards it
            if (!obstacleTilemap.HasTile(obstacleTilemap.WorldToCell(targetPosition)))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
                if (transform.position == targetPosition)
                {
                    // If the character has reached the next tile, update the current and next tile positions
                    currentTile = nextTile;
                    nextTile = GetNextTile(currentTile, targetTile);
                }
            }
            // If the next tile is an obstacle tile, stop moving
            else
            {
                currentTile = tilemap.WorldToCell(transform.position);
                nextTile = currentTile;
            }
        }
    }

    // Get the next tile position in the path towards the target tile position
    private Vector3Int GetNextTile(Vector3Int currentTile, Vector3Int targetTile)
    {
        Vector3Int nextTile = currentTile;

        // Calculate the distance to the target tile
        int distanceToTarget = Mathf.Abs(currentTile.x - targetTile.x) + Mathf.Abs(currentTile.y - targetTile.y);

        // If the target tile is not the current tile, get the direction towards the target tile
        if (distanceToTarget > 0)
        {
            Vector3Int direction = new Vector3Int(
                Mathf.Clamp(targetTile.x - currentTile.x, -1, 1),
                Mathf.Clamp(targetTile.y - currentTile.y, -1, 1),
                0
            );
            nextTile += direction;
        }

        return nextTile;
    }
}