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
    public int totalGames = 0;
    public float[] winRates = { 0.0f, 0.0f };
    public Sprite[] playerSquareSprites = new Sprite[2];
    public Sprite[] playerBoardSprites = new Sprite[2];
    public Sprite[] backgroundSprites = new Sprite[2];
    public bool AITraining = false;
    public AgentManager agentManager;
    public bool debug = false;
    public bool AIDebug = false;

    private float gameWinReawrd = 1.0f;
    private float gameLossReward = -1.0f;
    private float gameBlockReward = 0.5f;
    private float gamePlaceTwoReward = 0.3f;
    private float gamePlaceOneUncontendedReward = 0.05f;
    private float gamePlaceOneContendedReward = 0.01f;
    private float boardLossReward = -0.2f;
    private float boardBlockReward = 0.05f;
    private float boardPlaceTwoReward = 0.03f;
    private float boardPlaceOneUncontendedReward = 0.005f;


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
        playerMoves[boardNumber][squareNumber] = currentPlayer;
        boardMoves[boardNumber]++;
        currentPlayer = currentPlayer == (int)Player.X ? (int)Player.O : (int)Player.X;
        int boardWon = CheckWin(playerMoves[boardNumber], squareNumber);
        int gameWon = (int)Player.none;
        float[] rewards = { 0.0f, 0.0f };
        if (boardWon != (int)Player.none)
        {
            boardWins[boardNumber] = boardWon;
            gameWon = CheckWin(boardWins, boardNumber);
            if (gameWon != (int)Player.none)
            {
                winner = gameWon;
                playerWins[gameWon]++;
            }

        }
        else if (boardMoves[boardNumber] == 9)
        {
            boardWins[boardNumber] = (int)Player.draw;
        }
        SetActiveBoardsAndSquares(squareNumber);
        if (AITraining)
        {
            if (AIDebug) Debug.Log("Calculating rewards");
            int firstInRow = squareNumber - squareNumber % 3;
            int firstInCol = squareNumber % 3;
            int player = playerMoves[boardNumber][squareNumber];
            int opponent = player == (int)Player.X ? (int)Player.O : (int)Player.X;

            int[] row = { playerMoves[boardNumber][firstInRow], playerMoves[boardNumber][firstInRow + 1], playerMoves[boardNumber][firstInRow + 2] };
            int[] col = { playerMoves[boardNumber][firstInCol], playerMoves[boardNumber][firstInCol + 3], playerMoves[boardNumber][firstInCol + 6] };
            int[] downDia = null;
            int[] upDia = null;
            if (squareNumber == 0 || squareNumber == 4 || squareNumber == 8)
            {
                downDia = new int[] { playerMoves[boardNumber][0], playerMoves[boardNumber][4], playerMoves[boardNumber][8] };
            }
            if (squareNumber == 2 || squareNumber == 4 || squareNumber == 6)
            {
                upDia = new int[] { playerMoves[boardNumber][2], playerMoves[boardNumber][4], playerMoves[boardNumber][6] };
            }
            //if (AIDebug) Debug.Log("Row: " + row[0] + ", " + row[1] + ", " + row[2]);
            //if (AIDebug) Debug.Log("Col: " + col[0] + ", " + col[1] + ", " + col[2]);
            //if (AIDebug && downDia != null) Debug.Log("DownDia: " + downDia[0] + ", " + downDia[1] + ", " + downDia[2]);
            //if (AIDebug && upDia != null) Debug.Log("UpDia: " + upDia[0] + ", " + upDia[1] + ", " + upDia[2]);

            int[] rowMoves = new int[2];
            int[] colMoves = new int[2];
            int[] downDiaMoves = new int[2];
            int[] upDiaMoves = new int[2];


            for (int i = 0; i < 3; i++)
            {
                if (row[i] != (int)Player.none) rowMoves[row[i]]++;
                if (col[i] != (int)Player.none) colMoves[col[i]]++;
                if (downDia != null && downDia[i] != (int)Player.none) downDiaMoves[downDia[i]]++;
                if (upDia != null && upDia[i] != (int)Player.none) upDiaMoves[upDia[i]]++;
            }

            //if (AIDebug) Debug.Log("RowMoves: " + rowMoves[0] + ", " + rowMoves[1]);
            //if (AIDebug) Debug.Log("ColMoves: " + colMoves[0] + ", " + colMoves[1]);
            //if (AIDebug && downDia != null) Debug.Log("DownDiaMoves: " + downDiaMoves[0] + ", " + downDiaMoves[1]);
            //if (AIDebug && upDia != null) Debug.Log("UpDiaMoves: " + upDiaMoves[0] + ", " + upDiaMoves[1]);

            if (gameWon == player)
            {
                rewards[player] = gameWinReawrd;
                rewards[opponent] = gameLossReward;
                if (AIDebug) Debug.Log("Game won, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
            }
            else if (boardWon == player)
            {
                int firstInBoardRow = boardNumber - boardNumber % 3;
                int firstInBoardCol = boardNumber % 3;
                int[] boardRow = { boardWins[firstInBoardRow], boardWins[firstInBoardRow + 1], boardWins[firstInBoardRow + 2] };
                int[] boardCol = { boardWins[firstInBoardCol], boardWins[firstInBoardCol + 3], boardWins[firstInBoardCol + 6] };

                int[] boardDownDia = null;
                int[] boardUpDia = null;
                if (boardNumber == 0 || boardNumber == 4 || boardNumber == 8)
                {
                    boardDownDia = new int[] { boardWins[0], boardWins[4], boardWins[8] };
                }
                if (boardNumber == 2 || boardNumber == 4 || boardNumber == 6)
                {
                    boardUpDia = new int[] { boardWins[2], boardWins[4], boardWins[6] };
                }

                int[] boardRowMoves = new int[2];
                int[] boardColMoves = new int[2];
                int[] boardDownDiaMoves = new int[2];
                int[] boardUpDiaMoves = new int[2];

                for (int i = 0; i < 3; i++)
                {
                    if (boardRow[i] != (int)Player.none && boardRow[i] != (int)Player.draw)
                    {
                        boardRowMoves[boardRow[i]]++;
                    }
                    if (boardCol[i] != (int)Player.none && boardCol[i] != (int)Player.draw)
                    {

                        boardColMoves[boardCol[i]]++;
                    }
                    if (boardDownDia != null && boardDownDia[i] != (int)Player.none && boardDownDia[i] != (int)Player.draw) boardDownDiaMoves[boardDownDia[i]]++;
                    if (boardUpDia != null && boardUpDia[i] != (int)Player.none && boardUpDia[i] != (int)Player.draw) boardUpDiaMoves[boardUpDia[i]]++;
                }

                if (boardRowMoves[opponent] == 2)
                {
                    rewards[player] += gameBlockReward;
                    if (AIDebug) Debug.Log("Blocked opponents boardRow, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else if (boardRowMoves[player] == 2 && boardRowMoves[opponent] == 0)
                {
                    rewards[player] += gamePlaceTwoReward;
                    if (AIDebug) Debug.Log("Placed two in a boardRow on an empty boardRow, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else if (boardRowMoves[player] == 1 && boardRowMoves[opponent] == 0)
                {
                    rewards[player] += gamePlaceOneUncontendedReward;
                    if (AIDebug) Debug.Log("Placed one on an empty boardRow, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else
                {
                    rewards[player] += gamePlaceOneContendedReward;
                    if (AIDebug) Debug.Log("Board won on occupied row, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }

                if (boardColMoves[opponent] == 2)
                {
                    rewards[player] += gameBlockReward;
                    if (AIDebug) Debug.Log("Blocked opponents boardCol, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else if (boardColMoves[player] == 2 && boardColMoves[opponent] == 0)
                {
                    rewards[player] += gamePlaceTwoReward;
                    if (AIDebug) Debug.Log("Placed two in a boardCol on an empty boardCol, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else if (boardColMoves[player] == 1 && boardColMoves[opponent] == 0)
                {
                    rewards[player] += gamePlaceOneUncontendedReward;
                    if (AIDebug) Debug.Log("Placed one on an empty boardCol, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else
                {
                    rewards[player] += gamePlaceOneContendedReward;
                    if (AIDebug) Debug.Log("Board won on occupied col, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }

                if (boardDownDia != null)
                {
                    if (boardDownDiaMoves[opponent] == 2)
                    {
                        rewards[player] += gameBlockReward;
                        if (AIDebug) Debug.Log("Blocked opponents boardDownDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (boardDownDiaMoves[player] == 2 && boardDownDiaMoves[opponent] == 0)
                    {
                        rewards[player] += gamePlaceTwoReward;
                        if (AIDebug) Debug.Log("Placed two in a boardDownDia on an empty boardDownDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (boardDownDiaMoves[player] == 1 && boardDownDiaMoves[opponent] == 0)
                    {
                        rewards[player] += gamePlaceOneUncontendedReward;
                        if (AIDebug) Debug.Log("Placed one on an empty boardDownDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else
                    {
                        rewards[player] += gamePlaceOneContendedReward;
                        if (AIDebug) Debug.Log("Board won on occupied downDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                }

                if (boardUpDia != null)
                {
                    if (boardUpDiaMoves[opponent] == 2)
                    {
                        rewards[player] += gameBlockReward;
                        if (AIDebug) Debug.Log("Blocked opponents boardUpDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (boardUpDiaMoves[player] == 2 && boardUpDiaMoves[opponent] == 0)
                    {
                        rewards[player] += gamePlaceTwoReward;
                        if (AIDebug) Debug.Log("Placed two in a boardUpDia on an empty boardUpDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (boardUpDiaMoves[player] == 1 && boardUpDiaMoves[opponent] == 0)
                    {
                        rewards[player] += gamePlaceOneUncontendedReward;
                        if (AIDebug) Debug.Log("Placed one on an empty boardUpDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else
                    {
                        rewards[player] += gamePlaceOneContendedReward;
                        if (AIDebug) Debug.Log("Board won on occupied upDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                }

                rewards[opponent] += boardLossReward;
            }
            else
            {
                if (rowMoves[opponent] == 2)
                {
                    rewards[player] += boardBlockReward;
                    if (AIDebug) Debug.Log("Blocked opponents row, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else if (rowMoves[player] == 2 && rowMoves[opponent] == 0)
                {
                    rewards[player] += boardPlaceTwoReward;
                    if (AIDebug) Debug.Log("Placed two in a row on an empty row, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else if (rowMoves[player] == 1 && rowMoves[opponent] == 0)
                {
                    rewards[player] += boardPlaceOneUncontendedReward;
                    if (AIDebug) Debug.Log("Placed one on an empty row, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }

                if (colMoves[opponent] == 2)
                {
                    rewards[player] += boardBlockReward;
                    if (AIDebug) Debug.Log("Blocked opponents col, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else if (colMoves[player] == 2 && colMoves[opponent] == 0)
                {
                    rewards[player] += boardPlaceTwoReward;
                    if (AIDebug) Debug.Log("Placed two in a col on an empty col, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }
                else if (colMoves[player] == 1 && colMoves[opponent] == 0)
                {
                    rewards[player] += boardPlaceOneUncontendedReward;
                    if (AIDebug) Debug.Log("Placed one on an empty col, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                }

                if (downDia != null)
                {
                    if (downDiaMoves[opponent] == 2)
                    {
                        rewards[player] += boardBlockReward;
                        if (AIDebug) Debug.Log("Blocked opponents downDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (downDiaMoves[player] == 2 && downDiaMoves[opponent] == 0)
                    {
                        rewards[player] += boardPlaceTwoReward;
                        if (AIDebug) Debug.Log("Placed two in a downDia on an empty downDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (downDiaMoves[player] == 1 && downDiaMoves[opponent] == 0)
                    {
                        rewards[player] += boardPlaceOneUncontendedReward;
                        if (AIDebug) Debug.Log("Placed one on an empty downDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                }


                if (upDia != null)
                {
                    if (upDiaMoves[opponent] == 2)
                    {
                        rewards[player] += boardBlockReward;
                        if (AIDebug) Debug.Log("Blocked opponents upDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (upDiaMoves[player] == 2 && upDiaMoves[opponent] == 0)
                    {
                        rewards[player] += boardPlaceTwoReward;
                        if (AIDebug) Debug.Log("Placed two in a upDia on an empty upDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (upDiaMoves[player] == 1 && upDiaMoves[opponent] == 0)
                    {
                        rewards[player] += boardPlaceOneUncontendedReward;
                        if (AIDebug) Debug.Log("Placed one on an empty upDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                }
            }

            if (AIDebug) Debug.Log("Final rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
            agentManager.AddRewards(rewards[(int)Player.X], rewards[(int)Player.O]);


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
            else if (boardWon != (int)Player.none)
            {
                agentManager.EndEpisode();
            }
        }

    }


    public void SetActiveBoardsAndSquares(int squareNumber)
    {
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
        totalGames++;
        winRates[0] = (float)playerWins[0] / totalGames;
        winRates[1] = (float)playerWins[1] / totalGames;
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
