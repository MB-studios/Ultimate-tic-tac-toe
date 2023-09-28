using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using System.Linq.Expressions;

public class UltimateTicTacToeAgent : Agent
{
    public Game game;
    public AgentManager agentManager;
    public int player;
    public bool human = false;
    void Start()
    {
        game = gameObject.GetComponentInParent<Game>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentManager.GetObservations(player));

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int subBoardNumber = actions.DiscreteActions[0] / 9;
        int squareNumber = actions.DiscreteActions[0] % 9;
        game.PlayerMove(subBoardNumber, squareNumber);
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        for (int a = 0; a < 81; a++)
        {
            actionMask.SetActionEnabled(0, a, game.activeSquares[a] == 1);
        }
    }
}
