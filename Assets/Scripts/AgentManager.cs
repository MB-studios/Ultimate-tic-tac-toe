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
    void Start()
    {
        game = gameObject.GetComponent<Game>();
        game.agentManager = this;
        agentX = gameObject.transform.Find("AgentX").gameObject.GetComponent<UltimateTicTacToeAgent>();
        agentO = gameObject.transform.Find("AgentO").gameObject.GetComponent<UltimateTicTacToeAgent>();
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
}
