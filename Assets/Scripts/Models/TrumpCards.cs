using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrumpCards : MonoBehaviour, IPointerClickHandler
{
    bool IsCardClicked = false;
    public static GameObject ClickedCard;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Item clicked");
        foreach (var card in GameObject.FindGameObjectsWithTag("trumpcard"))
        {
            card.GetComponent<TrumpCards>().IsCardClicked = false;
            card.GetComponent<TrumpCards>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
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
