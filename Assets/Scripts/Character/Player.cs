using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveRange = 3;
    public float moveSpeed = 3f;
    public int health;
    public int attackDamages;
    public int attackRange = 1;
    private GameManager gameManager;
    public GameObject menu;
    [SerializeField] private OverlaySystem OverlaySystem;
    [HideInInspector]
    public bool hasMoved = false;
    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public bool hasAttacked = false;
    // Coroutine to move the player to the target position
    private IEnumerator MoveTo(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        //OverlaySystem.occupiedTiles.TryAdd(this.gameObject, targetPosition);
        OverlaySystem.occupiedTiles[this.gameObject] = Vector3Int.FloorToInt(targetPosition);
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
        hasMoved = true;
        FindObjectOfType<GameManager>().availableCharacters--;

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
        // TODO: When Enemy instance created, remove the comment here to make sure Players can only move during their own turn
        Debug.Log("Player clicked");
        if (!hasMoved && gameManager.currentTurn==GameManager.TurnType.Player)
        {
            //OverlaySystem.ShowValidMoveLocations(this);
            menu.GetComponent<Menu>().displayMenu();
            menu.GetComponent<Menu>().targetedPlayer = this;
            
        }
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        //menu = GameObject.FindGameObjectWithTag("Menu");
    }

    #region Health_functions
    public void updateHealth(int value)
    {
        health += value;
        Debug.Log("New Player health: "+ health);
    }
    #endregion

    #region Attack_functions
    public void Attack(GameObject enemy)
    {
        EnemyAI enemyAI= enemy.GetComponent<EnemyAI>();
        enemyAI.updateHealth(-attackDamages);
        if (enemyAI.health > 0)
        {
            updateHealth(-enemyAI.attackDamages);
            Debug.Log("Damages received: " + enemyAI.attackDamages);
        }
        hasAttacked = true;
        FindObjectOfType<GameManager>().availableCharacters--;
        //TODO: When the player selects an enemy for the character to attack, we perform a battle, 
        //where the player first attacks the enemy and the enemy's health decreases by the attack damage of the character.
        //If the enemy survives, the player is also receiving damage from the enemy.
    }
    #endregion

    #region Menu
    public void displayMenu()
    {
        if (hasMoved)
        {
            return;
        }

        //TODO: When the player is clicked on and has not moved, we display a menu with three options "Move", "Attack" and "Wait".
        //If the character is next to an enemy, it can attack it, but if they attack before moving, they will not be able to move after, it will finish the turn.
       //If the player clicks on Move, we show
       // the possible places to move to, and they can move there. Once they have finished moving, the menu appears again if within attacking range of an enemy, and they can 
       // choose to attack or wait.
       // If they choose to wait, it just updates the "hasMoved" field to true, to show that the character has been done with.
    }
    #endregion
}
