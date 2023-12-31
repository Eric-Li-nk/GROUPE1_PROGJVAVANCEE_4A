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
    private List<BombermanState.PlayerAction> MoveToPlay = new List<BombermanState.PlayerAction>();

    public char playerChar;
    
    //emplacement de la bombe
    [SerializeField] private GameObject placeBomb;
    [SerializeField] private GameObject Bomb;
    
    //nombre de tick a effectuer
    private int numberTest = 10;
    
    //nombre de simulation
    private int numberSimulation = 30;
    
    //facteur d'exploration
    private float factEploration = 0.6f;
    
    private Rigidbody _rigidbody;
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        //initialisation du gameState
        currentGameState = new GameState();
        StartNode = new NodeMCTS(currentGameState);
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        currentGameState.RefreshBoardUnity();
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
            int score = Simulation(newNode, numberSimulation);
            Backpropagation(newNode, score, numberSimulation);
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
        if (StartNode.GetChild().Count == 0)
        {
            return StartNode;
        }

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
            List<NodeMCTS> children = StartNode.GetChild();
            int rand = Random.Range(0, children.Count);
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
        BombermanState.PlayerAction selectAct = selectRandAct(MoveToPlay);
    
        GameState newState = ApplyAction(selectNode.GetGameState(), selectAct);

        NodeMCTS newNode = new NodeMCTS(newState, selectNode, selectAct);
            
        selectNode.children.Add(newNode);
        
        return newNode;
    }

    private int Simulation(NodeMCTS startNode, int nbSimulation)
    {
        int score = 0;
        for (int i = 0; i < nbSimulation; i++)
        {
            GameState copyCurrentGamestate = (GameState) startNode.GetGameState().Clone();
            while (!copyCurrentGamestate.IsGameOver())
            {
                copyCurrentGamestate.PlayAction(selectRandAct(getListAction(copyCurrentGamestate.GetBoard(), copyCurrentGamestate.bombBoard)));
            }
            score += copyCurrentGamestate.score;
        }
        return score;
    }

    private void Backpropagation(NodeMCTS newNode, int score, int nbSim)
    {
        if (newNode.GetParent() != null)
        {
            NodeMCTS parent = newNode.GetParent();
            parent.SetWin(parent.GetWin() + score);
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
        int pos_x = Mathf.RoundToInt(-this.transform.position.z) + 1;
        int pos_y = Mathf.RoundToInt(this.transform.position.x) + 1;

        char[][] bombMap = currentGameState.bombBoard;
        
        MoveToPlay.Clear();

        char[][] map = currentGameState.GetBoard();
        
        MoveToPlay.Add(BombermanState.PlayerAction.DoNothing);
        
        if(inBoard(pos_x,pos_y) && bombMap[pos_x][pos_y] == '_')
            MoveToPlay.Add(BombermanState.PlayerAction.PutBomb);
        
        if(inBoard(pos_x,pos_y-1) && (map[pos_x][pos_y-1] != 'M' && map[pos_x][pos_y-1] != '0'))
            MoveToPlay.Add(BombermanState.PlayerAction.GoLeft);
            
        if(inBoard(pos_x,pos_y+1) && (map[pos_x][pos_y+1] != 'M' && map[pos_x][pos_y+1] != '0'))
            MoveToPlay.Add(BombermanState.PlayerAction.GoRight);
            
        if(inBoard(pos_x+1,pos_y) && (map[pos_x+1][pos_y] != 'M' && map[pos_x+1][pos_y] != '0'))
            MoveToPlay.Add(BombermanState.PlayerAction.GoDown);
            
        if(inBoard(pos_x-1,pos_y) && (map[pos_x-1][pos_y] != 'M' && map[pos_x-1][pos_y] != '0'))
            MoveToPlay.Add(BombermanState.PlayerAction.GoUp);
    }

    private List<BombermanState.PlayerAction> getListAction(char[][] map, char[][] bombMap)
    {
        List<BombermanState.PlayerAction> moveToPlay = new List<BombermanState.PlayerAction>();
        int pos_x = -1, pos_y = -1;
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
        
        moveToPlay.Add(BombermanState.PlayerAction.DoNothing);
        
        if(inBoard(pos_x,pos_y) && bombMap[pos_x][pos_y] == '_')
            moveToPlay.Add(BombermanState.PlayerAction.PutBomb);
            
        if(inBoard(pos_x,pos_y-1) && (map[pos_x][pos_y-1] != 'M' && map[pos_x][pos_y-1] != '0'))
            moveToPlay.Add(BombermanState.PlayerAction.GoLeft);
            
        if(inBoard(pos_x,pos_y+1) && (map[pos_x][pos_y+1] != 'M' && map[pos_x][pos_y+1] != '0'))
            moveToPlay.Add(BombermanState.PlayerAction.GoRight);
            
        if(inBoard(pos_x+1,pos_y) && (map[pos_x+1][pos_y] != 'M' && map[pos_x+1][pos_y] != '0'))
            moveToPlay.Add(BombermanState.PlayerAction.GoDown);
            
        if(inBoard(pos_x-1,pos_y) && (map[pos_x-1][pos_y] != 'M' && map[pos_x-1][pos_y] != '0'))
            moveToPlay.Add(BombermanState.PlayerAction.GoUp);
        
        return moveToPlay;
    }

    //Permet de simuler l'action sur le gamestate passe en parametre puis d'en retourner une copie avec l'action effectuee
    private GameState ApplyAction(GameState state, BombermanState.PlayerAction act)
    {
        GameState newGameState = (GameState) state.Clone();
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

    // Fonctions pour simuler les actions de l'ia
    private void MoveLeft(GameState state)
    {
        int pos_x = Mathf.RoundToInt(-this.transform.position.z) + 1;
        int pos_y = Mathf.RoundToInt(this.transform.position.x) + 1;

        currentGameState.board[pos_x][pos_y] = '_';
        currentGameState.board[pos_x][pos_y - 1] = 'B';
    }

    private void MoveRight(GameState state)
    {
        int pos_x = Mathf.RoundToInt(-this.transform.position.z) + 1;
        int pos_y = Mathf.RoundToInt(this.transform.position.x) + 1;

        currentGameState.board[pos_x][pos_y] = '_';
        currentGameState.board[pos_x][pos_y + 1] = 'B';
    }
    
    private void MoveUp(GameState state)
    {
        int pos_x = Mathf.RoundToInt(-this.transform.position.z) + 1;
        int pos_y = Mathf.RoundToInt(this.transform.position.x) + 1;

        currentGameState.board[pos_x][pos_y] = '_';
        currentGameState.board[pos_x - 1][pos_y] = 'B';
    }
    
    private void MoveDown(GameState state)
    {
        int pos_x = Mathf.RoundToInt(-this.transform.position.z) + 1;
        int pos_y = Mathf.RoundToInt(this.transform.position.x) + 1;
        
        currentGameState.board[pos_x][pos_y] = '_';
        currentGameState.board[pos_x + 1][pos_y] = 'B';
    }
    
    private void PutBomb(GameState state)
    {
        int pos_x = Mathf.RoundToInt(-this.transform.position.z) + 1;
        int pos_y = Mathf.RoundToInt(this.transform.position.x) + 1;
        
        currentGameState.bombBoard[pos_x][pos_y] = '3';
    }

    //Fonction pour selectionner une action aleatoire selon la liste d'action passee en parametre
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

    //Fonction pour que l'ia effectuer les vraies actions
    private void PlayAction(BombermanState.PlayerAction act)
    {
        Vector3 moveInput = Vector3.zero;
        switch (act)
        {
            case BombermanState.PlayerAction.DoNothing:
                break;
            case BombermanState.PlayerAction.GoLeft :
                moveInput = new Vector3(-1, 0, 0);
                break;
            case BombermanState.PlayerAction.GoRight :
                moveInput = new Vector3(1, 0, 0);
                break;
            case BombermanState.PlayerAction.GoUp :
                moveInput = new Vector3(0, 0, 1);
                break;
            case BombermanState.PlayerAction.GoDown : 
                moveInput = new Vector3(0, 0, -1);
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
        _rigidbody.velocity = new Vector3(moveInput.x, 0, moveInput.z) * moveSpeed;
    }
    
}
