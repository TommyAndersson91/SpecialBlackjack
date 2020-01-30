using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameLogic : MonoBehaviour
{
    public static Stack AvaibleCards;
    int[] ints = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
    public static bool firstDraw;
    public static bool player1Turn;
    public GameObject player1Hand;
    public GameObject player2Hand;
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;
    public TextMeshProUGUI winnerText;
    public static int totalValue;
    public static int totalValue2;
    public static int passCounter;

    // Start is called before the first frame update
    void Start()
    {
     ShuffleArray(ints);
    player1Hand = GameObject.Find("CardsInHandPanel");
    player2Hand = GameObject.Find("CardsInHandPanel2");
    passCounter = 0;
    
      firstDraw = true;
      player1Turn = true;
     //För att se blandningen
    //  for (int i = 0; i < ints.Length; i++)
    //  {
    //      Debug.Log(ints[i]);
    //  }
        AvaibleCards = new Stack(ints);
    }

    public void Pass()
    {
        passCounter++;
       player1Turn = !player1Turn;
       Debug.Log(player1Turn);

        if  (player1Turn)
        {
            player1Score.text = totalValue + " / 21";
        } 
        else 
        {
            player2Score.text = totalValue2 + " / 21";
        }
        

        if (player1Hand.transform.childCount < 1 || player2Hand.transform.childCount < 1)
        {
            firstDraw = true;
        }

        if  (passCounter == 2)
         {
            RoundFinished();
        }

    }

    public void DrawCards()
    {
       
       Debug.Log(player1Turn);
        GameObject nextCard = GameObject.Find("" + AvaibleCards.Peek());
        if (player1Turn)
        {
            nextCard.transform.SetParent(player1Hand.transform);
            totalValue += int.Parse(AvaibleCards.Peek().ToString());
            player1Score.text = totalValue + " / 21";
        }
        if (!player1Turn)
        {
            nextCard.transform.SetParent(player2Hand.transform);
            totalValue2 += int.Parse(AvaibleCards.Peek().ToString());
            player2Score.text = totalValue2 + " / 21";
        }
        AvaibleCards.Pop();
        if (firstDraw)
        {
            firstDraw = false;
            DrawCards();
        }
        
    }

    public static void ShuffleArray(int[] a)
    {
        System.Random rand = new System.Random();
        int length = a.Length;
        for (int i = 0; i < length; i++)
        {
            Swap(a, i, i + rand.Next(length - i));
        }
    }

    public static void Swap(int[] arr, int a, int b)
    {
        
        int temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
        
    }

    public void RoundFinished()
    {
        if (totalValue < 22 && totalValue2 > 21)
        {
            Player1Winner();
        }
        else if (totalValue2 < 22 && totalValue > 21)
        {
            Player2Winner();
        }
        if (totalValue > 21 && totalValue2 > 21)
        {
            int player1Above21 = 0;
            int player2Above21 = 0;
            for (int i = totalValue; i < 21; i--)
            {
                player1Above21++;
            }
            for (int i = totalValue2; i < 21; i--)
            {
                player2Above21++;
            }
            if  (player1Above21 > player2Above21)
            { 
                Player2Winner();
            }
            else if (player2Above21 > player1Above21)
            {
                Player1Winner();
            }
            else
            {
                Draw();
            }
        }
        if (totalValue < 22 && totalValue2 < 22)
        {
                int player1To21 = 0;
                int player2To21 = 0;
                for (int i = totalValue; i < 22; i++)
                {
                    player1To21++;
                }
                for (int i = totalValue2; i < 22; i++)
                {
                    player2To21++;
                }
                if  (player1To21 > player2To21)
                {
                    Player2Winner();
                }
                if (player2To21 > player1To21)
                {
                    Player1Winner();
                }
                if (player1To21 == player2To21)
                {
                    Draw();
                } 
                
        }
        int [] ints = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
        totalValue = 0;
        totalValue2 = 0;
        passCounter = 0;
        player1Turn = true;
        ShuffleArray(ints);
        player1Hand.transform.DetachChildren();
        player2Hand.transform.DetachChildren();
        AvaibleCards = new Stack(ints);

    }

    public void Player1Winner ()
    {
        winnerText.text = "Player 1 is the winner!";
    }

    public void Player2Winner()
    {
        winnerText.text = "Player 2 is the winner!";
    }

    public void Draw()
    {
        winnerText.text = "It's a draw!";
    }
}
