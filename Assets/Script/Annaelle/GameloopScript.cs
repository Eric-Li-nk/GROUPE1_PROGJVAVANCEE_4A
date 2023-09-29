using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameMode{
    PvP,
    PvRand,
    PvMCTS
}

public class GameloopScript : MonoBehaviour
{
    private BombermanState bombermanstate;
    
    [SerializeField] private GameMode gamemode;
    private BombermanState game;
    [SerializeField] private GameObject IA;
    [SerializeField] private GameObject MenuFin;
    

    private void Start()
    {
        MenuFin.SetActive(false);
        switch (gamemode)
        {
            case GameMode.PvP: 
                IA.GetComponent<IaController>().enabled = false;
                IA.GetComponent<PlayerController>().enabled = true;
                break;
            
            case GameMode.PvRand :
                IA.GetComponent<IaController>().enabled = true;
                IA.GetComponent<PlayerController>().enabled = false;
                break;
        }
    }

    private void Update()
    {
        
        if(game.IsGameOver())
        {
            MenuFin.SetActive(true);
        }
       
    }
}
