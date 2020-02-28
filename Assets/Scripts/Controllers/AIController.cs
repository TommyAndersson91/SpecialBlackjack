using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AIController : MonoBehaviour
{

    public void PlayAI()
    {
        gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].PlayerWins = 0;
        gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerWins = 0;
        gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerName = "MasterBot";
        gameObject.GetComponent<GameLogic>().PlayingAgainstAI = !gameObject.GetComponent<GameLogic>().PlayingAgainstAI;
        if (!gameObject.GetComponent<GameLogic>().PlayingAgainstAI)
        {
            gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerName = "Player 2";
        }
        gameObject.GetComponent<GameLogic>().UpdateUI();
    }

    public void CheckAITurn(int roundCounter)
    {
        if (gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].IsPlayersTurn && roundCounter >= 4 && gameObject.GetComponent<GameLogic>().PlayingAgainstAI)
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
