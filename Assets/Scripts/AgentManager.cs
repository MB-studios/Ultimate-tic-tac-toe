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
        game.AITraining = true;
        game.agentManager = this;
        agentX = gameObject.transform.Find("AgentX").gameObject.GetComponent<UltimateTicTacToeAgent>();
        agentO = gameObject.transform.Find("AgentO").gameObject.GetComponent<UltimateTicTacToeAgent>();
    }

    private void FixedUpdate()
    {
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
    }


    private void RequestDecision()
    {
        if (game.currentPlayer == (int)Player.X)
        {
            agentX.RequestDecision();
        }
        else
        {
            agentO.RequestDecision();
        }

    }

    public void GiveRewards(float agentXReward, float agentOReward)
    {
        agentX.AddReward(agentXReward);
        agentO.AddReward(agentOReward);
    }

    public void NewGame()
    {
        playerChanged = true;
        lastPlayer = -1;
        agentX.EndEpisode();
        agentO.EndEpisode();
    }
}
