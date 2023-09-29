using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/**
 *
 * Script pour contronler l'IA en utilisant MCTS
 * 
 */
public class MCTSController : MonoBehaviour
{
    //action a jouer
    private BombermanState.PlayerAction act;
    
    //Noeud de depart
    private NodeMCTS StartNode;
    
    //Game state de depart
    private GameState currentGameState;
    
    //list de tous les mouvements possibles
    private List<BombermanState.PlayerAction> MoveToPlay;

    public char playerChar;
    
    //emplacement de la bombe
    [SerializeField] private GameObject placeBomb;
    [SerializeField] private GameObject Bomb;
    
    //nombre de tick a effectuer
    private int numberTest = 30;
    
    //nombre de simulation
    private int numberSimulation = 50;
    
    //facteur d'exploration
    private float factEploration = 0.6f;
    
    private Rigidbody _rigidbody;
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        //initialisation du gameState
        currentGameState = new GameState();
        StartNode = new NodeMCTS(currentGameState);
        MoveToPlay = new List<BombermanState.PlayerAction>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //activation du MCTS
        act = MCTSAction();
        
        //effectuer l'action selectionner
        PlayAction(act);
    }

    private BombermanState.PlayerAction MCTSAction()
    {
        //Copie le game state dans le noeud parent
        StartNode.setGameState(currentGameState);
        
        //lance la boucle
        for (int i = 0; i < numberTest; i++)
        {
            NodeMCTS selectedNode = Selection();
            NodeMCTS newNode = Expand(selectedNode);
            int nb_vic = Simulation(newNode, numberSimulation);
            Backpropagation(newNode, nb_vic, numberSimulation);
        }
        
        return GetBestScore(StartNode).GetActionPlayer();
    }

    /*
     * selection du noeud a partir duquel on vas faire la simulation
     *
     * retourne le node selectionné
     */
    private NodeMCTS Selection()
    {
        //si le noeud n'a pas d'enfant, on retoure le noeud de depart
        if (StartNode.GetChild() == null)
            return StartNode;
        
        /*
         * creation d"un nombre aleatoire pour choisir si on effectue une exploration ou une exploitation
         *
         * si rand est inferieur au facteur d'exploration, on fait une exploration sinon on fait une exploitation
         */
        float randNb = Random.Range(0, 1);
        
        if (randNb < factEploration)
        {
            /*
             * selection aléatoire d'un noeud enfant
             * 
             */ 
            getListAction();
            int rand = Random.Range(0, MoveToPlay.Count);
            List<NodeMCTS> children = StartNode.GetChild();
            return children[rand];
        }

        //exploiter (best noeud)
        NodeMCTS best = GetBestScore(StartNode);
        return best;
        
    }

    private NodeMCTS Expand(NodeMCTS selectNode)
    {
        getListAction();
        if (MoveToPlay.Count == 0)
        {
            return selectNode;
        }
        BombermanState.PlayerAction selectAct = MoveToPlay[Random.Range(0, MoveToPlay.Count)];

        GameState newState = ApplyAction(selectNode.GetGameState(), selectAct);

        NodeMCTS newNode = new NodeMCTS(newState, selectNode,selectAct);
            
        selectNode.children.Add(newNode);

        return newNode;
    }

    private int Simulation(NodeMCTS startNode, int nbSimulation)
    {
        int nbWin = 0;
        for (int i = 0; i < nbSimulation; i++)
        {
            BombermanState.PlayerAction selectAction = selectRandAct(MoveToPlay);
            GameState copyCurrentGamestate = currentGameState.Copy();
            while (copyCurrentGamestate.IsGameOver())
            {
                copyCurrentGamestate.RefreshBoard();
                copyCurrentGamestate.PlayAction(selectRandAct(getListAction(copyCurrentGamestate.GetBoard())));
            }

            nbWin += copyCurrentGamestate.win;
        }

        return nbWin;
    }

    private void Backpropagation(NodeMCTS newNode, int nbVic, int nbSim)
    {
        if (newNode.GetParent() != null)
        {
            NodeMCTS parent = newNode.GetParent();
            parent.SetWin(parent.GetWin() + nbVic);
            parent.SetNumberVisit(parent.GetNumberVisit() + nbSim);
            Backpropagation(parent, parent.GetWin(), parent.GetNumberVisit());
        }
    }
    
    private bool inBoard(int row, int col)
    {
        return row is >= 0 and < 15 && col is >= 0 and < 25;
    }

    private void getListAction()
    {
        int pos_x = (int)Math.Floor(this.transform.position.x);
        int pos_y = (int)Math.Floor(this.transform.position.y);

        MoveToPlay.Clear();

        char[][] map = currentGameState.GetBoard();
        //if (map[pos_x][pos_y] == 'A')
        MoveToPlay.Add(BombermanState.PlayerAction.DoNothing);
        MoveToPlay.Add(BombermanState.PlayerAction.PutBomb);
        
        if(inBoard(pos_x,pos_y-1) && (map[pos_x][pos_y-1] != 'M' || map[pos_x][pos_y-1] != '0'))
            MoveToPlay.Add(BombermanState.PlayerAction.GoLeft);
            
        if(inBoard(pos_x,pos_y+1) && (map[pos_x][pos_y+1] != 'M' || map[pos_x][pos_y+1] != '0'))
            MoveToPlay.Add(BombermanState.PlayerAction.GoRight);
            
        if(inBoard(pos_x+1,pos_y) && (map[pos_x+1][pos_y] != 'M' || map[pos_x+1][pos_y] != '0'))
            MoveToPlay.Add(BombermanState.PlayerAction.GoDown);
            
        if(inBoard(pos_x-1,pos_y) && (map[pos_x-1][pos_y] != 'M' || map[pos_x-1][pos_y] != '0'))
            MoveToPlay.Add(BombermanState.PlayerAction.GoUp);
    }

    private List<BombermanState.PlayerAction> getListAction(char[][] map)
    {
        List<BombermanState.PlayerAction> moveToPlay = new List<BombermanState.PlayerAction>();
        int pos_x = 0, pos_y = 0;
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 25; j++)
            {
                if (map[i][j] == playerChar)
                {
                    pos_x = i;
                    pos_y = j;
                }
            }
        }
        
        //if (map[pos_x][pos_y] == 'A')
        moveToPlay.Add(BombermanState.PlayerAction.DoNothing);
        moveToPlay.Add(BombermanState.PlayerAction.PutBomb);
            
        if(inBoard(pos_x,pos_y-1) && (map[pos_x][pos_y-1] != 'M' || map[pos_x][pos_y-1] != '0'))
            moveToPlay.Add(BombermanState.PlayerAction.GoLeft);
            
        if(inBoard(pos_x,pos_y+1) && (map[pos_x][pos_y+1] != 'M' || map[pos_x][pos_y+1] != '0'))
            moveToPlay.Add(BombermanState.PlayerAction.GoRight);
            
        if(inBoard(pos_x+1,pos_y) && (map[pos_x+1][pos_y] != 'M' || map[pos_x+1][pos_y] != '0'))
            moveToPlay.Add(BombermanState.PlayerAction.GoDown);
            
        if(inBoard(pos_x-1,pos_y) && (map[pos_x-1][pos_y] != 'M' || map[pos_x-1][pos_y] != '0'))
            moveToPlay.Add(BombermanState.PlayerAction.GoUp);
        
        return moveToPlay;
    }
    
    private GameState ApplyAction(GameState state, BombermanState.PlayerAction act)
    {
        GameState newGameState = state.Copy();
        switch (act)
        {
            case BombermanState.PlayerAction.DoNothing:
                break;
            case BombermanState.PlayerAction.GoLeft :
                MoveLeft(state);
                break;
            case BombermanState.PlayerAction.GoRight : 
                MoveRight(state);
                break;
            case BombermanState.PlayerAction.GoUp :
                MoveUp(state);
                break;
            case BombermanState.PlayerAction.GoDown : 
                MoveDown(state);
                break;
            case BombermanState.PlayerAction.PutBomb : 
                PutBomb(state);
                break;
            default:
                break;
        }

        return newGameState;
    }

    private void MoveLeft(GameState state)
    {
        int pos_x = Mathf.RoundToInt(this.transform.position.x);
        int pos_y = Mathf.RoundToInt(this.transform.position.y);

        char[][] map = currentGameState.GetBoard();

        map[pos_x][pos_y] = 'L';
        map[pos_x][pos_y - 1] = 'B';
    }

    private void MoveRight(GameState state)
    {
        int pos_x = Mathf.RoundToInt(this.transform.position.x);
        int pos_y = Mathf.RoundToInt(this.transform.position.y);

        char[][] map = currentGameState.GetBoard();

        map[pos_x][pos_y] = 'L';
        map[pos_x][pos_y + 1] = 'B';
    }
    
    private void MoveUp(GameState state)
    {
        int pos_x = Mathf.RoundToInt(this.transform.position.x);
        int pos_y = Mathf.RoundToInt(this.transform.position.y);

        char[][] map = currentGameState.GetBoard();

        map[pos_x][pos_y] = 'L';
        map[pos_x - 1][pos_y] = 'B';
    }
    
    private void MoveDown(GameState state)
    {
        int pos_x = Mathf.RoundToInt(this.transform.position.x);
        int pos_y = Mathf.RoundToInt(this.transform.position.y);

        char[][] map = currentGameState.GetBoard();

        map[pos_x][pos_y] = 'L';
        map[pos_x + 1][pos_y] = 'B';
    }
    
    private void PutBomb(GameState state)
    {
        int pos_x = Mathf.RoundToInt(this.transform.position.x);
        int pos_y = Mathf.RoundToInt(this.transform.position.y);

        currentGameState.bombBoard[pos_x][pos_y] = '3';
    }
    
    private BombermanState.PlayerAction selectRandAct(List<BombermanState.PlayerAction> acts)
    {
        int nbRand = Random.Range(0, acts.Count);
        return acts[nbRand];
    }

    private NodeMCTS GetBestScore(NodeMCTS node)
    {
        NodeMCTS bestChild = null;
        float bestscore = float.MinValue;
        float avgScore;

        foreach (var child in node.GetChild())
        {
            int numberVisit = child.GetNumberVisit();
            
            avgScore = child.GetWin() / numberVisit;

            if (avgScore > bestscore)
            {
                bestscore = avgScore;
                bestChild = child;
            }
        }
        
        return bestChild;
    }

    private void PlayAction(BombermanState.PlayerAction act)
    {
        Vector3 moveInput = Vector3.zero;
        switch (act)
        {
            case BombermanState.PlayerAction.DoNothing:
                break;
            case BombermanState.PlayerAction.GoLeft :
                moveInput = new Vector3(-1, 0, 0);
                MoveLeft(currentGameState);
                break;
            case BombermanState.PlayerAction.GoRight :
                moveInput = new Vector3(1, 0, 0);
                MoveRight(currentGameState);
                break;
            case BombermanState.PlayerAction.GoUp :
                moveInput = new Vector3(0, 0, 1);
                MoveUp(currentGameState);
                break;
            case BombermanState.PlayerAction.GoDown : 
                moveInput = new Vector3(0, 0, -1);
                MoveDown(currentGameState);
                break;
            case BombermanState.PlayerAction.PutBomb : 
                GameObject newBomb = Instantiate(Bomb);
                newBomb.transform.position = placeBomb.transform.position;
                newBomb.transform.position = new Vector3(Mathf.RoundToInt(newBomb.transform.position.x), 
                newBomb.transform.position.y, Mathf.RoundToInt(newBomb.transform.position.z));
                
                Destroy(newBomb,2.6f);
                PutBomb(currentGameState);
                break;
            default:
                break;
        }
        _rigidbody.velocity = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
    }
    
}
