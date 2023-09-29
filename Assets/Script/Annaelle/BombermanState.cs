using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombermanState : MonoBehaviour
{
    // Enumeration des actions que le joueur et l'ia peuvent effectuer
    public enum PlayerAction{
        DoNothing = 0,
        GoLeft = 1,
        GoRight = 2,
        GoUp = 3,
        GoDown = 4,
        PutBomb = 5
    }
}


