using System.Threading;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
public class CardController : MonoBehaviour
{
  public static List<GameObject> TrumpCards = new List<GameObject>();
  public static List<GameObject> AITrumpCards = new List<GameObject>();

  public IEnumerator CardAdded(GameObject card, int currentPlayerIndex, Text StartPos, Vector3 endPos)
  {
    card.transform.GetChild(1).gameObject.SetActive(true);
    card.transform.position = StartPos.transform.position;
    float elapsedTime = 0;
    float waitTime = 1.5f;
    endPos.Set(gameObject.GetComponent<HandArranger>().GetX(currentPlayerIndex), gameObject.GetComponent<HandArranger>().GetY(currentPlayerIndex), 10f);
    while (elapsedTime < waitTime)
    {
      float fraction = TranslateHelper.GetFraction(elapsedTime, waitTime, "CubicOutBack");
      card.transform.position = card.transform.position * (1 - fraction) + (endPos * fraction);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    card.transform.GetChild(1).gameObject.SetActive(false);
    yield break;
  }

  public Stack GetStack()
  {
    return gameObject.GetComponent<Cards>().AvaibleCards;
  }

  public int[] GetInts()
  {
    return gameObject.GetComponent<Cards>().ints;
  }

  public List<int> GetDrawnCards()
  {
    return gameObject.GetComponent<Cards>().DrawnCards;
  }

  public void DrawCard(GameObject card, int RoundCounter, Text StartPos, Vector3 endPos)
  {
    if (gameObject.GetComponent<GameLogic>().PlayingAgainstAI && !gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].IsPassed)
    {
      gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].IsPlayersTurn = !gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].IsPlayersTurn;
    }

    if (RoundCounter == 2)
    {
      card.transform.SetParent(gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerHand.transform, false);
      card.transform.GetComponentInChildren<TMP_Text>().SetText("?");
      gameObject.GetComponent<GameLogic>().HiddenCard = card;
    }
    else
    {
      card.transform.GetComponentInChildren<TMP_Text>().SetText(gameObject.GetComponent<CardController>().GetStack().Peek().ToString());
      if (RoundCounter < 5)
      {
        card.transform.SetParent(gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerHand.transform, false);
      }
      else
      {
        SoundManager.instance.RandomizeSfx(gameObject.GetComponent<GameLogic>().draw1Sound, gameObject.GetComponent<GameLogic>().draw2Sound);
        AnimateCardFly(card, gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerIndex, RoundCounter, StartPos, endPos);
      }
    }
    gameObject.GetComponent<GameLogic>().CurrentPlayer.HandValue += int.Parse(gameObject.GetComponent<CardController>().GetStack().Peek().ToString());
    gameObject.GetComponent<GameLogic>().CurrentPlayer.DrawnCards.Add(int.Parse(gameObject.GetComponent<CardController>().GetStack().Peek().ToString()));
    GetDrawnCards().Add(int.Parse(gameObject.GetComponent<CardController>().GetStack().Peek().ToString()));
    gameObject.GetComponent<CardController>().GetStack().Pop();
    gameObject.GetComponent<GameLogic>().CheckFinished();
  }

  public void AnimateCardFly(GameObject card, int currentPlayerIndex, int RoundCounter, Text StartPos, Vector3 endPos)
  {
    card.transform.SetParent(gameObject.GetComponent<GameLogic>().winnerText.transform, false);
    StartCoroutine(CardAdded(card, currentPlayerIndex, StartPos, endPos));
  }

  public void UseTrumpCard(Player currentPlayer, GameObject player1TrumpCards, GameObject player2TrumpCards)
  {
    if (currentPlayer.TrumpCards > 0)
    {
      if (currentPlayer.PlayerIndex == gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers().Count - 1)
      {
        int numChildren = player2TrumpCards.transform.childCount;
        int childNumber = 0;
        gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[currentPlayer.PlayerIndex] = currentPlayer;
        if (gameObject.GetComponent<AIController>().IsUsingHandIncrease)
        {
          gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].HandValue = gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].HandValue + 3;
          for (int i = 0; i < numChildren - 1; i++)
          {
            if (player2TrumpCards.transform.GetChild(i).gameObject.tag == "hand_increase" && gameObject.GetComponent<AIController>().IsUsingHandIncrease == true)
            {
              player2TrumpCards.transform.GetChild(i).gameObject.GetComponent<Animator>().Play("shrink");
              childNumber = player2TrumpCards.transform.GetChild(i).GetSiblingIndex();
              gameObject.GetComponent<AIController>().IsUsingHandIncrease = false;
            }
          }
        }
        else if (gameObject.GetComponent<AIController>().IsUsingAddLastCard)
        {
          gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].HandValue += gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].DrawnCards[gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].DrawnCards.Count - 1];
          for (int i = 0; i < numChildren - 1; i++)
          {
            if (player2TrumpCards.transform.GetChild(i).gameObject.tag == "add_last_card_value" && gameObject.GetComponent<AIController>().IsUsingAddLastCard == true)
            {
              player2TrumpCards.transform.GetChild(i).gameObject.GetComponent<Animator>().Play("shrink");
              childNumber = player2TrumpCards.transform.GetChild(i).GetSiblingIndex();
              gameObject.GetComponent<AIController>().IsUsingAddLastCard = false;
            }
          }
        }
        gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].TrumpCards -= 1;
        StartCoroutine(ShrinkTrumpCard(currentPlayer.PlayerIndex, player1TrumpCards, player2TrumpCards, childNumber));
      }
      else
      {
        if (TrumpCard.ClickedCard != null)
        {
          gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[currentPlayer.PlayerIndex] = currentPlayer;
          if (TrumpCard.ClickedCard.tag == "hand_increase")
          {
            gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].HandValue = gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].HandValue + 3;
          }
          if (TrumpCard.ClickedCard.tag == "add_last_card_value")
          {
            gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].HandValue += gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].DrawnCards[gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].DrawnCards.Count - 1];
          }
          gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].TrumpCards -= 1;
          TrumpCard.ClickedCard.transform.GetComponent<Animator>().Play("shrink");
          StartCoroutine(ShrinkTrumpCard(currentPlayer.PlayerIndex, player1TrumpCards, player2TrumpCards));
        }
      }
    }
  }

  IEnumerator ShrinkTrumpCard(int playerIndex, GameObject player1TrumpCards, GameObject player2TrumpCards, int childNumber = 0)
  {
    yield return new WaitForSeconds(1);
    if (playerIndex == gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers().Count - 1)
    {
      int numChildren = gameObject.GetComponent<GameLogic>().playerPanel.GetTrumpCardPanel(1).transform.childCount;
      TrumpCards.Remove(gameObject.GetComponent<GameLogic>().playerPanel.GetTrumpCardPanel(1).transform.GetChild(childNumber).gameObject);
      Destroy(gameObject.GetComponent<GameLogic>().playerPanel.GetTrumpCardPanel(1).transform.GetChild(childNumber).gameObject);
      player2TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = true;
      if (!gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0].IsPassed)
      {
        gameObject.GetComponent<GameLogic>().CurrentPlayer = gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0];
        gameObject.GetComponent<GameLogic>().UpdateUI();
      }
      else
      {
        gameObject.GetComponent<GameLogic>().CurrentPlayer = gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1];
      }
      yield break;
    }
    else
    {
      int numChildren = gameObject.GetComponent<GameLogic>().playerPanel.GetTrumpCardPanel(0).transform.childCount;
      Destroy(TrumpCard.ClickedCard);
      TrumpCard.ClickedCard = null;
      player1TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = true;
      if (!gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].IsPassed)
      {
        gameObject.GetComponent<GameLogic>().CurrentPlayer = gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1];
        gameObject.GetComponent<GameLogic>().UpdateUI();
        gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[1].IsPlayersTurn = true;
        gameObject.GetComponent<AIController>().CheckAITurn(gameObject.GetComponent<GameLogic>().RoundCounter);
      }
      else
      {
        gameObject.GetComponent<GameLogic>().CurrentPlayer = gameObject.GetComponent<GameLogic>().PlayerList.GetPlayers()[0];
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

  public void RandomTrumpCardFX()
  {
    if (TrumpCards.Count > 2)
    {
      int glow = 0;
      foreach (var item in TrumpCards)
      {
        if (item != null)
        {
          item.transform.GetChild(1).gameObject.SetActive(false);
        }
      }
      foreach (var item in TrumpCards)
      {
        if (item != null)
        {
          glow++;
          item.transform.GetChild(1).GetComponent<ParticleSystem>().startDelay = gameObject.GetComponent<GameLogic>().random.Next(0, 3);
          item.transform.GetChild(1).gameObject.SetActive(true);
          if (glow == 2)
          {
            return;
          }
        }
      }
    }
    else
    {
      foreach (var item in TrumpCards)
      {
        if (item != null)
        {
          item.transform.GetChild(1).gameObject.SetActive(true);
        }
      }
    }
    // TrumpCards[gameObject.GetComponent<GameLogic>().random.Next(0, TrumpCards.Count)].gameObject.SetActive(true);
  }
}
