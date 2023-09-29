using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeMCTS
{
    private NodeMCTS parent;

    public List<NodeMCTS> children;

    private int nbWin;

    private int numberVisit;

    private GameState _gameState;

    private BombermanState.PlayerAction currentAct;

    public char playerChar;
    
    public NodeMCTS(NodeMCTS parent, List<NodeMCTS> children, int nbWin, GameState gameState, BombermanState.PlayerAction currentAct)
    {
        this.parent = parent;
        this.children = children;

        this.nbWin = nbWin;
        _gameState = gameState;
        this.currentAct = currentAct;
    }

    public NodeMCTS(GameState currentState)
    {
        this.parent = null;
        this.children = new List<NodeMCTS>();
        this.nbWin = 0;
        this._gameState = currentState;
        this.currentAct = BombermanState.PlayerAction.DoNothing;
    }
    
    public NodeMCTS(GameState currentState, NodeMCTS parent, BombermanState.PlayerAction selectAct)
    {
        this.parent = parent;
        this.children = new List<NodeMCTS>();
        this.nbWin = 0;
        this._gameState = currentState;
        this.currentAct = selectAct;
    }
    public NodeMCTS GetParent()
    {
        return this.parent;
    }
    
    public List<NodeMCTS> GetChild()
    {
        return this.children;
    }
    
    public int GetWin()
    {
        //temps de survie + 1 si vic
        //temps de survie -1 si lose
        return this.nbWin;
        
        
    }

    public void SetWin(int x)
    {
        this.nbWin = x;
    }

    public void SetNumberVisit(int x)
    {
        this.numberVisit = x;
    }

    public int GetNumberVisit()
    {
        return numberVisit;
    }
    
    
    public GameState GetGameState()
    {
        return this._gameState;
    }

    public void setGameState(GameState gameState)
    {
        this._gameState = gameState;
    }
    
    public BombermanState.PlayerAction GetActionPlayer()
    {
        return this.currentAct;
    }
    
}
