using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList: MonoBehaviour
{
    private List<Player> Players;

    public PlayerList()
    {
        Players = new List<Player>();
    }

    public void AddPlayer(string name, GameObject playerHand)
    {
        Players.Add(new Player (name, playerHand));
       
    }
    public void AddPlayer(string name)
    {
        Players.Add(new Player(name));

    }
    public List<Player> GetPlayers()
    {
        return Players;
    }
}
