using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

enum AllTrumpCards
{
  HandIncrease,
  AddLastCardValue
}

public class TrumpCard : MonoBehaviour, IPointerClickHandler
{
  bool IsCardClicked = false;
  public static GameObject ClickedCard;
  private GameLogic gameLogic;

  private void Start()
  {
    CardController.TrumpCards.Add(this.gameObject);
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    gameLogic = transform.parent.transform.parent.transform.parent.transform.parent.GetChild(6).gameObject.GetComponent<GameLogic>();
    if (this.gameObject.transform.parent.name == "TrumpCardsPanel2" && transform.parent.transform.parent.transform.parent.gameObject.transform.transform.parent.GetChild(6).gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerIndex == 0)
    {
      return;
    }
    if (this.gameObject.transform.parent.name == "TrumpCardsPanel" && transform.parent.transform.parent.transform.parent.transform.parent.gameObject.transform.GetChild(6).gameObject.GetComponent<GameLogic>().CurrentPlayer.PlayerIndex == 1)
    {
      return;
    }
    CardController.TrumpCards.Remove(this.gameObject);
    foreach (var card in CardController.TrumpCards)
    {
      if (card != null)
      {
        card.GetComponent<TrumpCard>().IsCardClicked = false;
        if (card.transform.localScale.x > 1)
        {
          card.GetComponent<Animator>().Play("normalstate");
        }
        card.GetComponent<TrumpCard>().gameObject.name = "Card";
      }
    }
    IsCardClicked = true;
    this.gameObject.name = "CardClicked";
    ClickedCard = this.gameObject;
    ClickedCard.GetComponent<Animator>().Play("enlarge");
    CardController.TrumpCards.Add(this.gameObject);
    gameLogic.UpdateUI();

  }

  public GameObject AddRandomTrumpCard(GameObject trumpCard)
  {
    int random = Random.Range(0, 2);
    switch (random)
    {
      case 0:
        return SetHandIncrease(trumpCard);
      case 1:
        return AddLastCardValue(trumpCard);
      default:
        return trumpCard;
    }
  }

  private GameObject SetHandIncrease(GameObject trumpCard)
  {
    trumpCard.gameObject.tag = "hand_increase";
    trumpCard.transform.GetComponentInChildren<TMP_Text>().fontSize = 14;
    trumpCard.transform.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Italic;
    trumpCard.transform.GetComponentInChildren<TMP_Text>().SetText("Increase your opponents hand value by 3");
    return trumpCard;
  }

  private GameObject AddLastCardValue(GameObject trumpCard)
  {
    trumpCard.gameObject.tag = "add_last_card_value";
    trumpCard.transform.GetComponentInChildren<TMP_Text>().fontSize = 14;
    trumpCard.transform.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Italic;
    trumpCard.transform.GetComponentInChildren<TMP_Text>().SetText("Add the value from your opponent's last card to your hand");
    return trumpCard;
  }
}
