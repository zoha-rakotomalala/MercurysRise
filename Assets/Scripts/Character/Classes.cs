using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Classes : MonoBehaviour
{
    public int health;
    public int attackDamages;
    public int attackRange;
    public int moveRange;
    public float moveSpeed;

    public abstract void Attack(GameObject enemy);
}
