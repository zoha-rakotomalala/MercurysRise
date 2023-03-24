using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveRange = 3;
    public float moveSpeed = 3f;
    public bool hasMoved = false;
    private GameManager gameManager;
    [SerializeField] private OverlaySystem OverlaySystem;

    // Coroutine to move the player to the target position
    private IEnumerator MoveTo(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
    }

    // Coroutine to move the player along a path of tiles
    private IEnumerator MoveToTilePath(List<Vector3Int> path)
    {
        foreach (Vector3Int tile in path)
        {
            Vector3 targetPosition = new Vector3(tile.x + 0.5f, tile.y + 0.5f, transform.position.z);
            yield return StartCoroutine(MoveTo(targetPosition));
        }
    }

    // Move the player to the target tile position
    public void MoveToTile(Vector3Int targetTilePosition)
    {
        List<Vector3Int> path = OverlaySystem.Path(transform.position, targetTilePosition);
        StartCoroutine(MoveToTilePath(path));

    }

    // Returns a list of valid move locations for the player
    private List<Vector3Int> GetValidMoveLocations(Vector3Int currentPosition, int moveRange)
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

                if (OverlaySystem.IsValidMove(neighbor) && !visited.ContainsKey(neighbor) && neighborDistance <= moveRange)
                {
                    queue.Enqueue(neighbor);
                    visited[neighbor] = neighborDistance;
                    validMoveLocations.Add(neighbor);
                }
            }
        }

        return validMoveLocations;
    }

    public List<Vector3Int> GetValidMoveLocations()
    {
        return GetValidMoveLocations(Vector3Int.FloorToInt(transform.position), moveRange);
    }

    private void OnMouseDown()
    {
        Debug.Log("Player clicked");
        if (!hasMoved /*&& gameManager.currentTurn==GameManager.TurnType.Player*/)
        {
            OverlaySystem.ShowValidMoveLocations(this);
        }
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }
}
