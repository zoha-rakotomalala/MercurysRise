using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Classes : MonoBehaviour
{
    public int health;
    public int attackRange;
    public int attackDamages;
    public bool hasAttacked = false;

    public abstract void Attack(GameObject enemy);
    public abstract List<Vector3Int> GetValidAttackLocations(OverlaySystem overlaySystem);

    public void updateHealth(int healthModifier)
    {
        health += healthModifier;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}