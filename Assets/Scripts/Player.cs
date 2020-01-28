using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;


public class Player : MonoBehaviour
{
    public ArrayList PlayerCards = new ArrayList();
    public List<Int32> PlayedCards = new List<Int32>();
    public int randomIndex;
    public int randomIndex2;
    public static int[] theCards = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    public List<Int32> CardDeck = new List<Int32>();
    public ArrayList drawnCards;
    public TextMeshProUGUI myText;
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public GameObject card5;
    public GameObject player1Hand;
    public GameObject player2Hand;
    public System.Random random;
    public bool firstDraw;
    public int totalValue;
   
    


  public void Start() {
        player1Hand = GameObject.Find("CardsInHandPanel");
        player2Hand = GameObject.Find("CardsInHandPanel2");
        random = new System.Random();
        for (int i = 1; i < theCards.Length; i++)
        {
            CardDeck.Add(i);
        }
        firstDraw = true;
        var numbers = new List<int>(Enumerable.Range(1, 75));
        Stack myStack = new Stack();
        
         
  }
   
        public void Randomize()
  {
        if (firstDraw)
        {
       
        card1 = GameObject.Find(""+randomIndex);
        card2 = GameObject.Find(""+randomIndex2);
        totalValue = randomIndex+randomIndex2;
        } else
        {
            CheckNumber();
          GameObject nextCard = GameObject.Find(""+randomIndex);
          nextCard.transform.SetParent(player1Hand.transform);

        }
  }
        
 public void CheckNumber()
 {
        randomIndex = random.Next(CardDeck.Count());
            PlayedCards.Add(CardDeck.IndexOf(randomIndex));
            CardDeck.Remove(randomIndex);
                if (CardDeck.Count < 1)
                {
                    myText.text = "No more cards";
                } 
 }

        // foreach (var item in CardDeck)
        // {
        //     if (item == randomIndex)
        //     {
        //         CardDeck.Remove(item);
        //     }
        // }
    

  public void DrawCards()
  {
        Randomize();
        
        //För att rensa spelarens hand.
        // if (player1Hand.transform.childCount > 0)
        // {
        //     player1Hand.transform.DetachChildren();
        //     PlayerCards.RemoveRange(0, 2);
        // }
        if (firstDraw)
        {
        card1.transform.SetParent(player1Hand.transform);
        card2.transform.SetParent(player1Hand.transform);
        // transform.parent.GetComponent<Cards>().SetCards();

            PlayedCards.Add(randomIndex);
            PlayedCards.Add(randomIndex2);

        firstDraw = false;
        } else
        {
          totalValue += randomIndex; 
        }
       
        myText.text = totalValue + " / 21";
  }

  public void DrawCard()
  {

  }
 
}

