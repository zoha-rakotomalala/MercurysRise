using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [HideInInspector]
    public Player targetedPlayer;
    public EnemyAI enemy;
    public bool currentlyAttacking = false;
    [SerializeField] private OverlaySystem OverlaySystem;
    private bool overlayDisplayed = false;
    public Button moveButton;
    public Button attackButton;
    public Button waitButton;
    public void displayMenu()
    {
        this.gameObject.SetActive(true);
        if (overlayDisplayed)
        {
            OverlaySystem.overlayTilemap.ClearAllTiles();
            overlayDisplayed = false;
        }
        if (targetedPlayer.hasMoved)
        {
            moveButton.interactable= false;
        }
        else
        {
            moveButton.interactable= true;
        }
        if (targetedPlayer.hasAttacked)
        {
            attackButton.interactable= false;
        }
        else
        {
            attackButton.interactable= true;
        }
        currentlyAttacking = false;
    }

    public void hideMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void Move()
    {
        OverlaySystem.ShowValidMoveLocations(targetedPlayer);
        overlayDisplayed = true;
        //FindObjectOfType<GameManager>().availableCharacters--;
        //targetedPlayer.hasMoved = true;
        hideMenu();
    }

    public void Wait()
    {
        targetedPlayer.hasMoved = true;
        targetedPlayer.hasAttacked= true;
        FindObjectOfType<GameManager>().availableCharacters--;
        hideMenu();
        targetedPlayer.selected = false;
    }

    public void Attack()
    {
        // TODO: Highlight the potential attack tiles on the overlay
        /*if (Mathf.Abs(Vector3.Distance(targetedPlayer.transform.position,enemy.transform.position))<=targetedPlayer.attackRange) {
            targetedPlayer.Attack(enemy.gameObject);
        }*/
        currentlyAttacking = true;
        overlayDisplayed = true;
        OverlaySystem.ShowValidAttackLocations(targetedPlayer);
        hideMenu();
    }

    private void Start()
    {
        OverlaySystem = FindObjectOfType<OverlaySystem>();
    }
}
