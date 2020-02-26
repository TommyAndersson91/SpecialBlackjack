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

    public void SetupPanel(Player player)
    {
        ScoreChanged(player.HandValue, player.PlayerIndex, player.PlayerName);
        player.onAdd += ScoreChanged;
        player.onWin += WinsChanged;
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

    private void WinsChanged()
    {
        GameObject.Find("GameLogic").GetComponent<GameLogic>().RoundScoreText.text = "Wins \n" + GameObject.Find("GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[0].PlayerName + ": " + GameObject.Find("GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[0].PlayerWins +
        "\n" + GameObject.Find("GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerName + ": " + GameObject.Find("GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[1].PlayerWins;
    }

    private void ScoreChanged(int handValue, int playerIndex, string playerName)
    {
        if (playerIndex == 0)
        {
            scoreText.text = playerName + ": " + handValue.ToString() + " /21"; 
        }
        else
        {
            if (GameObject.Find("GameLogic").GetComponent<GameLogic>().HiddenValue != 0)
            {
                scoreText2.text = playerName + ": ? + " + GameObject.Find("GameLogic").GetComponent<GameLogic>().GetHiddenValue() + " /21";
            }
            else
            {
                scoreText2.text = playerName + ": ? /21";
            }
        }
    }
    public void SetPlayer2ScoreText(string playerName, int handValue)
    {
        scoreText2.text = playerName + ": " + handValue.ToString() + " /21";
    }

}
    
