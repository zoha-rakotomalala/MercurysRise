using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int moveRange = 3;
    public float moveSpeed = 3f;
    public int maxTilesPerMove = 1;
    [SerializeField] private OverlaySystem overlaySystem;
    [SerializeField] private Player targetPlayer;

    private void Start()
    {
        StartCoroutine(MoveTowardsPlayer());
    }

    private IEnumerator MoveTo(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
    }

    private IEnumerator MoveToTilePath(List<Vector3Int> path)
    {
        int pathLength = Mathf.Min(path.Count, maxTilesPerMove);

        for (int i = 0; i < pathLength; i++)
        {
            Vector3 targetPosition = new Vector3(path[i].x + 0.5f, path[i].y + 0.5f, transform.position.z);
            yield return StartCoroutine(MoveTo(targetPosition));
        }
    }

    private IEnumerator MoveTowardsPlayer()
    {
        while (true)
        {
            Vector3Int currentPosition = Vector3Int.FloorToInt(transform.position);
            Vector3Int targetPosition = Vector3Int.FloorToInt(targetPlayer.transform.position);
            List<Vector3Int> validMoveLocations = overlaySystem.GetValidMoveLocations(currentPosition, moveRange);

            if (validMoveLocations.Count > 0)
            {
                Vector3Int bestMove = currentPosition;
                float minDistance = float.MaxValue;

                foreach (Vector3Int move in validMoveLocations)
                {
                    float distance = Vector3Int.Distance(move, targetPosition);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        bestMove = move;
                    }
                }

                List<Vector3Int> path = overlaySystem.Path(transform.position, bestMove);
                yield return StartCoroutine(MoveToTilePath(path));
            }

            yield return new WaitForSeconds(1f);
        }
    }
}

