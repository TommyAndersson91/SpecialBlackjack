using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
//     public void UpdateUI()
//     {
//         HiddenValue = PlayerList.GetPlayers()[1].HandValue - PlayerList.GetPlayers()[1].DrawnCards[0];
//         player1Score.text = PlayerList.GetPlayers()[0].PlayerName + ": " + PlayerList.GetPlayers()[0].HandValue + " / 21";
//         if (!RoundOver && RoundCounter >= 4)
//         {
//             winnerText.SetText(CurrentPlayer.PlayerName + "'s turn");
//             player2Score.text = PlayerList.GetPlayers()[1].PlayerName + ": ? + " + HiddenValue + " / 21";
//         }
//         else
//         {
//             player2Score.text = PlayerList.GetPlayers()[1].PlayerName + ": " + PlayerList.GetPlayers()[1].HandValue + " / 21";
//             HiddenCard.transform.GetComponentInChildren<TMP_Text>().SetText(PlayerList.GetPlayers()[1].DrawnCards[0].ToString());
//         }

//         if (CurrentPlayer.TrumpCards == 0)
//         {
//             TrumpCardButton.gameObject.SetActive(false);
//         }
//         else if (CurrentPlayer.PlayerIndex == 0 && PlayerList.GetPlayers()[0].TrumpCards > 0)
//         {
//             TrumpCardButton.gameObject.SetActive(true);
//         }
//         if (CurrentPlayer.PlayerIndex == 0 || CurrentPlayer.PlayerIndex == 1 && !RoundOver && !PlayingAgainstAI)
//         {
//             PassButton.gameObject.SetActive(true);
//             DrawButton.gameObject.SetActive(true);
//             PlayAIButton.gameObject.SetActive(true);
//         }
//         if (CurrentPlayer.PlayerIndex == 1 && PlayingAgainstAI || RoundOver)
//         {
//             TrumpCardButton.gameObject.SetActive(false);
//             PassButton.gameObject.SetActive(false);
//             DrawButton.gameObject.SetActive(false);
//             PlayAIButton.gameObject.SetActive(false);
//         }
//     }
}
