using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameInitialization : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Transform blocList;
    [SerializeField] private GameObject blocPrefab;

    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;
    [SerializeField] private GameObject RandomIAPrefab;
    [SerializeField] private GameObject MCTSIAPrefab;

    [SerializeField] private Transform spawnPointPlayer1;
    [SerializeField] private Transform spawnPointPlayer2;

    [HideInInspector] public static GameObject[][] mapObjet;
    [HideInInspector] public static char[][] map;
    private string filename;

    void Start()
    {
        SelectMap();
        IntiateMap();
        SpawnPlayers();
        GenerateMap();
    }

    private void SpawnPlayers()
    {
        if (_gameConfig.player1 == _gameConfig.player2 && _gameConfig.player1 == PlayerType.Human)
        {
            GameManager.instance.player1 = Instantiate(player1Prefab, spawnPointPlayer1.position, spawnPointPlayer1.rotation);
            GameManager.instance.player2 = Instantiate(player2Prefab, spawnPointPlayer2.position, spawnPointPlayer2.rotation);
        }
        else
        {
            GameManager.instance.player1 = SpawnPlayer(_gameConfig.player1, spawnPointPlayer1);
            GameManager.instance.player2 = SpawnPlayer(_gameConfig.player2, spawnPointPlayer2);
        }

        if (_gameConfig.player1 == PlayerType.MCTS)
            GameManager.instance.player1.GetComponent<MCTSController>().playerChar = 'A';
        if(_gameConfig.player2 == PlayerType.MCTS)
            GameManager.instance.player2.GetComponent<MCTSController>().playerChar = 'B';
    }

    private IEnumerator DestroyBlocs()
    {
        foreach (var row in mapObjet)
        {
            foreach (var bloc in row)
            {
                Destroy(bloc);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    
    private void IntiateMap(){
        var reader = new  StreamReader(filename);
        map = new char[15][];
        string line;
        int row=0;
        while (true)
        {
            line=reader.ReadLine();
            
            if (line==null) break;
            char[] lineArray = line.ToCharArray();
            map[row] = new char[25];

            for (int col = 0; col < 25; col++)
            {
                map[row][col] = lineArray[col];
            }

            row++;

        }
        reader.Close();
    }

    private void GenerateMap()
    {
        char caseMap;
        int i = 0;
        mapObjet = new GameObject[15][];
        for(int row = 0; row < 15; row++)
        {
            mapObjet[row] = new GameObject[25];
            for(int col = 0; col < 25; col++)
            {
                caseMap = map[row][col];
                switch(caseMap)
                {
                    case '0':
                        Vector3 position = new Vector3(col-1, blocPrefab.transform.position.y,  -row + 1);
                        mapObjet[row][col] = Instantiate(blocPrefab, position, blocPrefab.transform.rotation, blocList);;
                        i++;
                        break;
                    case 'M':
                        mapObjet[row][col] = null;
                        break;
                    case 'L':
                        mapObjet[row][col] = null;
                        break;
                    case '1':
                        mapObjet[row][col] = GameManager.instance.player1;
                        break;
                    case '2':
                        mapObjet[row][col] = GameManager.instance.player2;
                        break;
                    case 'A':
                        mapObjet[row][col] = GameManager.instance.player1;
                        break;
                    case 'B':
                        mapObjet[row][col] = GameManager.instance.player2;
                        break;
                }
            }
        }
    }

    private void SelectMap()
    {
        PlayerType joueur1 = _gameConfig.player1;
        PlayerType joueur2 = _gameConfig.player2;
        if(joueur1 == PlayerType.Human && joueur2 == PlayerType.Human){
            filename = "Assets/Map/MapPVP.txt";
        }
        else if((joueur1 == PlayerType.Human && joueur2 == PlayerType.Random) || (joueur1 == PlayerType.Human && joueur2 == PlayerType.MCTS)){
            filename = "Assets/Map/MapPvIA.txt";
        }
        else if((joueur1 == PlayerType.Random && joueur2 == PlayerType.Random) || (joueur1 == PlayerType.Random && joueur2 == PlayerType.MCTS) || (joueur1 == PlayerType.MCTS && joueur2 == PlayerType.MCTS) || (joueur1 == PlayerType.MCTS && joueur2 == PlayerType.Random)){
            filename = "Assets/Map/MapIAvIA.txt";
        }
        else{
            filename = "Assets/Map/Map1.txt";
        }
    }

    private GameObject SpawnPlayer(PlayerType playerType, Transform transform)
    {
        GameObject prefabToSpawn = null;
        switch (playerType)
        {
            case PlayerType.None:
                Debug.LogError("Players is not set !!!!");
                return null;
            case PlayerType.Human:
                prefabToSpawn = player1Prefab;
                break;
            case PlayerType.Random:
                prefabToSpawn = RandomIAPrefab;
                break;
            case PlayerType.MCTS:
                prefabToSpawn = MCTSIAPrefab;
                break;
        }
        return Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }
}
