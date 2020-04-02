using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cards : MonoBehaviour
{
  public int[] ints = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
  public List<int> DrawnCards;
  public Stack AvaibleCards { get; set; }

  public void InitCards()
  {
    DrawnCards = new List<Int32>();
  }
}
