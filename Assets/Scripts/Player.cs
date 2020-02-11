using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;


public class Player : MonoBehaviour
{
public string PlayerName {get; set;}
public int HandValue {get; set;}
public List<Int32> DrawnCards { get; set; }
public bool IsPassed { get; set; }
public bool IsPlayersTurn { get; set;}
public GameObject PlayerHand {get; set;}

public Player(string playerName, GameObject playerHand)
{
  this.PlayerName = playerName;
  this.PlayerHand = playerHand;
  HandValue = 0;
  IsPassed = false;
  IsPlayersTurn = false;
  DrawnCards = new List<Int32>();
}
public Player()
{
  HandValue = 0;
  IsPassed = false;
  IsPlayersTurn = false;
  DrawnCards = new List<Int32>();
}  
}

