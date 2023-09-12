using UnityEngine;

public class Game : MonoBehaviour
{
    public int currentPlayer;
    public int[][] playerMoves = { new int[9], new int[9], new int[9], new int[9], new int[9], new int[9], new int[9], new int[9], new int[9] };
    public int[] boardWins = new int[9];
    public bool[] activeBoards = new bool[9];
    private int winner = 0;
    public int[] playerWins = { 0, 0 };
    public Sprite player1Square;
    public Sprite player2Square;
    public Sprite player1Board;
    public Sprite player2Board;
    public Sprite backgroundActive;
    public Sprite backgroundInactive;


    void Start()
    {
        NewGame();
    }

    public Sprite GetBoardBackgroundSprite(int subBoardNumber)
    {
        return activeBoards[subBoardNumber] ? backgroundActive : backgroundInactive;
    }

    public Sprite GetPlayerBoardSprite(int subBoardNumber)
    {
        if (boardWins[subBoardNumber] == 1)
        {
            return player1Board;
        }
        else if (boardWins[subBoardNumber] == 2)
        {
            return player2Board;
        }
        else
        {
            return null;
        }
    }

    public Sprite GetPlayerSquareSprite(int subBoardNumber, int squareNumber)
    {
        if (playerMoves[subBoardNumber][squareNumber] == 1)
        {
            return player1Square;
        }
        else if (playerMoves[subBoardNumber][squareNumber] == 2)
        {
            return player2Square;
        }
        else
        {
            return null;
        }
    }

    public bool BoardIsActive(int subBoardNumber)
    {
        return activeBoards[subBoardNumber] && winner == 0;
    }

    public bool SquareIsActive(int subBoardNumber, int squareNumber)
    {
        return activeBoards[subBoardNumber] && playerMoves[subBoardNumber][squareNumber] == 0;
    }

    public void PlayerMove(int boardNumber, int squareNumber)
    {
        playerMoves[boardNumber][squareNumber] = currentPlayer;
        currentPlayer = currentPlayer == 1 ? 2 : 1;
        int boardWon = CheckWin(playerMoves[boardNumber], squareNumber);
        int gameWon = 0;
        if (boardWon != 0)
        {
            boardWins[boardNumber] = boardWon;
            gameWon = CheckWin(boardWins, boardNumber);
            if (gameWon != 0)
            {
                winner = gameWon;
                playerWins[gameWon - 1]++;
            }
        }

        if (gameWon != 0)
        {
            for (int i = 0; i < 9; i++)
            {

                activeBoards[i] = false;
            }
        }
        else
        {
            if (boardWins[squareNumber] != 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (boardWins[i] == 0)
                    {
                        activeBoards[i] = true;
                    }
                    else
                    {
                        activeBoards[i] = false;
                    }

                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    if (i == squareNumber || boardWins[i] != 0)
                    {
                        activeBoards[i] = true;
                    }
                    else
                    {
                        activeBoards[i] = false;
                    }
                }
            }
        }

    }

    public int GetPlayerWins(int player)
    {
        return playerWins[player - 1];
    }

    public Sprite getCurrentPlayerSprite()
    {
        if (winner == 0)
        {
            return currentPlayer == 1 ? player1Board : player2Board;
        }
        else
        {
            return null;
        }

    }

    public void NewGame()
    {
        currentPlayer = Random.Range(1, 3);
        winner = 0;
        for (int i = 0; i < 9; i++)
        {
            boardWins[i] = 0;
            activeBoards[i] = true;
            for (int j = 0; j < 9; j++)
            {
                playerMoves[i][j] = 0;
            }
        }
    }

    private int CheckWin(int[] squares, int squareNumber)
    {
        int firstInRow = squareNumber - squareNumber % 3;
        int firstInCol = squareNumber % 3;

        if (squares[firstInRow] == squares[firstInRow + 1] && squares[firstInRow] == squares[firstInRow + 2])
        {
            return squares[squareNumber];
        }

        if (squares[firstInCol] == squares[firstInCol + 3] && squares[firstInCol] == squares[firstInCol + 6])
        {
            return squares[squareNumber];
        }

        if (squareNumber == 0 || squareNumber == 4 || squareNumber == 8)
        {
            if (squares[0] == squares[4] && squares[0] == squares[8])
            {
                return squares[squareNumber];
            }
        }

        if (squareNumber == 2 || squareNumber == 4 || squareNumber == 6)
        {
            if (squares[2] == squares[4] && squares[2] == squares[6])
            {
                return squares[squareNumber];
            }
        }

        return 0;
    }
}
