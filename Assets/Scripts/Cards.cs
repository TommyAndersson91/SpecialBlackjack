using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour
{
public static int[] theCards = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
public static ArrayList CardDeck = new ArrayList();

public void Start() 
{
   for (int i = 0; i < theCards.Length; i++)
   {
    Cards.CardDeck.Add(i);
   }
    Debug.Log(CardDeck.ToString());
}
}
