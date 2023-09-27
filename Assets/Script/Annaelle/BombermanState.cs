using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombermanState : MonoBehaviour
{
    public enum PlayerAction{
        DoNothing = 0,
        GoLeft = 1,
        GoRight = 2,
        GoUp = 3,
        GoDown = 4,
        PutBomb = 5
    }

    public static bool isGameOver;
    private float score;
    private PlayerAction act;

    [SerializeField] private List<GameObject> player;
    [SerializeField] private GameObject Player1Spawn;
    [SerializeField] private GameObject Player2Spawn;
    
    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void Reset()
    {
        player[0].transform.position = Player1Spawn.transform.position;
        player[1].transform.position = Player2Spawn.transform.position;
    }

    

    public float GetScore()
    {
        return score;
    }
}


