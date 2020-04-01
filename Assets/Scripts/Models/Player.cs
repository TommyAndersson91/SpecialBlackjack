using System.Collections.Generic;
using UnityEngine;
using System;

public class Player: MonoBehaviour
{
  public delegate void Add(int handValue, int playerIndex, string playerName);
  public delegate void CountWins();
  public delegate void ChangeName();
  public ChangeName onNameChanged;
  public CountWins onWin;
  public Add onAdd;
  private int _handValue;
  private int _playerWins;
  private string _playerName;
  public int HandValue
  {
    get
    {
        return _handValue;
    }
    set
    {
      _handValue = value;
      onAdd?.Invoke(_handValue, PlayerIndex, PlayerName);
    }
  }
  public int PlayerWins
  {
    get
    {
        return _playerWins;
    }
    set
    {
      _playerWins = value;
      onWin?.Invoke();
    }
  }
  public string PlayerName
  {
      get { return _playerName; }
      set 
      { 
        _playerName = value;
        onNameChanged?.Invoke();
      }
  }
  
  public List<Int32> DrawnCards { get; set; }
  public bool IsPassed { get; set; }
  public bool IsPlayersTurn { get; set;}
  public bool IsWinner {get; set;}
  public GameObject PlayerHand {get; set;}
  public int PlayerIndex {get; set;}
  public int TrumpCards {get; set;}
  public event System.EventHandler HandValueChanged;

  public Player(string playerName, GameObject playerHand)
  {
    this.PlayerName = playerName;
    this.PlayerHand = playerHand;
    HandValue = 0;
    IsPassed = false;
    IsPlayersTurn = false;
    DrawnCards = new List<Int32>();
    PlayerWins = 0;
    TrumpCards = 0;
    IsWinner = false;
  }

  public Player(string playerName)
  {
    this.PlayerName = playerName;
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

