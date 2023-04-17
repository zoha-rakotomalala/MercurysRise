using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [HideInInspector]
    public Player targetedPlayer;
    public EnemyAI enemy;
    public bool currentlyAttacking = false;
    [SerializeField] private OverlaySystem OverlaySystem;
    public void displayMenu()
    {
        this.gameObject.SetActive(true);
        currentlyAttacking = false;
    }

    public void hideMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void Move()
    {
        OverlaySystem.ShowValidMoveLocations(targetedPlayer);
        //FindObjectOfType<GameManager>().availableCharacters--;
        //targetedPlayer.hasMoved = true;
        hideMenu();
    }

    public void Wait()
    {
        targetedPlayer.hasMoved = true;
        FindObjectOfType<GameManager>().availableCharacters--;
        hideMenu();
    }

    public void Attack()
    {
        // TODO: Highlight the potential attack tiles on the overlay
        /*if (Mathf.Abs(Vector3.Distance(targetedPlayer.transform.position,enemy.transform.position))<=targetedPlayer.attackRange) {
            targetedPlayer.Attack(enemy.gameObject);
        }*/
        currentlyAttacking = true;
        OverlaySystem.ShowValidAttackLocations(targetedPlayer.GetComponent<Classes>());
        hideMenu();
    }

    private void Start()
    {
        OverlaySystem = FindObjectOfType<OverlaySystem>();
    }
}
