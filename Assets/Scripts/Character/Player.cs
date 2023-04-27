using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public int moveRange;
    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public int health;
    [HideInInspector] 
    public int maxHealth;
    [HideInInspector]
    public int attackDamages;
    [HideInInspector]
    public int attackRange;
    private GameManager gameManager;
    public GameObject menu;
    [SerializeField] private OverlaySystem OverlaySystem;
    [HideInInspector]
    public bool hasMoved = false;
    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public bool hasAttacked = false;
    [HideInInspector]
    public bool selected = false;
    [HideInInspector]
    public string Class;
    [HideInInspector]
    public HoverShowStats hoveringSystem;
    public Sprite basicSprite;
    public Sprite selectedSprite;
    public Sprite finishedSprite;
    //public Animator animator;


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
    /*private IEnumerator MoveToTilePath(List<Vector3Int> path)
    {
        foreach (Vector3Int tile in path)
        {
            Vector3 targetPosition = new Vector3(tile.x + 0.5f, tile.y + 0.5f, transform.position.z);
            yield return StartCoroutine(MoveTo(targetPosition));
        }
    }*/
    private IEnumerator MoveToTilePath(List<Vector3Int> path)
    {
        Vector3Int currentPosition = Vector3Int.FloorToInt(transform.position);
        foreach (Vector3Int tile in path)
        {
            Vector3 targetPosition = new Vector3(tile.x + 0.5f, tile.y + 0.5f, transform.position.z);

            // Calculate the direction of the movement
            Vector3Int direction = Vector3Int.RoundToInt(targetPosition - transform.position);

            // Map the direction to one of the four directions (up, down, left, right)
            /*int x = 0, y = 0;
            if (direction.x > 0) x = 1;
            else if (direction.x < 0) x = -1;
            if (direction.y > 0) y = 1;
            else if (direction.y < 0) y = -1;*/

            // Set the X and Y parameters of the animator
            //animator.SetFloat("X", x);
            //animator.SetFloat("Y", y);

            yield return StartCoroutine(MoveTo(targetPosition));
        }
    }

    // Move the player to the target tile position
    public void MoveToTile(Vector3Int targetTilePosition)
    {
        List<Vector3Int> path = OverlaySystem.Path(transform.position, targetTilePosition);
        StartCoroutine(MoveToTilePath(path));
        hasMoved = true;
        //FindObjectOfType<GameManager>().availableCharacters--;
        menu.GetComponent<Menu>().displayMenu();

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
        selected = true;
        if ((!hasMoved || !hasAttacked) && gameManager.currentTurn == GameManager.TurnType.Player)
        {
            //OverlaySystem.ShowValidMoveLocations(this);
            if (menu.GetComponent<Menu>().targetedPlayer!= null && menu.GetComponent<Menu>().targetedPlayer.GetComponent<SpriteRenderer>().sprite != finishedSprite)
            {
                menu.GetComponent<Menu>().targetedPlayer.GetComponent<SpriteRenderer>().sprite = menu.GetComponent<Menu>().targetedPlayer.basicSprite;
            }
            menu.GetComponent<Menu>().targetedPlayer = this;
            this.GetComponent<SpriteRenderer>().sprite = selectedSprite;
            menu.GetComponent<Menu>().displayMenu();
            

        }
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        //menu = GameObject.FindGameObjectWithTag("Menu");
        Classes playerClass = this.GetComponent<Classes>();
        /*moveRange = playerClass.moveRange;
        moveSpeed = playerClass.moveSpeed;
        health = playerClass.health;
        attackDamages= playerClass.attackDamages;
        attackRange = playerClass.attackRange;*/
        playerClass.GetClassCharacteristics(this);
        Debug.Log(this.transform.name + ": " + " move range = " + moveRange + " move speed= " + moveSpeed + " health= " + health + " attack damages= " + attackDamages + " attack range =" + attackRange);
        hoveringSystem = this.GetComponent<HoverShowStats>();
        //animator = this.GetComponent<Animator>();
    }

    #region Health_functions
    public void updateHealth(int value)
    {
        health += value;
        Debug.Log("New Player health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameManager.alivePlayableCharacters--;
        Destroy(this.gameObject);
        Debug.Log("Player killed in action");
        OverlaySystem.occupiedTiles.Remove(this.gameObject);
        gameManager.players.Remove(this.gameObject);
    }
    #endregion

    #region Attack_functions
    public void Attack(GameObject enemy)
    {
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        enemyAI.updateHealth(-attackDamages);
        if (enemyAI.health > 0)
        {
            updateHealth(-enemyAI.attackDamages);
            Debug.Log("Damages received: " + enemyAI.attackDamages);
        }
        hasAttacked = true;
        FindObjectOfType<GameManager>().availableCharacters--;

    }
    #endregion
}
