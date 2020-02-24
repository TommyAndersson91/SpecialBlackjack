﻿using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CardController : MonoBehaviour
{
    public IEnumerator CardAdded(GameObject card, int currentPlayerIndex, Text StartPos, Vector3 endPos)
    {
        card.transform.position = StartPos.transform.position;
        float elapsedTime = 0;
        float waitTime = 2f;
        endPos.Set(HandArranger.GetX(currentPlayerIndex), HandArranger.GetY(currentPlayerIndex), 0.0f);
        while (elapsedTime < waitTime)
        {
            card.transform.position = Vector3.Lerp(card.transform.position, endPos, elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public void DrawCard(int RoundCounter, Stack AvaibleCards, Text StartPos, Vector3 endPos)
    {
        if (GameLogic.PlayingAgainstAI && !PlayerList.GetPlayers()[0].IsPassed)
        {
            PlayerList.GetPlayers()[1].IsPlayersTurn = !PlayerList.GetPlayers()[1].IsPlayersTurn;
        }
            GameObject card = Instantiate(Resources.Load<GameObject>("1"));
        if (RoundCounter == 2)
        {
            card.transform.SetParent(gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerHand.transform);
            card.transform.GetComponentInChildren<TMP_Text>().SetText("?");
            gameObject.GetComponent<GameLogic>().HiddenCard = card; 
        }
        else
        {
            card.transform.GetComponentInChildren<TMP_Text>().SetText(AvaibleCards.Peek().ToString());
            if (RoundCounter < 5)
            {
                card.transform.SetParent(gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerHand.transform);
            }
            AnimateCardFly(card, gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerIndex, RoundCounter, StartPos, endPos);
        }
        gameObject.GetComponent<GameLogic>().CurrentPlayer.HandValue += int.Parse(AvaibleCards.Peek().ToString());
        gameObject.GetComponent<GameLogic>().CurrentPlayer.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
        Cards.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
    }

    public void AnimateCardFly(GameObject card, int currentPlayerIndex, int RoundCounter, Text StartPos, Vector3 endPos)
    {
        if (RoundCounter > 4)
        {
            card.transform.SetParent(PlayerList.GetPlayers()[currentPlayerIndex].PlayerHand.transform.GetComponentInParent<Canvas>().transform);
            StartCoroutine(CardAdded(card, currentPlayerIndex, StartPos, endPos));
        }
    }

    public void UseTrumpCard(Player currentPlayer, GameObject player1TrumpCards, GameObject player2TrumpCards)
    {
        if (currentPlayer.TrumpCards > 0)
        {
            if (currentPlayer.PlayerIndex == PlayerList.GetPlayers().Count - 1)
            {
                PlayerList.GetPlayers()[currentPlayer.PlayerIndex] = currentPlayer;
                PlayerList.GetPlayers()[0].HandValue = PlayerList.GetPlayers()[0].HandValue + 3;
                PlayerList.GetPlayers()[1].TrumpCards -= 1;
                int numChildren = player2TrumpCards.transform.childCount;
                player2TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = false;
                player2TrumpCards.transform.GetChild(numChildren - 1).gameObject.GetComponent<Animation>().Play("animshrink");
                StartCoroutine(ShrinkTrumpCard(currentPlayer.PlayerIndex));
            }
            else
            {
                PlayerList.GetPlayers()[currentPlayer.PlayerIndex] = currentPlayer;
                PlayerList.GetPlayers()[1].HandValue = PlayerList.GetPlayers()[1].HandValue + 3;
                PlayerList.GetPlayers()[0].TrumpCards -= 1;
                int numChildren = player1TrumpCards.transform.childCount;
                player1TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = false;
                player1TrumpCards.transform.GetChild(numChildren - 1).GetComponent<Animation>().Play("animshrink");
                StartCoroutine(ShrinkTrumpCard(currentPlayer.PlayerIndex));
                
            }
        }
    }

    IEnumerator ShrinkTrumpCard(int playerIndex)
    {
        yield return new WaitForSeconds(1);
        if (playerIndex == PlayerList.GetPlayers().Count - 1)
        {
            int numChildren = gameObject.GetComponent<GameLogic>().Player2TrumpCards.transform.childCount;
            Destroy(gameObject.GetComponent<GameLogic>().Player2TrumpCards.transform.GetChild(numChildren - 1).gameObject);
            if (!PlayerList.GetPlayers()[0].IsPassed)
            {
                gameObject.GetComponent<GameLogic>().CurrentPlayer = PlayerList.GetPlayers()[0];
            }
            else
            {
                gameObject.GetComponent<GameLogic>().CurrentPlayer = PlayerList.GetPlayers()[1];
            }
            yield break;
        }
        else
        {
            int numChildren = gameObject.GetComponent<GameLogic>().Player1TrumpCards.transform.childCount;
            Destroy(gameObject.GetComponent<GameLogic>().Player1TrumpCards.transform.GetChild(numChildren - 1).gameObject);
            if (!PlayerList.GetPlayers()[1].IsPassed)
            {
                gameObject.GetComponent<GameLogic>().CurrentPlayer = PlayerList.GetPlayers()[1];
            }
            else
            {
                gameObject.GetComponent<GameLogic>().CurrentPlayer = PlayerList.GetPlayers()[0];
            }
            yield break;
        }
    }    
}
