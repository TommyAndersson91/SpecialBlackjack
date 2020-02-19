using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cards : MonoBehaviour
{
public static int[] theCards = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
public static ArrayList CardDeck = new ArrayList();
public static List<Int32> DrawnCards;


private void Start() {
    
}

public static void InitCards()
{
DrawnCards = new List<Int32>();
}
}
