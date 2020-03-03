using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
   private string _playerName;
   private int _frame;
   private int _trinket;

    public string PlayerName
   {
       get { return _playerName; }
       set { _playerName = value; }
   }
   
   public int Frame
   {
       get { return _frame; }
       set { _frame = value; }
   }
   
   public int Trinket
   {
       get { return _trinket; }
       set { _trinket = value; }
   }
   
   
   
}
