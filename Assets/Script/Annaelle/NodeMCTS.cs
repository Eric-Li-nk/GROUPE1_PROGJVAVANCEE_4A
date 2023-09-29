using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeMCTS : MonoBehaviour
{
    private NodeMCTS parent;

    private List<NodeMCTS> children;

    private Time timeSurvied;


    private GameState _gameState;

    private BombermanState.PlayerAction currentAct;
    
    public NodeMCTS(NodeMCTS parent, List<NodeMCTS> children, Time timeSurvied, GameState gameState, BombermanState.PlayerAction currentAct)
    {
        this.parent = parent;
        
        for (int i = 0; i < children.Count; i++)
        {
            this.children.Add(children[i]);
        }
        
        this.timeSurvied = timeSurvied;
        _gameState = gameState;
        this.currentAct = currentAct;
    }

    public NodeMCTS(GameState currentState)
    {
        this.parent = null;
        this.children = null;
        this.timeSurvied = null;
        this._gameState = currentState;
        this.currentAct = BombermanState.PlayerAction.DoNothing;
    }
    
    public NodeMCTS GetParent()
    {
        return this.parent;
    }
    
    public List<NodeMCTS> GetChild()
    {
        return this.children;
    }
    
    public Time GetTimeSurvied()
    {
        //temps de survie + 1 si vic
        //temps de survie -1 si lose
        return this.timeSurvied;
    }

    public void createChildren()
    {
        
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
