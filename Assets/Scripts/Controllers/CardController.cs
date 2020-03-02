using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
public class CardController : MonoBehaviour
{
    public IEnumerator CardAdded(GameObject card, int currentPlayerIndex, Text StartPos, Vector3 endPos)
    {
        card.transform.position = StartPos.transform.position;
        float elapsedTime = 0;
        float waitTime = 2f;
        endPos.Set(gameObject.GetComponent<HandArranger>().GetX(currentPlayerIndex), gameObject.GetComponent<HandArranger>().GetY(currentPlayerIndex), 0.0f);
        while (elapsedTime < waitTime)
        {
            card.transform.position = Vector3.Lerp(card.transform.position, endPos, elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public Stack GetStack()
    {
        return gameObject.GetComponent<Cards>().AvaibleCards;
    }

    public int[] getInts()
    {
        return gameObject.GetComponent<Cards>().ints;
    }

    public List<int> GetDrawnCards()
    {
        return gameObject.GetComponent<Cards>().DrawnCards;
    }

    public void DrawCard(int RoundCounter, Text StartPos, Vector3 endPos)
    {
        if (gameObject.GetComponent<GameLogic>().PlayingAgainstAI && !gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].IsPassed)
        {
            gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].IsPlayersTurn = !gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].IsPlayersTurn;
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
            card.transform.GetComponentInChildren<TMP_Text>().SetText(gameObject.GetComponent<CardController>().GetStack().Peek().ToString());
            if (RoundCounter < 5)
            {
                card.transform.SetParent(gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerHand.transform);
            }
            AnimateCardFly(card, gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerIndex, RoundCounter, StartPos, endPos);
        }
        gameObject.GetComponent<GameLogic>().CurrentPlayer.HandValue += int.Parse(gameObject.GetComponent<CardController>().GetStack().Peek().ToString());
        gameObject.GetComponent<GameLogic>().CurrentPlayer.DrawnCards.Add(int.Parse(gameObject.GetComponent<CardController>().GetStack().Peek().ToString()));
        GetDrawnCards().Add(int.Parse(gameObject.GetComponent<CardController>().GetStack().Peek().ToString()));
        gameObject.GetComponent<CardController>().GetStack().Pop();
        }

    public void AnimateCardFly(GameObject card, int currentPlayerIndex, int RoundCounter, Text StartPos, Vector3 endPos)
    {
        if (RoundCounter > 4)
        {
            card.transform.SetParent(gameObject.GetComponent<GameLogic>().winnerText.transform);
            StartCoroutine(CardAdded(card, currentPlayerIndex, StartPos, endPos));
        }
    }

    public void UseTrumpCard(Player currentPlayer, GameObject player1TrumpCards, GameObject player2TrumpCards)
    {
        if (currentPlayer.TrumpCards > 0)
        {
            if (currentPlayer.PlayerIndex == gameObject.GetComponent<PlayerList>().GetPlayers().Count - 1)
            {
                gameObject.GetComponent<PlayerList>().GetPlayers()[currentPlayer.PlayerIndex] = currentPlayer;
                gameObject.GetComponent<PlayerList>().GetPlayers()[0].HandValue = gameObject.GetComponent<PlayerList>().GetPlayers()[0].HandValue + 3;
                gameObject.GetComponent<PlayerList>().GetPlayers()[1].TrumpCards -= 1;
                int numChildren = player2TrumpCards.transform.childCount;
                player2TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = false;
                player2TrumpCards.transform.GetChild(numChildren - 1).gameObject.GetComponent<Animation>().Play("animshrink");
                StartCoroutine(ShrinkTrumpCard(currentPlayer.PlayerIndex));
            }
            else
            {
                gameObject.GetComponent<PlayerList>().GetPlayers()[currentPlayer.PlayerIndex] = currentPlayer;
                gameObject.GetComponent<PlayerList>().GetPlayers()[1].HandValue = gameObject.GetComponent<PlayerList>().GetPlayers()[1].HandValue + 3;
                gameObject.GetComponent<PlayerList>().GetPlayers()[0].TrumpCards -= 1;
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
        if (playerIndex == gameObject.GetComponent<PlayerList>().GetPlayers().Count - 1)
        {
            int numChildren = gameObject.GetComponent<GameLogic>().playerPanel.GetTrumpCardPanel(1).transform.childCount;
       
            Destroy(gameObject.GetComponent<GameLogic>().playerPanel.GetTrumpCardPanel(1).transform.GetChild(numChildren - 1).gameObject);
            if (!gameObject.GetComponent<PlayerList>().GetPlayers()[0].IsPassed)
            {
                gameObject.GetComponent<GameLogic>().CurrentPlayer = gameObject.GetComponent<PlayerList>().GetPlayers()[0];
            }
            else
            {
                gameObject.GetComponent<GameLogic>().CurrentPlayer = gameObject.GetComponent<PlayerList>().GetPlayers()[1];
            }
            yield break;
        }
        else
        {
            int numChildren = gameObject.GetComponent<GameLogic>().playerPanel.GetTrumpCardPanel(0).transform.childCount; ;
            Destroy(gameObject.GetComponent<GameLogic>().playerPanel.GetTrumpCardPanel(0).transform.GetChild(numChildren - 1).gameObject);
            if (!gameObject.GetComponent<PlayerList>().GetPlayers()[1].IsPassed)
            {
                gameObject.GetComponent<GameLogic>().CurrentPlayer = gameObject.GetComponent<PlayerList>().GetPlayers()[1];
                gameObject.GetComponent<PlayerList>().GetPlayers()[1].IsPlayersTurn = true;
                gameObject.GetComponent<AIController>().CheckAITurn(gameObject.GetComponent<GameLogic>().RoundCounter); 
            }
            else
            {
                gameObject.GetComponent<GameLogic>().CurrentPlayer = gameObject.GetComponent<PlayerList>().GetPlayers()[0];
            }
            yield break;
        }
    }

    public void ShuffleArray(int[] a)
    {
        int length = a.Length;
        for (int i = 0; i < length; i++)
        {
            Swap(a, i, i + gameObject.GetComponent<GameLogic>().random.Next(length - i));
        }
    }

    public void Swap(int[] arr, int a, int b)
    {
        int temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }    
}
