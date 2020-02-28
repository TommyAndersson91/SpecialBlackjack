using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText = default;
    [SerializeField]
    private TextMeshProUGUI scoreText2 = default;
    [SerializeField]
    private GameObject Player1TrumpCards = default;
    [SerializeField]
    private GameObject Player2TrumpCards = default;
    [SerializeField]
    private GameObject player1Hand = default;
    [SerializeField]
    private GameObject player2Hand = default;

    public void SetupPanel(Player player)
    {
        ScoreChanged(player.HandValue, player.PlayerIndex, player.PlayerName);
        player.onAdd += ScoreChanged;
        player.onWin += WinsChanged;
        if (player.PlayerIndex == 0)
        {
            player.PlayerHand = player1Hand;
        }
        else
        {
            player.PlayerHand = player2Hand;
            // gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerHand = player2Hand;
        }

        
        
    }
    public GameObject GetTrumpCardPanel(int playerIndex)
    {
        if (playerIndex == 0)
        {
           return Player1TrumpCards;
        }
        else
        {
            return Player2TrumpCards;
        }
    }

    public GameObject GetPlayerHand(int playerIndex)
    {
        if (playerIndex == 0)
        {
            return player1Hand;
        }
        else
        {
            return player2Hand;
        }
    }

    private void WinsChanged()
    {
       
        //TODO FIX: NOT WORKING ATM 
        Debug.Log("WinsChanged Called");
        // gameObject.GetComponent<GameLogic>().SetWinnerText();
        // Debug.Log(Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().RoundScoreText.text);
        //  gameObject.GetComponent<GameLogic>().winnerText.text = "Test";
        // Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().RoundScoreText.text =
        //   =  "Wins \n" + Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[0].PlayerName + ": " + Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[0].PlayerWins +
        //  "\n" + Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerName + ": " + Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerWins;
    }

    private void ScoreChanged(int handValue, int playerIndex, string playerName)
    {
        if (playerIndex == 0)
        {
            scoreText.text = playerName + ": " + handValue.ToString() + " /21"; 
        }
        // else
        // {
        //     if (Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().HiddenValue != 0)
        //     {
        //         scoreText2.text = playerName + ": ? + " + Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().GetHiddenValue() + " /21";
        //     }
        //     else
        //     {
                scoreText2.text = playerName + ": ? /21";
        //     }
        // }
    }
    public void SetPlayer2ScoreText(string playerName, int handValue)
    {
        scoreText2.text = playerName + ": " + handValue.ToString() + " /21";
    }

}
    
