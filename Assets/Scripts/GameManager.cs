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
        private OverlaySystem overlaySystem;
        // TODO: When player / enemy dies, de-increment those
        public int alivePlayableCharacters;
        public int aliveEnemyCharacters;
        [HideInInspector]
        public int availableCharacters;

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
                break;
        }

        

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
            overlaySystem = FindObjectOfType<OverlaySystem>();
        
        }

    // Update is called once per frame
    void Update()
        {
            if(availableCharacters==0)
            {
                StartCoroutine(SwitchTurns());
                Debug.Log("Turn of" + currentTurn);
                foreach(GameObject c in overlaySystem.occupiedTiles.Keys)
            {
                Debug.Log(c.transform.name + " is in position" + overlaySystem.occupiedTiles[c].ToString());
            }
            }
        }

        void resetCharacters()
        {
                switch (currentTurn)
                {
            case TurnType.Player: { foreach (GameObject playerObject in playableCharacters) { playerObject.GetComponent<Player>().hasMoved = false; playerObject.GetComponent<Player>().hasAttacked = false; }; break; }
            
            case TurnType.Enemy: { foreach (GameObject enemyObject in enemyCharacters) { enemyObject.GetComponent<EnemyAI>().hasMoved = false; } break; }
        }
    }

    
    }

