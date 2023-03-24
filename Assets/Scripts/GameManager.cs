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
    public Player[] playableCharacters;
    public Player[] enemyCharacters; // To be modified to the enemy class
    public int alivePlayableCharacters;
    public int aliveEnemyCharacters;
    public int availableCharacters;

    private void SwitchTurns()
    {
        if (currentTurn == TurnType.Player)
        {
            currentTurn = TurnType.Enemy;
            // Call a method to handle the enemy turn
        }
        else
        {
            currentTurn = TurnType.Player;
            // Call a method to handle the player turn
        }
        Debug.Log("New turn: "+ currentTurn);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTurn = TurnType.Player;
        availableCharacters = alivePlayableCharacters;
    }



    // Update is called once per frame
    void Update()
    {
        if( availableCharacters==0)
        {
            SwitchTurns();

            switch(currentTurn)
            {
                case TurnType.Player: availableCharacters = alivePlayableCharacters; resetCharacters() ; break;
                case TurnType.Enemy: availableCharacters = aliveEnemyCharacters; resetCharacters(); break;
            }
        }
    }

    void resetCharacters()
    {
        switch(currentTurn)
        {
            case TurnType.Player: foreach(Player player in playableCharacters) { player.hasMoved = false; }; break;
            case TurnType.Enemy: foreach(Player player in enemyCharacters) { player.hasMoved = false; } break;
        }
    }
}
