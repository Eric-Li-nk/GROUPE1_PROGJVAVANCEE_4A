using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject placeBomb;
    [SerializeField] private GameObject Bomb;

    [SerializeField] private float moveSpeed;
    
    private char[][] map = new char[25][];
    
    private Rigidbody _rigidbody;
    private Vector2 moveInput = Vector2.zero;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        intiateMap(map);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnPlaceBomb(InputAction.CallbackContext context)
    {
        GameObject newBomb = Instantiate(Bomb);
        newBomb.transform.position = placeBomb.transform.position;
        Destroy(newBomb,2.6f);
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
    }
    
    void intiateMap(char[][] map){
        string fileName = "Assets/Map/map1.txt";
        TextReader reader;
        reader = new  StreamReader(fileName);
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
        
        //printMap(map);
        
    }

    void printMap(char[][] map)
    {
        for (int row = 0; row < 15; row++)
        {
            string line = "";
            for (int col = 0; col < 25; col++)
            {
                line += map[row][col];
            }
            
            Debug.Log(line);
        }
    }

    int updateMap(char[][] map, char jr, BombermanState.PlayerAction act)
    {
        int pos=0;
        int currentRow = 0;
        for (int row = 0; row < 15; row++)
        {
            string line = "";
            for (int col = 0; col < 25; col++)
            {
                line += map[row][col];
            }

            if (line.Contains(jr))
            {
                pos = line.IndexOf(jr);
                currentRow = row;
                break;
            }
        }

        switch (act)
        {
            case BombermanState.PlayerAction.DoNothing :
                break;
            case BombermanState.PlayerAction.GoLeft :
                return MoveLeft(map, currentRow, pos);
            case BombermanState.PlayerAction.GoRight : 
                return MoveRight(map, currentRow, pos);
            case BombermanState.PlayerAction.GoUp : 
                return MoveUp(map, currentRow, pos);
            case BombermanState.PlayerAction.GoDown : 
                return MoveDown(map, currentRow, pos);
            default:
                return 0;
        }
        return 0;
    }


    int MoveLeft(char[][] map, int row,int col)
    {
        if (map[row][col - 1] == '0')
        {
            map[row][col - 1] = 'J';
            map[row][col] = '0';
            printMap(map);
            return 1;
        }
        return 0;
    }
    
    int MoveRight(char[][] map, int row,int col)
    {
        if (map[row][col + 1] == '0')
        {
            map[row][col +1] = 'J';
            map[row][col] = '0';
            printMap(map);
            return 1;
        }
        return 0;
    }
    
    int MoveUp(char[][] map, int row,int col)
    {
        if (row -1 >0 && map[row-1][col] == '0')
        {
            map[row-1][col] = 'J';
            map[row][col] = '0';
            printMap(map);
            return 1;
        }
        return 0;
    }
    
    int MoveDown(char[][] map, int row,int col)
    {
        if (row +1 <25 && map[row+1][col] == '0')
        {
            map[row+1][col] = 'J';
            map[row][col] = '0';
            printMap(map);
            return 1;
        }
        return 0;
    }
}

