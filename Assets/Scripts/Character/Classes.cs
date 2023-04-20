using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classes : MonoBehaviour
{
    [HideInInspector]
    public int moveRange;
    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public int attackDamages;
    [HideInInspector]
    public int attackRange;

    private void Start()
    {
        //GetClassCharacteristics();
        //Debug.Log(this.transform.name + ": "+ " move range = "+moveRange+" move speed= "+moveSpeed+" health= "+health+" attack damages= "+attackDamages+" attack range ="+ attackRange);
    }

    public void GetClassCharacteristics(Player player)
    {
        Archer archer = player.GetComponent<Archer>();
        Swordsman swordsman = player.GetComponent<Swordsman>();
        Thief thief = player.GetComponent<Thief>();

        if (archer != null)
        {
            player.moveRange = archer.moveRange;
            player.moveSpeed = archer.moveSpeed;
            player.health = archer.health;
            player.maxHealth = player.health;
            player.attackDamages = archer.attackDamages;
            player.attackRange = archer.attackRange;
            player.Class = "Archer";
        }
        else if (swordsman!=null)
        {
            player.moveRange = swordsman.moveRange;
            player.moveSpeed = swordsman.moveSpeed;
            player.health = swordsman.health;
            player.maxHealth = player.health;
            player.attackRange = swordsman.attackRange;
            player.attackDamages = swordsman.attackDamages;
            player.Class = "Swordsman";
        }
        else if (thief!=null)
        {
            player.moveRange = thief.moveRange;
            player.moveSpeed = thief.moveSpeed;
            player.health = thief.health;
            player.maxHealth = player.health;
            player.attackRange = thief.attackRange;
            player.attackDamages = thief.attackDamages;
            player.Class = "Thief";
        }

    }
}
