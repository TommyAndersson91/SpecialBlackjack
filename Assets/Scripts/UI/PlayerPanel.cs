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
    // private int HiddenValue {get; set;}
    public void SetupPanel(Player player)
    {
        ScoreChanged(player.HandValue, player.PlayerIndex, player.PlayerName);
        player.onAdd += ScoreChanged;
    }
    private void Start() {
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
                scoreText2.text = playerName + ": (?) " + GameObject.Find("GameLogic").GetComponent<GameLogic>().GetHiddenValue() + " /21";
            }
            else
            {
            scoreText2.text = playerName + ": ? " + " /21";
            }
        }
    }
    public void SetPlayer2ScoreText(string playerName, int handValue)
    {
        scoreText2.text = playerName + ": " + handValue.ToString() + " /21";
    }

}
    
