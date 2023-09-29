using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private char[][] board = new char[25][];
    public char[][] bombBoard = new char[15][];

    private Time timer;

    private bool isGameOver = false;

    public int win = 0;
    
    public GameState()
    {
        this.board = GameInitialization.map;
        for (int i = 0; i < 15; i++)
        {
            bombBoard[i] = new char[25];
        }
        this.timer = null;
    }
    
    public GameState(Time timer)
    {
        this.board = GameInitialization.map;
        this.timer = timer;
    }

    private (int, int)  GetMCTSPosition()
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

    public void PlayAction(BombermanState.PlayerAction action)
    {
        (int pos_x,int pos_y) = GetMCTSPosition();
        switch (action)
        {
            case BombermanState.PlayerAction.DoNothing:
                break;
            case BombermanState.PlayerAction.PutBomb:
                bombBoard[pos_x][pos_y] = '3';
                break;
            case BombermanState.PlayerAction.GoUp:
                board[pos_x - 1][pos_y] = 'B';
                break;
            case BombermanState.PlayerAction.GoDown:
                board[pos_x + 1][pos_y] = 'B';
                break;
            case BombermanState.PlayerAction.GoLeft:
                board[pos_x][pos_y - 1] = 'B';
                break;
            case BombermanState.PlayerAction.GoRight:
                board[pos_x][pos_y + 1] = 'B';
                break;
        }
        board[pos_x][pos_y] = 'L';
    }
    
    public void RefreshBoard()
    {
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
                        bombBoard[row][col] = '0';
                        Explosion(row, col);
                        break;
                }
            }
        }
    }
    
    private bool inBoard(int row, int col)
    {
        return row is >= 0 and < 15 && col is >= 0 and < 25;
    }

    private void Explosion(int row, int col)
    {
        int rayon = 2;

        // Le personnage présent sur la bombe est mort
        if (!ExplosionCase(row, col, rayon, board))
            return;

        bool canBlastUp = true;
        bool canBlastDown = true;
        bool canBlastLeft = true;
        bool canBlastRight = true;
        
        
        for (int i = 0; i < rayon; i++)
        {
            if (canBlastUp)
                canBlastUp = ExplosionCase(row - 1 - i, col, rayon, board);
            if (canBlastDown)
                canBlastDown = ExplosionCase(row + 1 + i, col, rayon, board);
            if (canBlastLeft)
                canBlastLeft = ExplosionCase(row, col - 1 - i, rayon, board);
            if (canBlastRight)
                canBlastRight = ExplosionCase(row, col + 1 + i, rayon, board);
        }
    }

    private bool ExplosionCase(int row, int col, int rayon, char[][] board)
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
            board[row][col] = 'L';
            return false;
        }
        // Explosion Personnage
        if (caseBoard == 'A' || caseBoard == 'B' || caseBoard == 'C' || caseBoard == 'D')
        {
            Gameover();
            return false;
        }

        return true;
        // Sinon on continue le rayon de l'explosion
    }

    private void Gameover()
    {
        isGameOver = true;
        if (GetMCTSPosition() == (-1, -1))
            win = -10;
        else
        {
            win = 10;
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public Time GetTime()
    {
        return this.timer;
    }

    public char[][] GetBoard()
    {
        return this.board;
    }

    public GameState Copy()
    {
        GameState copy = new GameState();
        copy.board = this.board;
        copy.bombBoard = this.bombBoard;
        copy.timer = this.timer;

        return copy;
    }
}
