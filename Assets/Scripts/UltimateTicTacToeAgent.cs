using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;

public class UltimateTicTacToeAgent : Agent
{
    public Game game;
    void Start()
    {
        game = gameObject.GetComponentInParent<Game>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        for (int b = 0; b < 9; b++)
        {
            for (int s = 0; s < 9; s++)
            {
                sensor.AddObservation(game.playerMoves[b][s]);
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int subBoardNumber = actions.DiscreteActions[0] / 9;
        int squareNumber = actions.DiscreteActions[0] % 9;
        game.PlayerMove(subBoardNumber, squareNumber);
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        int counter = 0;
        for (int b = 0; b < 9; b++)
        {
            for (int s = 0; s < 9; s++)
            {
                actionMask.SetActionEnabled(0, counter++, game.activeSquares[b * 9 + s] == 1);
            }
        }
    }
}
