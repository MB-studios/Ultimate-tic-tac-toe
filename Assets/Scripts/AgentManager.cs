using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public Game game;
    public UltimateTicTacToeAgent agentX;
    public UltimateTicTacToeAgent agentO;
    public bool playerChanged = true;
    public int lastPlayer = -1;
    private float gameWinReward = 1.0f;
    private float boardBlockReward = 0.5f;
    private float boardPlaceTwoReward = 0.3f;
    private float boardPlaceOneUncontendedReward = 0.05f;
    private float boardPlaceOneContendedReward = 0.01f;
    private float squareBlockReward = 0.05f;
    private float squarePlaceTwoReward = 0.03f;
    private float squarePlaceOneUncontendedReward = 0.005f;
    public bool RewardDebug = false;

    void Start()
    {
        game = gameObject.GetComponent<Game>();
        game.agentManager = this;
        agentX = gameObject.transform.Find("AgentX").gameObject.GetComponent<UltimateTicTacToeAgent>();
        agentO = gameObject.transform.Find("AgentO").gameObject.GetComponent<UltimateTicTacToeAgent>();
        agentX.player = (int)Player.X;
        agentO.player = (int)Player.O;
        agentX.agentManager = this;
        agentO.agentManager = this;
    }


    public double executionTimeAverage = 0;
    private int executionTimeCount = 0;

    private void FixedUpdate()
    {
        double start = Time.realtimeSinceStartupAsDouble;
        if (lastPlayer != game.currentPlayer)
        {
            lastPlayer = game.currentPlayer;
            playerChanged = true;
        }
        if (playerChanged)
        {
            playerChanged = false;
            RequestDecision();
        }
        double end = Time.realtimeSinceStartupAsDouble;
        executionTimeAverage = (executionTimeAverage * executionTimeCount + (end - start)) / (executionTimeCount + 1);
        executionTimeCount++;
    }


    private void RequestDecision()
    {
        if (game.currentPlayer == (int)Player.X && agentX.human == false)
        {
            agentX.RequestDecision();
        }
        else if (game.currentPlayer == (int)Player.O && agentO.human == false)
        {
            agentO.RequestDecision();
        }

    }

    public void AddRewards(float agentXReward, float agentOReward)
    {
        agentX.AddReward(agentXReward);
        agentO.AddReward(agentOReward);
    }

    public void EndEpisode()
    {
        agentX.EndEpisode();
        agentO.EndEpisode();
    }

    public void NewGame()
    {
        playerChanged = true;
        lastPlayer = -1;
        EndEpisode();
    }

    public void CalculateRewards(int boardNumber, int squareNumber, int boardWon, int gameWon)
    {
        if (RewardDebug) Debug.Log("Calculating rewards");
        float[] rewards = { 0.0f, 0.0f };
        int player = game.playerMoves[boardNumber][squareNumber];
        int opponent = player == (int)Player.X ? (int)Player.O : (int)Player.X;
        /*
        int firstInRow = squareNumber - squareNumber % 3;
        int firstInCol = squareNumber % 3;
        

        int[] row = { game.playerMoves[boardNumber][firstInRow], game.playerMoves[boardNumber][firstInRow + 1], game.playerMoves[boardNumber][firstInRow + 2] };
        int[] col = { game.playerMoves[boardNumber][firstInCol], game.playerMoves[boardNumber][firstInCol + 3], game.playerMoves[boardNumber][firstInCol + 6] };
        int[] downDia = null;
        int[] upDia = null;
        if (squareNumber == 0 || squareNumber == 4 || squareNumber == 8)
        {
            downDia = new int[] { game.playerMoves[boardNumber][0], game.playerMoves[boardNumber][4], game.playerMoves[boardNumber][8] };
        }
        if (squareNumber == 2 || squareNumber == 4 || squareNumber == 6)
        {
            upDia = new int[] { game.playerMoves[boardNumber][2], game.playerMoves[boardNumber][4], game.playerMoves[boardNumber][6] };
        }
        //if (RewardDebug) Debug.Log("Row: " + row[0] + ", " + row[1] + ", " + row[2]);
        //if (RewardDebug) Debug.Log("Col: " + col[0] + ", " + col[1] + ", " + col[2]);
        //if (RewardDebug && downDia != null) Debug.Log("DownDia: " + downDia[0] + ", " + downDia[1] + ", " + downDia[2]);
        //if (RewardDebug && upDia != null) Debug.Log("UpDia: " + upDia[0] + ", " + upDia[1] + ", " + upDia[2]);

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

        //if (RewardDebug) Debug.Log("RowMoves: " + rowMoves[0] + ", " + rowMoves[1]);
        //if (RewardDebug) Debug.Log("ColMoves: " + colMoves[0] + ", " + colMoves[1]);
        //if (RewardDebug && downDia != null) Debug.Log("DownDiaMoves: " + downDiaMoves[0] + ", " + downDiaMoves[1]);
        //if (RewardDebug && upDia != null) Debug.Log("UpDiaMoves: " + upDiaMoves[0] + ", " + upDiaMoves[1]);
*/
        if (gameWon == player)
        {
            rewards[player] = gameWinReward;
            rewards[opponent] = -gameWinReward;
            if (RewardDebug) Debug.Log("Game won, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
        }
        /*
                else if (boardWon == player)
                {
                    int firstInBoardRow = boardNumber - boardNumber % 3;
                    int firstInBoardCol = boardNumber % 3;
                    int[] boardRow = { game.boardWins[firstInBoardRow], game.boardWins[firstInBoardRow + 1], game.boardWins[firstInBoardRow + 2] };
                    int[] boardCol = { game.boardWins[firstInBoardCol], game.boardWins[firstInBoardCol + 3], game.boardWins[firstInBoardCol + 6] };

                    int[] boardDownDia = null;
                    int[] boardUpDia = null;
                    if (boardNumber == 0 || boardNumber == 4 || boardNumber == 8)
                    {
                        boardDownDia = new int[] { game.boardWins[0], game.boardWins[4], game.boardWins[8] };
                    }
                    if (boardNumber == 2 || boardNumber == 4 || boardNumber == 6)
                    {
                        boardUpDia = new int[] { game.boardWins[2], game.boardWins[4], game.boardWins[6] };
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
                        rewards[player] += boardBlockReward;
                        rewards[opponent] -= boardBlockReward;
                        if (RewardDebug) Debug.Log("Blocked opponents boardRow, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (boardRowMoves[player] == 2 && boardRowMoves[opponent] == 0)
                    {
                        rewards[player] += boardPlaceTwoReward;
                        rewards[opponent] -= boardPlaceTwoReward;
                        if (RewardDebug) Debug.Log("Placed two in a boardRow on an empty boardRow, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (boardRowMoves[player] == 1 && boardRowMoves[opponent] == 0)
                    {
                        rewards[player] += boardPlaceOneUncontendedReward;
                        rewards[opponent] -= boardPlaceOneUncontendedReward;
                        if (RewardDebug) Debug.Log("Placed one on an empty boardRow, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else
                    {
                        rewards[player] += boardPlaceOneContendedReward;
                        rewards[opponent] -= boardPlaceOneContendedReward;
                        if (RewardDebug) Debug.Log("Board won on occupied row, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }

                    if (boardColMoves[opponent] == 2)
                    {
                        rewards[player] += boardBlockReward;
                        rewards[opponent] -= boardBlockReward;
                        if (RewardDebug) Debug.Log("Blocked opponents boardCol, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (boardColMoves[player] == 2 && boardColMoves[opponent] == 0)
                    {
                        rewards[player] += boardPlaceTwoReward;
                        rewards[opponent] -= boardPlaceTwoReward;
                        if (RewardDebug) Debug.Log("Placed two in a boardCol on an empty boardCol, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (boardColMoves[player] == 1 && boardColMoves[opponent] == 0)
                    {
                        rewards[player] += boardPlaceOneUncontendedReward;
                        rewards[opponent] -= boardPlaceOneUncontendedReward;
                        if (RewardDebug) Debug.Log("Placed one on an empty boardCol, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else
                    {
                        rewards[player] += boardPlaceOneContendedReward;
                        rewards[opponent] -= boardPlaceOneContendedReward;
                        if (RewardDebug) Debug.Log("Board won on occupied col, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }

                    if (boardDownDia != null)
                    {
                        if (boardDownDiaMoves[opponent] == 2)
                        {
                            rewards[player] += boardBlockReward;
                            rewards[opponent] -= boardBlockReward;
                            if (RewardDebug) Debug.Log("Blocked opponents boardDownDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else if (boardDownDiaMoves[player] == 2 && boardDownDiaMoves[opponent] == 0)
                        {
                            rewards[player] += boardPlaceTwoReward;
                            rewards[opponent] -= boardPlaceTwoReward;
                            if (RewardDebug) Debug.Log("Placed two in a boardDownDia on an empty boardDownDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else if (boardDownDiaMoves[player] == 1 && boardDownDiaMoves[opponent] == 0)
                        {
                            rewards[player] += boardPlaceOneUncontendedReward;
                            rewards[opponent] -= boardPlaceOneUncontendedReward;
                            if (RewardDebug) Debug.Log("Placed one on an empty boardDownDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else
                        {
                            rewards[player] += boardPlaceOneContendedReward;
                            rewards[opponent] -= boardPlaceOneContendedReward;
                            if (RewardDebug) Debug.Log("Board won on occupied downDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                    }

                    if (boardUpDia != null)
                    {
                        if (boardUpDiaMoves[opponent] == 2)
                        {
                            rewards[player] += boardBlockReward;
                            rewards[opponent] -= boardBlockReward;
                            if (RewardDebug) Debug.Log("Blocked opponents boardUpDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else if (boardUpDiaMoves[player] == 2 && boardUpDiaMoves[opponent] == 0)
                        {
                            rewards[player] += boardPlaceTwoReward;
                            rewards[opponent] -= boardPlaceTwoReward;
                            if (RewardDebug) Debug.Log("Placed two in a boardUpDia on an empty boardUpDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else if (boardUpDiaMoves[player] == 1 && boardUpDiaMoves[opponent] == 0)
                        {
                            rewards[player] += boardPlaceOneUncontendedReward;
                            rewards[opponent] -= boardPlaceOneUncontendedReward;
                            if (RewardDebug) Debug.Log("Placed one on an empty boardUpDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else
                        {
                            rewards[player] += boardPlaceOneContendedReward;
                            rewards[opponent] -= boardPlaceOneContendedReward;
                            if (RewardDebug) Debug.Log("Board won on occupied upDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                    }
                }
                else
                {
                    if (rowMoves[opponent] == 2)
                    {
                        rewards[player] += squareBlockReward;
                        rewards[opponent] -= squareBlockReward;
                        if (RewardDebug) Debug.Log("Blocked opponents row, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (rowMoves[player] == 2 && rowMoves[opponent] == 0)
                    {
                        rewards[player] += squarePlaceTwoReward;
                        rewards[opponent] -= squarePlaceTwoReward;
                        if (RewardDebug) Debug.Log("Placed two in a row on an empty row, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (rowMoves[player] == 1 && rowMoves[opponent] == 0)
                    {
                        rewards[player] += squarePlaceOneUncontendedReward;
                        rewards[opponent] -= squarePlaceOneUncontendedReward;
                        if (RewardDebug) Debug.Log("Placed one on an empty row, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }

                    if (colMoves[opponent] == 2)
                    {
                        rewards[player] += squareBlockReward;
                        rewards[opponent] -= squareBlockReward;
                        if (RewardDebug) Debug.Log("Blocked opponents col, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (colMoves[player] == 2 && colMoves[opponent] == 0)
                    {
                        rewards[player] += squarePlaceTwoReward;
                        rewards[opponent] -= squarePlaceTwoReward;
                        if (RewardDebug) Debug.Log("Placed two in a col on an empty col, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }
                    else if (colMoves[player] == 1 && colMoves[opponent] == 0)
                    {
                        rewards[player] += squarePlaceOneUncontendedReward;
                        rewards[opponent] -= squarePlaceOneUncontendedReward;
                        if (RewardDebug) Debug.Log("Placed one on an empty col, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                    }

                    if (downDia != null)
                    {
                        if (downDiaMoves[opponent] == 2)
                        {
                            rewards[player] += squareBlockReward;
                            rewards[opponent] -= squareBlockReward;
                            if (RewardDebug) Debug.Log("Blocked opponents downDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else if (downDiaMoves[player] == 2 && downDiaMoves[opponent] == 0)
                        {
                            rewards[player] += squarePlaceTwoReward;
                            rewards[opponent] -= squarePlaceTwoReward;
                            if (RewardDebug) Debug.Log("Placed two in a downDia on an empty downDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else if (downDiaMoves[player] == 1 && downDiaMoves[opponent] == 0)
                        {
                            rewards[player] += squarePlaceOneUncontendedReward;
                            rewards[opponent] -= squarePlaceOneUncontendedReward;
                            if (RewardDebug) Debug.Log("Placed one on an empty downDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                    }


                    if (upDia != null)
                    {
                        if (upDiaMoves[opponent] == 2)
                        {
                            rewards[player] += squareBlockReward;
                            rewards[opponent] -= squareBlockReward;
                            if (RewardDebug) Debug.Log("Blocked opponents upDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else if (upDiaMoves[player] == 2 && upDiaMoves[opponent] == 0)
                        {
                            rewards[player] += squarePlaceTwoReward;
                            rewards[opponent] -= squarePlaceTwoReward;
                            if (RewardDebug) Debug.Log("Placed two in a upDia on an empty upDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                        else if (upDiaMoves[player] == 1 && upDiaMoves[opponent] == 0)
                        {
                            rewards[player] += squarePlaceOneUncontendedReward;
                            rewards[opponent] -= squarePlaceOneUncontendedReward;
                            if (RewardDebug) Debug.Log("Placed one on an empty upDia, rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
                        }
                    }
                }

                if (RewardDebug) Debug.Log("Final rewards: " + rewards[(int)Player.X] + ", " + rewards[(int)Player.O]);
             */

        AddRewards(rewards[(int)Player.X], rewards[(int)Player.O]);


        bool movesAvaiable = false;
        for (int b = 0; b < 9; b++)
        {
            if (game.activeBoards[b] == 1)
            {
                movesAvaiable = true;
                break;
            }
        }
        if (!movesAvaiable)
        {
            game.NewGame();
            NewGame();
        }
        else if (boardWon != (int)Player.none)
        {
            //EndEpisode();
        }
    }

    public float[] GetObservations(int player)
    {
        float[] observations = new float[81];
        for (int b = 0; b < 9; b++)
        {
            for (int s = 0; s < 9; s++)
            {
                if (game.playerMoves[b][s] == (int)Player.none)
                {
                    observations[b * 9 + s] = 0;
                }
                else if (game.playerMoves[b][s] == player)
                {
                    observations[b * 9 + s] = 1;
                }
                else
                {
                    observations[b * 9 + s] = -1;
                }
            }
        }
        return observations;
    }

}
