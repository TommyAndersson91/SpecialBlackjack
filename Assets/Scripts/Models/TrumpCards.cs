using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrumpCards : MonoBehaviour, IPointerClickHandler
{
    bool IsCardClicked = false;
    public static GameObject ClickedCard;

    private void Start() {
        gameObject.AddComponent<PlayerPanel>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Item clicked");
        foreach (var card in GameObject.FindGameObjectsWithTag("trumpcard"))
        {
            card.GetComponent<TrumpCards>().IsCardClicked = false;
            // card.GetComponent<TrumpCards>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            card.GetComponent<Animator>().Play("normalstate");
            card.GetComponent<TrumpCards>().gameObject.name = "Card";
        }
        gameObject.name = "CardClicked";
        transform.GetComponent<Animator>().Play("enlarge");
        IsCardClicked = true;
        ClickedCard = this.gameObject;

    }

    private void OnMouseDown()
     {
      
    }
    
    // private void OnMouseExit() {
    //     transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    // }
}
