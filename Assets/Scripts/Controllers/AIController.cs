using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AIController : MonoBehaviour
{

    public void PlayAI()
    {
        PlayerList.GetPlayers()[0].PlayerWins = 0;
        PlayerList.GetPlayers()[1].PlayerWins = 0;

        PlayerList.GetPlayers()[1].PlayerName = "MasterBot";
        GameLogic.PlayingAgainstAI = !GameLogic.PlayingAgainstAI;
        if (!GameLogic.PlayingAgainstAI)
        {
            PlayerList.GetPlayers()[1].PlayerName = "Player 2";
        }
    }

    public void CheckAITurn(int roundCounter)
    {
        if (PlayerList.GetPlayers()[1].IsPlayersTurn && roundCounter >= 4 && GameLogic.PlayingAgainstAI)
        {
            StartCoroutine(gameObject.GetComponent<GameLogic>().PlayAITUrn());
            return;
        }
    }

    public bool FiftyPercent(System.Random random)
    {
        return random.Next() > (Int32.MaxValue / 2);
        // Next() returns an int in the range [0..Int32.MaxValue]
    }

}
