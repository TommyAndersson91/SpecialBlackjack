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

public class TrumpCards : MonoBehaviour, IPointerClickHandler
{
    bool IsCardClicked = false;
    public static GameObject ClickedCard;

    private void Start() {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
        foreach (var card in GameObject.FindGameObjectsWithTag("hand_increase"))
        {
            card.GetComponent<TrumpCards>().IsCardClicked = false;
            // card.GetComponent<TrumpCards>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (card != this.gameObject)
            {
            card.GetComponent<Animator>().Play("normalstate");
            }
            card.GetComponent<TrumpCards>().gameObject.name = "Card";
        }
        foreach (var card in GameObject.FindGameObjectsWithTag("add_last_card_value"))
        {
            card.GetComponent<TrumpCards>().IsCardClicked = false;
            // card.GetComponent<TrumpCards>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (card != this.gameObject)
            {
                card.GetComponent<Animator>().Play("normalstate");
            }
            card.GetComponent<TrumpCards>().gameObject.name = "Card";
        }
        IsCardClicked = true;
        Debug.Log(gameObject.tag);
        this.gameObject.name = "CardClicked";
        ClickedCard = null;
        ClickedCard = this.gameObject;
        ClickedCard.GetComponent<Animator>().Play("enlarge");
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

    private void OnMouseDown()
     {
      
    }
    
    // private void OnMouseExit() {
    //     transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    // }
}
