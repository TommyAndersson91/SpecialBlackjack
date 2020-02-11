using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    private static List<Player> Players;

    public PlayerList()
    {
        Players = new List<Player>();
    }

    public static void AddPlayer(string name, GameObject playerHand)
    {
        Players.Add(new Player (name, playerHand));
       
    }
    public static List<Player> GetPlayers()
    {
        return Players;
    }
}
