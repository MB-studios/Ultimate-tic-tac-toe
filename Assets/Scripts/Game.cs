using UnityEngine;

public enum Player
{
    none = -1,
    X = 0,
    O = 1,
    draw = 2
}

public class Game : MonoBehaviour
{
    public int currentPlayer;
    public int[][] playerMoves = { new int[9], new int[9], new int[9], new int[9], new int[9], new int[9], new int[9], new int[9], new int[9] };
    public int[] boardWins = new int[9];
    public int[] boardMoves = new int[9];
    public int[] activeBoards = new int[9];
    public int[] activeSquares = new int[81];
    private int winner = (int)Player.none;
    public int[] playerWins = { 0, 0 };
    public Sprite[] playerSquareSprites = new Sprite[2];
    public Sprite[] playerBoardSprites = new Sprite[2];
    public Sprite[] backgroundSprites = new Sprite[2];
    public bool AITraining = false;
    public AgentManager agentManager;
    public bool debug = false;


    void Start()
    {
        NewGame();
    }

    public Sprite GetBoardBackgroundSprite(int subBoardNumber)
    {
        return backgroundSprites[activeBoards[subBoardNumber]];
    }

    public Sprite GetPlayerBoardSprite(int subBoardNumber)
    {
        if (boardWins[subBoardNumber] == (int)Player.none || boardWins[subBoardNumber] == (int)Player.draw)
        {
            return null;
        }
        else
        {
            return playerBoardSprites[boardWins[subBoardNumber]];
        }
    }

    public Sprite GetPlayerSquareSprite(int subBoardNumber, int squareNumber)
    {
        if (playerMoves[subBoardNumber][squareNumber] == (int)Player.none)
        {
            return null;
        }
        else
        {
            return playerSquareSprites[playerMoves[subBoardNumber][squareNumber]];
        }
    }

    public bool BoardIsActive(int subBoardNumber)
    {
        return activeBoards[subBoardNumber] == 1 && winner == (int)Player.none;
    }

    public bool SquareIsActive(int subBoardNumber, int squareNumber)
    {
        return activeSquares[subBoardNumber * 9 + squareNumber] == 1;
    }

    public void PlayerMove(int boardNumber, int squareNumber)
    {
        if (debug) Debug.Log("PlayerMove: " + boardNumber + ", " + squareNumber);
        float agentXreward = 0.0f;
        float agentOReward = 0.0f;
        playerMoves[boardNumber][squareNumber] = currentPlayer;
        boardMoves[boardNumber]++;
        currentPlayer = currentPlayer == (int)Player.X ? (int)Player.O : (int)Player.X;
        int boardWon = CheckWin(playerMoves[boardNumber], squareNumber);
        if (boardWon != (int)Player.none)
        {
            if (AITraining)
            {
                agentXreward = boardWon == (int)Player.X ? 0.2f : -0.1f;
                agentOReward = boardWon == (int)Player.O ? 0.2f : -0.1f;
            }
            boardWins[boardNumber] = boardWon;
            int gameWon = CheckWin(boardWins, boardNumber);
            if (gameWon != (int)Player.none)
            {
                if (AITraining)
                {
                    agentXreward = boardWon == (int)Player.X ? 1.0f : -1.0f;
                    agentOReward = boardWon == (int)Player.O ? 1.0f : -1.0f;
                }
                winner = gameWon;
                playerWins[gameWon]++;
                if (AITraining)
                {
                    NewGame();
                    agentManager.NewGame();
                }
            }

        }
        else if (boardMoves[boardNumber] == 9)
        {
            boardWins[boardNumber] = (int)Player.draw;
        }
        SetActiveBoardsAndSquares(squareNumber);
        if (AITraining)
        {
            bool movesAvaiable = false;
            for (int b = 0; b < 9; b++)
            {
                if (activeBoards[b] == 1)
                {
                    movesAvaiable = true;
                    break;
                }
            }
            if (!movesAvaiable)
            {
                NewGame();
                agentManager.NewGame();
            }
        }
        agentManager.GiveRewards(agentXreward, agentOReward);
    }


    public void SetActiveBoardsAndSquares(int squareNumber)
    {
        // Someone won the game, set all boards and squares to inactive
        if (winner != (int)Player.none)
        {
            if (debug) Debug.Log("Game is won");
            for (int b = 0; b < 9; b++)
            {
                activeBoards[b] = 0;
                for (int s = 0; s < 9; s++)
                {
                    activeSquares[b * 9 + s] = 0;
                }
            }
        }
        else
        {
            if (boardWins[squareNumber] > (int)Player.none)
            {
                if (debug) Debug.Log("The board that is next is already won or is filled up");
                for (int b = 0; b < 9; b++)
                {
                    bool boardIsActive = boardWins[b] <= (int)Player.none;
                    activeBoards[b] = boardIsActive ? 1 : 0;
                    for (int s = 0; s < 9; s++)
                    {
                        activeSquares[b * 9 + s] = boardIsActive && playerMoves[b][s] <= (int)Player.none ? 1 : 0;
                    }
                }
            }
            else
            {
                if (debug) Debug.Log("The next board is not won and not full");
                for (int b = 0; b < 9; b++)
                {
                    if (b == squareNumber)
                    {
                        if (debug) Debug.Log("Setting board " + b + " to active");
                        activeBoards[b] = 1;
                        for (int s = 0; s < 9; s++)
                        {
                            activeSquares[b * 9 + s] = playerMoves[b][s] == (int)Player.none ? 1 : 0;
                            if (debug) Debug.Log("Setting square " + s + " to " + activeSquares[b * 9 + s]);
                        }
                    }
                    else
                    {
                        if (debug) Debug.Log("Setting board " + b + " to inactive");
                        activeBoards[b] = 0;
                        for (int s = 0; s < 9; s++)
                        {
                            activeSquares[b * 9 + s] = 0;
                        }
                    }
                }
            }
        }

    }

    public int GetPlayerWins(int player)
    {
        return playerWins[player];
    }

    public Sprite getCurrentPlayerSprite()
    {
        if (winner != (int)Player.none)
        {
            return null;

        }
        else
        {
            return playerBoardSprites[currentPlayer];
        }

    }

    public void NewGame()
    {
        currentPlayer = Random.Range((int)Player.X, (int)Player.O + 1);
        boardMoves = new int[9];
        winner = (int)Player.none;
        for (int i = 0; i < 9; i++)
        {
            boardWins[i] = (int)Player.none;
            activeBoards[i] = 1;
            for (int j = 0; j < 9; j++)
            {
                playerMoves[i][j] = (int)Player.none;
                activeSquares[i * 9 + j] = 1;
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

        return (int)Player.none;
    }
}