using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class Player : MonoBehaviour
{
    public ArrayList PlayerCards = new ArrayList();
    public int randomIndex;
    public int randomIndex2;
    public static int[] theCards = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    public static List<Int32> CardDeck = new List<Int32>();
    public ArrayList drawnCards;
    public TextMeshProUGUI myText;
    public GameObject card1;
    public GameObject card2;
    public GameObject player1Hand;
    public GameObject player2Hand;
    public System.Random random;
    


  public void Start() {
        player1Hand = GameObject.Find("CardsInHandPanel");
        player2Hand = GameObject.Find("CardsInHandPanel2");
        random = new System.Random();
        for (int i = 1; i < theCards.Length; i++)
        {
            CardDeck.Add(i);
        }
  }

  public void Randomize()
  {
        
        int[] drawn = Enumerable.Range(1, 11).OrderBy(x => random.Next()).Take(2).ToArray();
        // randomIndex = random.Next(0, (CardDeck.Count));
        // Debug.Log(randomIndex);
         randomIndex = drawn[0];
         randomIndex2 = drawn[1];
        // if (randomIndex == randomIndex2)
        // {
        //     // randomIndex = Random.Range(0, 10);
        //     Randomize();
        // }
        card1 = GameObject.Find(""+randomIndex);
        card2 = GameObject.Find(""+randomIndex2);
        // for (int i = 0; i < CardDeck.Count; i++)
        // {
        //     if (randomIndex == CardDeck.IndexOf(i))
        //     {
        //     CardDeck.RemoveAt(i);
        //     }
        // }
        // foreach (var item in CardDeck)
        // {
        //     if (int.Parse(item.ToString()) == randomIndex)
        //     {
        //         CardDeck.Remove(item);
        //     }
        // }
    }

  public void DrawCards()
  {
        Randomize();
        
        if (player1Hand.transform.childCount > 0)
        {
            player1Hand.transform.DetachChildren();
            PlayerCards.RemoveRange(0, 2);
        }
        card1.transform.SetParent(player1Hand.transform);
        card2.transform.SetParent(player1Hand.transform);

     

        // transform.parent.GetComponent<Cards>().SetCards();
        
    
        for (int i = 0; i < 2; i++)
        {
            // PlayerCards.Add(Cards.CardDeck[randomIndex]);
            PlayerCards.Add(randomIndex);
            PlayerCards.Add(randomIndex2);
            // CardDeck.RemoveAt(randomIndex);
            // PlayerCards.Add(Cards.CardDeck[randomIndex2]);
            // CardDeck.RemoveAt(randomIndex2);
        }
        
        
        int valueCard1 = int.Parse(player1Hand.transform.GetChild(0).name);
        int valueCard2 = int.Parse(player1Hand.transform.GetChild(1).name);
        
       // int valueCard1 = int.Parse(PlayerCards[0].ToString());
       // int valueCard2 = int.Parse(PlayerCards[1].ToString());
        // int totalValue = valueCard1 + valueCard2;
        myText.text = valueCard1+valueCard2 + " / 21";
        
  }
}
