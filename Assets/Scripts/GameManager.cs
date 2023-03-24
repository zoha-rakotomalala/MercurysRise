using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum TurnType
    {
        Player,
        Enemy
    }

    public TurnType currentTurn;
    private GameObject[] playableCharacters;
    private GameObject[] enemyCharacters;
    // TODO: When player / enemy dies, de-increment those
    public int alivePlayableCharacters;
    public int aliveEnemyCharacters;
    [HideInInspector]
    public int availableCharacters;

    private void SwitchTurns()
    {
        Debug.Log("Before:"+availableCharacters);
        switch (currentTurn)
        {
            case TurnType.Player: { currentTurn = TurnType.Enemy; availableCharacters = aliveEnemyCharacters; break; }
            case TurnType.Enemy: {currentTurn= TurnType.Player; availableCharacters = alivePlayableCharacters; break;}
        }
        Debug.Log("After:" + availableCharacters);
        resetCharacters();
        Debug.Log("New turn: "+ currentTurn);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTurn = TurnType.Player;
        playableCharacters = GameObject.FindGameObjectsWithTag("Player");
        enemyCharacters = GameObject.FindGameObjectsWithTag("Enemy");
        availableCharacters = playableCharacters.Length;
        alivePlayableCharacters = playableCharacters.Length;
        aliveEnemyCharacters = enemyCharacters.Length;
        
    }



    // Update is called once per frame
    void Update()
    {
        if(availableCharacters==0)
        {
            SwitchTurns();
        }
    }

    void resetCharacters()
    {
            switch (currentTurn)
            {
                case TurnType.Player: { foreach (GameObject playerObject in playableCharacters) { playerObject.GetComponent<Player>().hasMoved = false; Debug.Log("player reset"); }; break; }
                // TODO: Modify the enemyObject.GetComponent<Player> to enemyObject.GetComponent<Enemy> when Enemy instances are defined
                case TurnType.Enemy: { foreach (GameObject enemyObject in enemyCharacters) { enemyObject.GetComponent<Player>().hasMoved = false; Debug.Log("Enemy reset"); } break; }
            }
    }
}
