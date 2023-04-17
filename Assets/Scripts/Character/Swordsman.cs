using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Classes
{
    public override void Attack(GameObject enemy)
    {
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (Vector3Int.Distance(Vector3Int.FloorToInt(transform.position), Vector3Int.FloorToInt(enemy.transform.position)) <= attackRange)
        {
            enemyAI.updateHealth(-attackDamages);
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
            Vector3Int.down,
            Vector3Int.left + Vector3Int.up,
            Vector3Int.left + Vector3Int.down,
            Vector3Int.right + Vector3Int.up,
            Vector3Int.right + Vector3Int.down,
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int attackLocation = currentPosition + direction;

            if (overlaySystem.IsValidMove(attackLocation))
            {
                validAttackLocations.Add(attackLocation);
            }
        }

        return validAttackLocations;
    }
}