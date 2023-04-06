using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [HideInInspector]
    public Player targetedPlayer;
    [SerializeField] private OverlaySystem OverlaySystem;
    public void displayMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void hideMenu() 
    {
        this.gameObject.SetActive(false);
    }

    public void Move()
    {
        OverlaySystem.ShowValidMoveLocations(targetedPlayer);
        hideMenu();
    }

    private void Start()
    {
        OverlaySystem = FindObjectOfType<OverlaySystem>();
    }
}
