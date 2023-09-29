using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class GameState
{
    public char[][] board = new char[25][];
    public char[][] bombBoard = new char[15][];

    private bool isGameOver;

    public int score = 0;

    public GameState()
    {
        board = GameInitialization.map;
        for (int i = 0; i < 15; i++)
        {
            bombBoard[i] = new char[25];
            for (int j = 0; j < 25; j++)
            {
                bombBoard[i][j] = '_';
            }
        }

        isGameOver = false;
    }

    public GameState(Time timer)
    {
        this.board = GameInitialization.map;
    }
    // Permet de creer une copie du gamestate
    public object Clone()
    {
        MemoryStream mem = new MemoryStream();
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter form =
            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        form.Serialize(mem, this);
        mem.Position = 0;
        return form.Deserialize(mem);
    }

    //Permet de recuperer la position de l'ia sur le board
    private (int, int) GetMCTSPosition()
    {
        int pos_x = -1, pos_y = -1;
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 25; j++)
            {
                if (board[i][j] == 'B')
                {
                    pos_x = i;
                    pos_y = j;
                }
            }
        }

        return (pos_x, pos_y);
    }

    // Permet de simuler l'action passee en parametre sur le board
    public void PlayAction(BombermanState.PlayerAction action)
    {

        RefreshBoard();

        if (isGameOver)
        {
            return;
        }

        (int pos_x, int pos_y) = GetMCTSPosition();

        switch (action)
        {
            case BombermanState.PlayerAction.DoNothing:
                break;
            case BombermanState.PlayerAction.PutBomb:
                bombBoard[pos_x][pos_y] = '3';
                break;
            case BombermanState.PlayerAction.GoUp:
                board[pos_x - 1][pos_y] = 'B';
                board[pos_x][pos_y] = '_';
                break;
            case BombermanState.PlayerAction.GoDown:
                board[pos_x + 1][pos_y] = 'B';
                board[pos_x][pos_y] = '_';
                break;
            case BombermanState.PlayerAction.GoLeft:
                board[pos_x][pos_y - 1] = 'B';
                board[pos_x][pos_y] = '_';
                break;
            case BombermanState.PlayerAction.GoRight:
                board[pos_x][pos_y + 1] = 'B';
                board[pos_x][pos_y] = '_';
                break;
        }
    }

    public void ShowBoard()
    {
        string b = "";
        string bomb = "";
        for (int row = 0; row < 15; row++)
        {
            for (int col = 0; col < 25; col++)
            {
                b += board[row][col];
                bomb += bombBoard[row][col];
            }

            b += "\n";
            bomb += "\n";
        }

        Debug.Log(b);
        Debug.Log(bomb);
    }

    public void RefreshBoard()
    {
        score++;

        for (int row = 0; row < 15; row++)
        {
            for (int col = 0; col < 25; col++)
            {
                switch (bombBoard[row][col])
                {
                    case '3':
                        bombBoard[row][col] = '2';
                        break;
                    case '2':
                        bombBoard[row][col] = '1';
                        break;
                    case '1':
                        bombBoard[row][col] = '_';
                        Explosion(row, col);
                        break;
                }
            }
        }
    }

    public void RefreshBoardUnity()
    {
        for (int i = 0; i < 15; i++)
        for (int j = 0; j < 25; j++)
        {
            if (board[i][j] != 'M')
                board[i][j] = '_';
        }

        foreach (Transform player in GameInitialization.instance.playerList)
        {
            (int x, int z) = GetPosition(player.transform.position);
            board[x][z] = player.name[0];
        }

        foreach (Transform bloc in GameInitialization.instance.blocList)
        {
            (int x, int z) = GetPosition(bloc.transform.position);
            board[x][z] = '0';
        }
    }

    private (int, int) GetPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(-position.z) + 1;
        int y = Mathf.RoundToInt(position.x) + 1;
        return (x, y);
    }

    private bool inBoard(int row, int col)
    {
        return row is >= 0 and < 15 && col is >= 0 and < 25;
    }

    private void Explosion(int row, int col)
    {
        int rayon = 2;

        // Le personnage présent sur la bombe est mort
        if (!ExplosionCase(row, col, board))
            return;

        bool canBlastUp = true;
        bool canBlastDown = true;
        bool canBlastLeft = true;
        bool canBlastRight = true;


        for (int i = 0; i < rayon; i++)
        {
            if (canBlastUp)
                canBlastUp = ExplosionCase(row - 1 - i, col, board);
            if (canBlastDown)
                canBlastDown = ExplosionCase(row + 1 + i, col, board);
            if (canBlastLeft)
                canBlastLeft = ExplosionCase(row, col - 1 - i, board);
            if (canBlastRight)
                canBlastRight = ExplosionCase(row, col + 1 + i, board);
        }
    }

    private bool ExplosionCase(int row, int col, char[][] board)
    {
        if (!inBoard(row, col))
            return false;

        char caseBoard = board[row][col];

        // Explosion mur incassable
        if (caseBoard == 'M')
            return false;
        // Explosion mur cassable
        if (caseBoard == '0')
        {
            score += 10;
            board[row][col] = '_';
            return false;
        }

        // Explosion Personnage
        if (caseBoard is 'A' or 'B' or 'C' or 'D')
        {
            board[row][col] = '_';
            if (caseBoard == 'B')
                Gameover(false);
            else
            {
                Gameover(true);
            }

            return false;
        }

        return true;
        // Sinon on continue le rayon de l'explosion
    }

    private void Gameover(bool win)
    {
        isGameOver = true;
        if (!win)
        {
            score += -100;
        }
        else
        {
            score += 100;
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public char[][] GetBoard()
    {
        return this.board;
    }
}
