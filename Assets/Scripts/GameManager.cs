using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum TurnType
    {
        Player,
        Enemy
    }

    public TurnType currentTurn;
    public HashSet<GameObject> players;
    public HashSet<GameObject> enemies;
    private GameObject[] playableCharacters;
    private GameObject[] enemyCharacters;
    private OverlaySystem overlaySystem;
    public TMP_Text turn;
    // TODO: When player / enemy dies, de-increment those
    public int alivePlayableCharacters;
    public int aliveEnemyCharacters;
    [HideInInspector]
    public int availableCharacters;
    public GameObject win;
    public GameObject lose;

    IEnumerator SwitchTurns()
    {
        resetCharacters();

        switch (currentTurn)
        {
            case TurnType.Player:
                availableCharacters = aliveEnemyCharacters;
                yield return new WaitForSeconds(2f);
                currentTurn = TurnType.Enemy;
                break;

            case TurnType.Enemy:
                currentTurn = TurnType.Player;
                availableCharacters = alivePlayableCharacters;
                yield return new WaitForSeconds(2f);
                break;
        }
        turn.text = "Turn: " + currentTurn.ToString();


    }



    // Start is called before the first frame update
    void Start()
    {
        currentTurn = TurnType.Player;
        //playableCharacters = ;
        players = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        //enemyCharacters = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        availableCharacters = players.Count;
        alivePlayableCharacters = players.Count;
        aliveEnemyCharacters = enemies.Count;
        overlaySystem = FindObjectOfType<OverlaySystem>();
        turn.text = "Turn: " + currentTurn.ToString();

    }
    private IEnumerator ChangeScenes(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MoonMap");
    }

    // Update is called once per frame
    void Update()
    {
        if (availableCharacters == 0)
        {
            StartCoroutine(SwitchTurns());

        }
        if (aliveEnemyCharacters == 0)
        {
            Win();
            StartCoroutine(ChangeScenes(5));
        }
        if (alivePlayableCharacters == 0)
        {
            Lose();
        }
    }

    void resetCharacters()
    {
        switch (currentTurn)
        {
            case TurnType.Player: { foreach (GameObject playerObject in players) { playerObject.GetComponent<Player>().hasMoved = false; playerObject.GetComponent<Player>().hasAttacked = false; }; break; }

            case TurnType.Enemy: { foreach (GameObject enemyObject in enemies) { enemyObject.GetComponent<EnemyAI>().hasMoved = false; } break; }
        }
    }

    void Win()
    {
        win.SetActive(true);

    }

    void Lose()
    {
        lose.SetActive(true);
    }

}

