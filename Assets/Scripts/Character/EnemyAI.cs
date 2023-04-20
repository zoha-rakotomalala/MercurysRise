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
    [HideInInspector]
    public int maxHealth;
    public int attackDamages;
    public int attackRange = 1;
    [HideInInspector]
    public bool hasMoved = false;
    public bool isDead = false;
    public GameObject menu;
    private HoverShowStats hoveringSystem;

    private void Start()
    {
        maxHealth = health;
        hoveringSystem = this.GetComponent<HoverShowStats>();
        StartCoroutine(Init());
    }

    // When targeted for an attack
    private void OnMouseDown()
    {
        if (menu.GetComponent<Menu>().currentlyAttacking)
        {
            //menu.GetComponent<Menu>().enemy = this;
            Debug.Log("Enemy chosen");
            overlaySystem.overlayTilemap.ClearAllTiles();
            Player targetedPlayer = menu.GetComponent<Menu>().targetedPlayer;
            targetedPlayer.Attack(this.gameObject);
            targetedPlayer.hasMoved = true;

        }

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
                yield return new WaitForSeconds(1f);
                // Check if the closest player is within attack range
                float distanceToClosestPlayer = Vector3.Distance(transform.position, closestPlayer.transform.position);
                if (distanceToClosestPlayer <= attackRange)
                {
                    Attack(closestPlayer.gameObject);
                }
            }
        }
    }

    #region Health_functions
    public void updateHealth(int value)
    {
        health += value;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameManager.aliveEnemyCharacters--;
        Destroy(this.gameObject);
        Debug.Log("Enemy killed in action");
        overlaySystem.occupiedTiles.Remove(this.gameObject);
        gameManager.enemies.Remove(this.gameObject);
    }
    #endregion

    #region Attack_functions
    public void Attack(GameObject playerGO)
    {
        Player player = playerGO.GetComponent<Player>();
        player.updateHealth(-attackDamages);
        if (player.health > 0)
        {
            updateHealth(-player.attackDamages);
            Debug.Log("Damages received: " + player.attackDamages);
        }
        //FindObjectOfType<GameManager>().availableCharacters--;

    }
    #endregion
}
