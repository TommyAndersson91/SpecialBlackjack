﻿using System.Linq;
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
    private string player2Score;

    public void SetupPanel(Player player)
    {
        ScoreChanged(player.HandValue, player.PlayerIndex, player.PlayerName);
        player.onAdd += ScoreChanged;
        player.onWin += WinsChanged;
        player.onNameChanged += NameChanged;
        if (player.PlayerIndex == 0)
        {
            player.PlayerHand = player1Hand;
        }
        else
        {
            player.PlayerHand = player2Hand;
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
       transform.parent.gameObject.transform.GetChild(7).gameObject.GetComponent<GameLogic>().RoundScoreText.SetText("Wins \n" + transform.parent.gameObject.transform.GetChild(7).gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].PlayerName + ": " + transform.parent.gameObject.transform.GetChild(7).gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].PlayerWins +
         "\n" + transform.parent.gameObject.transform.GetChild(7).gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerName + ": " + transform.parent.gameObject.transform.GetChild(7).gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerWins);
    }

    private void ScoreChanged(int handValue, int playerIndex, string playerName)
    {
        if (playerIndex == 0)
        {
            scoreText.text = playerName + ": " + handValue.ToString() + " /21"; 
        }
        else
        {
            if (transform.parent.gameObject.transform.GetChild(7).GetComponent<GameLogic>().HiddenValue != 0)
            {
                player2Score = playerName + ": ? + " + transform.parent.gameObject.transform.GetChild(7).GetComponent<GameLogic>().HiddenValue + " /21";
                scoreText2.text = player2Score;
            }
            else
            {
                player2Score = playerName + ": ? /21";
                scoreText2.text = player2Score;
            }
        }
    }
    
    private void NameChanged()
    {
      WinsChanged();
    // var newText = scoreText2.text.ToCharArray();
    // newText[0] = 'M';
    // newText[1] = 'a';
    // newText[2] = 's';
    // newText[3] = 't';
    // newText[4] = 'e';
    // newText[5] = 'r';
    // newText[6] = 'B';
    // newText[7] = 'o';
    // newText[8] = 't';
    // scoreText2.text = newText.ToString();
    // Debug.Log(newText);

    }

    public void SetPlayer2ScoreText(string playerName, int handValue)
    {
        scoreText2.text = playerName + ": " + handValue.ToString() + " /21";
    }

}
    
