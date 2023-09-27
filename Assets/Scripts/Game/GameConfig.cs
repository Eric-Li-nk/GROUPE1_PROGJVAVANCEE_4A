using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    None,
    Human,
    Random,
    MCTS
}

[CreateAssetMenu(fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public PlayerType player1;
    public PlayerType player2;
}
