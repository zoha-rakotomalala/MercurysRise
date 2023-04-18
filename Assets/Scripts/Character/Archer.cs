using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Classes
{
        public int rangedAttackRange = 3;

        private void Start()
        {
            attackRange = rangedAttackRange;
        }
    public override void Attack(GameObject enemy)
    {
        if (hasAttacked) return;

        OverlaySystem overlaySystem = FindObjectOfType<OverlaySystem>();
        Vector3Int enemyPosition = Vector3Int.FloorToInt(enemy.transform.position);
        List<Vector3Int> validAttackLocations = GetValidAttackLocations(overlaySystem);

        if (validAttackLocations.Contains(enemyPosition))
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            enemyAI.updateHealth(-attackDamages);
            hasAttacked = true;
            FindObjectOfType<GameManager>().availableCharacters--;
        }
    }



    public override List<Vector3Int> GetValidAttackLocations(OverlaySystem overlaySystem)
    {
        Vector3Int currentPosition = Vector3Int.FloorToInt(transform.position);
        List<Vector3Int> validAttackLocations = new List<Vector3Int>();

        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.left,
            Vector3Int.right,
            Vector3Int.up,
            Vector3Int.down
        };

        foreach (Vector3Int direction in directions)
        {
            for (int i = 1; i <= attackRange; i++)
            {
                Vector3Int attackLocation = currentPosition + (direction * i);

                if (overlaySystem.IsValidMove(attackLocation))
                {
                    validAttackLocations.Add(attackLocation);
                }
                else
                {
                    break;
                }
            }
        }

        return validAttackLocations;
    }
}
