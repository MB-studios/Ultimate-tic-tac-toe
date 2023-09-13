using UnityEngine;

public class Game : MonoBehaviour
{
    public int currentPlayer;
    public int[] playerMoves = new int[9];
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

    public Sprite GetPlayerSquareSprite(int squareNumber)
    {
        if (playerMoves[squareNumber] == 1)
        {
            return player1Square;
        }
        else if (playerMoves[squareNumber] == 2)
        {
            return player2Square;
        }
        else
        {
            return null;
        }
    }

    public bool SquareIsActive(int squareNumber)
    {
        return winner == 0 && playerMoves[squareNumber] == 0;
    }

    public void PlayerMove(int squareNumber)
    {
        playerMoves[squareNumber] = currentPlayer;
        currentPlayer = currentPlayer == 1 ? 2 : 1;
        winner = CheckWin(playerMoves, squareNumber);
        if (winner != 0)
        {
            playerWins[winner - 1]++;
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
        playerMoves = new int[9];
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
