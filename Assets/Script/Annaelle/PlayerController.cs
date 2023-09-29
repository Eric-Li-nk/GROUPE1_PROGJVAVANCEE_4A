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
        if(context.ReadValueAsButton())
        {
            GameObject newBomb = Instantiate(Bomb);
            newBomb.transform.position = placeBomb.transform.position;
            newBomb.transform.position = new Vector3(Mathf.RoundToInt(newBomb.transform.position.x),
                newBomb.transform.position.y, Mathf.RoundToInt(newBomb.transform.position.z));
            Destroy(newBomb, 2.6f);
        }
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
        
        
    }
    
}

