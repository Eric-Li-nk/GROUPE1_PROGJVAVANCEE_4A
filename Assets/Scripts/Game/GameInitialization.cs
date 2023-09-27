using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameInitialization : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Transform blocList;
    [SerializeField] private GameObject blocPrefab;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject RandomIAPrefab;
    [SerializeField] private GameObject MCTSIAPrefab;

    [SerializeField] private Transform spawnPointPlayer1;
    [SerializeField] private Transform spawnPointPlayer2;

    void Start()
    {
        SpawnBlocs();
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        SpawnPlayer(_gameConfig.player1, spawnPointPlayer1);
        SpawnPlayer(_gameConfig.player2, spawnPointPlayer2);
    }

    private void SpawnPlayer(PlayerType playerType, Transform position)
    {
        GameObject prefabToSpawn = null;
        switch (playerType)
        {
            case PlayerType.None:
                Debug.LogError("Players is not set !!!!");
                return;
            case PlayerType.Human:
                prefabToSpawn = playerPrefab;
                break;
            case PlayerType.Random:
                prefabToSpawn = RandomIAPrefab;
                break;
            case PlayerType.MCTS:
                prefabToSpawn = MCTSIAPrefab;
                break;
        }
        Instantiate(prefabToSpawn, spawnPointPlayer1);
    }

    private void SpawnBlocs()
    {
        string fileName = "Assets/Map/map1.txt";
        StreamReader reader = new StreamReader(fileName);
        char c = (char) reader.Read();
        int x = -1, z = 1;
        while (!reader.EndOfStream)
        {
            c = (char) reader.Read();
            
            Vector3 position = new Vector3(x, blocPrefab.transform.position.y, z);
            switch (c)
            {
                case '0':
                    Instantiate(blocPrefab, position, blocPrefab.transform.rotation, blocList);
                    x += 1;
                    break;
                case '\n':
                    x = -1;
                    z -= 1;
                    break;
                default:
                    x += 1;
                    break;
            }
        }
        reader.Close();

    }
}
