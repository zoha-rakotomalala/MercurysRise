using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameManager gameManager;
    private OverlaySystem overlaySystem;
    private Player[] playerCharacters;

    public int moveRange = 3;
    public float moveSpeed = 3f;
    public int health;
    public int attackDamages;
    public int attackRange = 1;
    [HideInInspector]
    public bool hasMoved = false;
    public bool isDead = false;

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(3f);
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        overlaySystem = FindObjectOfType<OverlaySystem>().GetComponent<OverlaySystem>();
        playerCharacters = FindObjectsOfType<Player>();
    }

    private void Update()
    {
        if (gameManager != null && gameManager.currentTurn == GameManager.TurnType.Enemy && !hasMoved)
        {
            StartCoroutine(MoveToClosestPlayer());
            hasMoved = true;
            //overlaySystem.occupiedTiles.(this.gameObject, this.transform.position);
            overlaySystem.occupiedTiles[this.gameObject] = Vector3Int.FloorToInt(this.transform.position);
            gameManager.availableCharacters--;
        }
    }

    private IEnumerator MoveToClosestPlayer()
    {
        Player closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (Player player in playerCharacters)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestPlayer = player;
                closestDistance = distance;
            }
        }

        if (closestPlayer != null)
        {
            Vector3Int targetTilePosition = overlaySystem.GetClosestValidMoveLocation(Vector3Int.FloorToInt(transform.position), Vector3Int.FloorToInt(closestPlayer.transform.position), moveRange);
            if (targetTilePosition != Vector3Int.FloorToInt(transform.position))
            {
                List<Vector3Int> path = overlaySystem.Path(transform.position, targetTilePosition);
                foreach (Vector3Int tile in path)
                {
                    Vector3 targetPosition = new Vector3(tile.x + 0.5f, tile.y + 0.5f, transform.position.z);
                    while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                        yield return null;
                    }
                }
                transform.position = new Vector3(targetTilePosition.x + 0.5f, targetTilePosition.y + 0.5f, transform.position.z);
            }
        }
    }
}
