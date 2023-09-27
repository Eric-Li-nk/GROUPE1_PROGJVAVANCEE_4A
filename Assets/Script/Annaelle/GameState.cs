using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameState
{
    bool IsGameOver();
    void Reset();

    void Act(BombermanState.PlayerAction action);

    void Render(); 
    BombermanState.PlayerAction GetUserInputs();
    float GetScore();
}
