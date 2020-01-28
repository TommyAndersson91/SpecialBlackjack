using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameLogic : MonoBehaviour
{
    public Stack AvaibleCards;
    int[] ints = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
    public bool firstDraw;
    public bool player1Turn;
    public GameObject player1Hand;
    public GameObject player2Hand;
    public TextMeshProUGUI myText;
    public int totalValue;
    public int totalValue2;

    // Start is called before the first frame update
    void Start()
    {
     ShuffleArray(ints);
    player1Hand = GameObject.Find("CardsInHandPanel");
    player2Hand = GameObject.Find("CardsInHandPanel2");
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
        player1Turn = !player1Turn;

    }

    public void DrawCards()
    {
       
       
        GameObject nextCard = GameObject.Find("" + AvaibleCards.Peek());
        if (player1Turn)
        {
            nextCard.transform.SetParent(player1Hand.transform);
            totalValue += int.Parse(AvaibleCards.Peek().ToString());
            myText.text = totalValue + " / 21";
        }
        else
        {
            nextCard.transform.SetParent(player2Hand.transform);
            totalValue2 += int.Parse(AvaibleCards.Peek().ToString());
            myText.text = totalValue2 + " / 21";
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
}
