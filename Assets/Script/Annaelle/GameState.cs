using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private char[][] board = new char[25][];

    private Time timer;
    
    
    public GameState()
    {
        this.board = GameInitialization.map;
        this.timer = null;
    }
    
    public GameState(Time timer)
    {
        this.board = GameInitialization.map;
        this.timer = timer;
    }

    public Time GetTime()
    {
        return this.timer;
    }
    
    public char[][] GetBoard()
    {
        return this.board;
    }
}
