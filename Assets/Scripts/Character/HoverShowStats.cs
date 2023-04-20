using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverShowStats : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text classText;
    public GameObject StatsHolder;
    public void displayCharacterStatistics(GameObject character)
    {
        if (character.tag == "Player")
        {
            Player player = character.GetComponent<Player>();
            healthText.text = "Health: " + player.health + " / " + player.maxHealth;
            attackText.text = "Attack: " + player.attackDamages;
            classText.text = "Class: " + player.Class;
        }
        else if (character.tag == "Enemy")
        {
            EnemyAI enemy = character.GetComponent<EnemyAI>();
            healthText.text = "Health: " + enemy.health + " / " + enemy.maxHealth;
            attackText.text = "Attack: " + enemy.attackDamages;
            classText.text = "";
        }
        StatsHolder.SetActive(true);
    }

    public void hideCharacterStatistics()
    {
        StatsHolder.SetActive(false);
    }

    private void OnMouseOver()
    {
        displayCharacterStatistics(this.gameObject);
    }

    private void OnMouseExit()
    {
        if (this.CompareTag("Player"))
        {
            if (!this.GetComponent<Player>().selected)
            {
                hideCharacterStatistics();
            }
        }
        else { hideCharacterStatistics(); }
    }

    private void OnMouseDown()
    {
        if (this.CompareTag("Player"))
        {
            Player player = this.GetComponent<Player>();
            if (player.selected)
            {
                displayCharacterStatistics(this.gameObject);
            }
            
        }
        
    }
}
