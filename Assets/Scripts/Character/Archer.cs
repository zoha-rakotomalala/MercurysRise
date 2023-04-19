using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    [HideInInspector]
    public int moveRange = 3;
    [HideInInspector]
    public float moveSpeed = 3f;
    [HideInInspector]
    public int health = 4;
    [HideInInspector]
    public int attackDamages = 3;
    
    public int attackRange = 3;
}
