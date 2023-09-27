using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombermanState : GameState
{
    public enum PlayerAction{
        DoNothing = 0,
        GoLeft = 1,
        GoRight = 2,
        GoUp = 3,
        GoDown = 4,
        PutBomb = 5
    }

    private bool isGameOver;
    private float score;
    private PlayerAction act;
    
    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void Reset()
    {
        
    }

    public void Act(BombermanState.PlayerAction action)
    {
        
    }

    public void Render()
    {
        
    }

    public BombermanState.PlayerAction GetUserInputs()
    {
        return act;
    }

    public float GetScore()
    {
        return score;
    }
}


