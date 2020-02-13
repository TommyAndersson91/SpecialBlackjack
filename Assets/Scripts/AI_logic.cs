﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI_logic : MonoBehaviour
{


public static Boolean CalculateMove(Player AI, Player player)
{

        if (player.IsPassed && AI.HandValue < 21 && AI.HandValue < player.HandValue && player.HandValue <= 21)
        {
            return true;
        }
        if (player.IsPassed && player.HandValue < AI.HandValue)
        {
            return false;
        }

        if (player.HandValue > AI.HandValue && !player.IsPassed && player.HandValue <= 21)
        {
            return true;
        }

        if (player.IsPassed && AI.HandValue > player.HandValue)
        {
            return false;
        }
        if (player.HandValue > 21)
        {
            return false;
        }
        if (AI.HandValue <= 10)
        {
            return true;
        }
        if (AI.HandValue== 11 && !IsContainNumber(new int[] {11}))
        {
            return true;
        }
        if (AI.HandValue== 12 && !IsContainNumber(new int[] {11,10}))
        {
            return true;
        }
        if (AI.HandValue== 13 && !IsContainNumber(new int[] { 11, 10, 9 }))
        {
            return true;
        }
        if (AI.HandValue== 14 && !IsContainNumber(new int[] { 11, 10,9,8 }))
        {
            return true;
        }
        if (AI.HandValue== 15 && !IsContainNumber(new int[] { 11, 10, 9, 8, 7 }))
        {
            return true;
        }
        if (AI.HandValue== 16 && !IsContainNumber(new int[] { 11, 10, 9, 8,7,6 }))
        {
            return true;
        }
        if (AI.HandValue== 17 && !IsContainNumber(new int[] { 11, 10, 9, 8, 7, 6, 5 }))
        {
            return true;
        }
        if (AI.HandValue== 18 && !IsContainNumber(new int[] { 11, 10, 9, 8, 7, 6, 5, 4 }))
        {
            return true;
        }
        if (AI.HandValue== 19 && !IsContainNumber(new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3 }))
        {
            return true;
        }
        if (AI.HandValue== 20 && !IsContainNumber(new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 }))
        {
            return true;
        }
        else
        {
            return false;
        }
} 

public static Boolean IsContainNumber(Int32[] numbers)
{
    int counter = 0;

    foreach (var number in numbers)
    {
        if (Array.Exists(GameLogic.AvaibleCards.ToArray(), element => int.Parse(element.ToString()) == number))
        {
        Debug.Log(GameLogic.AvaibleCards.ToArray()[counter]);
          counter++;         
        }
    }

    if (counter == numbers.Length)
    {
        return false;
    }
    else
    {
        return true;
    }
     
      
}
        

}