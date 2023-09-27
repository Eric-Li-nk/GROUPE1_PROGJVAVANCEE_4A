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
    [SerializeField] private GameMode gamemode;
    private GameState game;

    private void Update()
    {
        game.Render();
        while(!game.IsGameOver())
        {
            var inputs = game.GetUserInputs();
            game.Act(inputs);
            game.Render();
        }
        
        Debug.Log($"End + : {game.GetScore()}");
        game.Reset();
    }
}
