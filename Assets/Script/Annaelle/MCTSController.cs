using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using Random = UnityEngine.Random;

public class MCTSController : MonoBehaviour
{
    private BombermanState.PlayerAction act;
    private NodeMCTS StartNode;
    private GameState currentGameState;
    private List<BombermanState.PlayerAction> MoveToPlay;
    
    private int numberTest = 10;
    private int numberSimulation = 5;
    private float factEploration = 0.8f;

    private void Start()
    {
        currentGameState = new GameState();
        
    }

    void Update()
    {
        act = MCTSAction();
    }

    private BombermanState.PlayerAction MCTSAction()
    {
        //boucle select/expand/simul/retro xfois
        StartNode.setGameState(currentGameState);
        for (int i = 0; i < numberTest; i++)
        {
            NodeMCTS selectedNode = Selection();
            NodeMCTS newNode = Expand(selectedNode);
            Time timer = Simulation(newNode, numberSimulation);
            Backpropagation(newNode, timer, numberSimulation);
        }
        
        return GetBest();
    }

    private NodeMCTS Selection()
    {
        if (StartNode.GetChild() == null)
            return StartNode;
        else
        {
            float randNb = Random.Range(0, 1);
            //select aleatoire
            if (randNb < factEploration)
            {
                getListAction();
                int rand = Random.Range(0, MoveToPlay.Count);
                List<NodeMCTS> children = StartNode.GetChild();
                return children[rand];
            }
            else
            {
                //exploiter (best noeud)
                NodeMCTS best = GetBestScore();
                return best;
            }
        }
        
    }

    private void getListAction()
    {
        int pos_x = (int)Math.Floor(this.transform.position.x);
        int pos_y = (int)Math.Floor(this.transform.position.y);

        char[][] map = currentGameState.GetBoard();
        if (map[pos_x][pos_y] == 'A')
        {
            MoveToPlay.Add(BombermanState.PlayerAction.DoNothing);
            MoveToPlay.Add(BombermanState.PlayerAction.PutBomb);
            
            if(map[pos_x-1][pos_y] == 'L')
                MoveToPlay.Add(BombermanState.PlayerAction.GoLeft);
            
            if(map[pos_x+1][pos_y] == 'L')
                MoveToPlay.Add(BombermanState.PlayerAction.GoRight);
            
            if(map[pos_x][pos_y-1] == 'L')
                MoveToPlay.Add(BombermanState.PlayerAction.GoDown);
            
            if(map[pos_x][pos_y+1] == 'L')
                MoveToPlay.Add(BombermanState.PlayerAction.GoUp);
        }
    }

    private NodeMCTS GetBestScore()
    {
        //funct pour recup BestScore
        return NodeMCTS;
    }
    
    private BombermanState.PlayerAction GetBest()
    {
        
        return StartNode.GetActionPlayer();
    }
}
