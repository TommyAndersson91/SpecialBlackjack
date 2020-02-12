﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Timers;

public class GameLogic : MonoBehaviour
{
    public static Stack AvaibleCards;
    int[] ints = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
    // public static bool player1Turn = true;
    // public static bool Player1Passed = false;
    // public static bool Player2Passed = false;
    public static bool FirstRound = true;
    [SerializeField]
    private GameObject player1Hand;
    public static GameObject player2Hand;
    [SerializeField]
    private Button PassButton;
    [SerializeField]
    private Button DrawButton;
    [SerializeField]
    private Button PlayAIButton;
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI RoundScoreText;
    public TextMeshProUGUI player2Score;
    public TextMeshProUGUI winnerText;
    // public static int PlayerList.GetPlayers()[0].HandValue;
    // public static int PlayerList.GetPlayers()[0].HandValue;
    public static bool NewGame = true;
    public static System.Random random;
    public static PlayerList PlayerList = new PlayerList();
    public static int PassedCounter = 0;
    public static Player CurrentPlayer;
    public static int Index = 0;
    public static bool PlayingAgainstAI = false;
    public static int RoundCounter = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        Cards.InitCards();
        random = new System.Random();
        PlayerList.AddPlayer("Player 1", GameObject.Find("CardsInHandPanel"));
        PlayerList.AddPlayer("Player 2", GameObject.Find("CardsInHandPanel2"));
        foreach (var player in PlayerList.GetPlayers())
        {
            player.PlayerIndex = Index;
            Index++;
        }
        player1Hand = PlayerList.GetPlayers()[0].PlayerHand;
        player2Hand = PlayerList.GetPlayers()[1].PlayerHand;  
        PassButton.onClick.AddListener(Pass);
        DrawButton.onClick.AddListener(DrawCards);
        PlayAIButton.onClick.AddListener(PlayAI);
        if (FirstRound)
        {
            Setup();
            FirstRound = false;
        }
        Index = 0;
    }

    private void Update() {
        player1Score.text = PlayerList.GetPlayers()[0].PlayerName + ": " + PlayerList.GetPlayers()[0].HandValue + " / 21";
        player2Score.text = PlayerList.GetPlayers()[1].PlayerName + ": " + PlayerList.GetPlayers()[1].HandValue + " / 21";

        RoundScoreText.autoSizeTextContainer = true;
        RoundScoreText.text = "Wins \n Player 1: " + PlayerList.GetPlayers()[0].PlayerWins +
        "\n Player 2: " + PlayerList.GetPlayers()[1].PlayerWins;
        // if (NewGame)
        // {
        //     Setup();
        // }                                                                        
        // CheckFinished();
       
    }

    public void PlayAI()
    {
        PlayerList.GetPlayers()[0].PlayerWins = 0;
        PlayerList.GetPlayers()[1].PlayerWins = 0;

        PlayerList.GetPlayers()[1].PlayerName = "MasterBot";
        PlayingAgainstAI = !PlayingAgainstAI;
        if (!PlayingAgainstAI)
        {
            PlayerList.GetPlayers()[1].PlayerName = "Player2";
        }
    }

    public void CheckAITurn()
    {
        if (PlayerList.GetPlayers()[1].IsPlayersTurn && RoundCounter >= 4)
        {
            StartCoroutine(PlayAITUrn());
        }
    }

    IEnumerator PlayAITUrn()
    {
        yield return new WaitForSeconds(2);
        if (AI_logic.CalculateMove(PlayerList.GetPlayers()[1], PlayerList.GetPlayers()[0]))
        {
            DrawCards();
            CheckAITurn();
            CheckAITurn();
            CheckAITurn();
            yield break;
        }
        else
        {
            Pass();
            yield break; 
        }
    }

    public void Setup()
    {

        CurrentPlayer = new Player();

        // CurrentPlayer.HandValue = 0;
        // CurrentPlayer.IsPassed = false;
        // CurrentPlayer.IsPlayersTurn = false;
        // foreach (var item in PlayerList.GetPlayers())
        // {
        //     Debug.Log(item.PlayerName);
        // }
        ShuffleArray(ints);
        AvaibleCards = new Stack(ints);
        CurrentPlayer = PlayerList.GetPlayers()[random.Next(PlayerList.GetPlayers().Count-1)];
        DrawCards();
        DrawCards();
        DrawCards();
        DrawCards();      
    }


    public void Pass()
    {


        foreach (var player in PlayerList.GetPlayers())
        {
            if (CurrentPlayer.PlayerName.Equals(player.PlayerName) && !player.IsPassed)
            {
                PlayerList.GetPlayers()[player.PlayerIndex].IsPassed = true;
            }
        }
        PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;

        if (CurrentPlayer.PlayerIndex == PlayerList.GetPlayers().Count-1)
        {
            PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
            CurrentPlayer = PlayerList.GetPlayers()[0];
            winnerText.text = CurrentPlayer.PlayerName + "'s turn";
        }
        else
        {
            PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
            CurrentPlayer = PlayerList.GetPlayers()[1];
            winnerText.text = CurrentPlayer.PlayerName + "'s turn";
        }

        if (PlayerList.GetPlayers()[0].IsPassed)
        {
            PlayerList.GetPlayers()[1].IsPlayersTurn = true;
            CheckAITurn();
            if (PlayerList.GetPlayers()[1].IsPassed)
            {
                CheckFinished();
            }
            else 
            {
                return;
            }
        }
        CheckFinished();     
    }

    public void CheckFinished()
    {
        int playersPassed = 0;
        foreach (var player in PlayerList.GetPlayers())
        {
            if (player.IsPassed)
            {
                playersPassed++;
             if (playersPassed == PlayerList.GetPlayers().Count)
             {
                 RoundFinished();
                 return;
             }
            }
        }
        playersPassed = 0;
        // winnerText.text = CurrentPlayer.PlayerName + "'s turn";
    }

    public void DrawCards ()
    {
        RoundCounter++;
        NewGame = false;

        if (PlayingAgainstAI && !PlayerList.GetPlayers()[0].IsPassed)
        {
            PlayerList.GetPlayers()[1].IsPlayersTurn = !PlayerList.GetPlayers()[1].IsPlayersTurn;
        }

        if (!CurrentPlayer.IsPassed)
        {
        GameObject obj = Instantiate(Resources.Load<GameObject>("1"));
        obj.transform.SetParent(CurrentPlayer.PlayerHand.transform);
        obj.transform.GetComponentInChildren<TMP_Text>().SetText(AvaibleCards.Peek().ToString());
        CurrentPlayer.HandValue += int.Parse(AvaibleCards.Peek().ToString());
        CurrentPlayer.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
        Cards.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
        AvaibleCards.Pop();
        }

        // if (CurrentPlayer.HandValue >= 21)
        // {
        //     Pass();
        // }

        if (CurrentPlayer.PlayerIndex == PlayerList.GetPlayers().Count-1 && !PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex-1].IsPassed)
        {
            PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
            if (CurrentPlayer.HandValue > 21 && PlayerList.GetPlayers()[0].HandValue <= 21)
            {
                RoundFinished();
                return;
            }
            CurrentPlayer = PlayerList.GetPlayers()[0];
        }
        else if (CurrentPlayer.PlayerIndex < PlayerList.GetPlayers().Count - 1 && !PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex+1].IsPassed)
        {
            PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
            if (CurrentPlayer.HandValue > 21 && PlayerList.GetPlayers()[1].HandValue <= 21)
            {
                RoundFinished();
                return;
            }
            CurrentPlayer = PlayerList.GetPlayers()[1];
        }

        winnerText.text = CurrentPlayer.PlayerName + "'s turn";
        CheckAITurn();
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

    public void RoundFinished()
    {
        if (PlayerList.GetPlayers()[0].HandValue < 22 && PlayerList.GetPlayers()[1].HandValue > 21)
        {
            Player1Winner();
            return;
        }
        else if (PlayerList.GetPlayers()[1].HandValue < 22 && PlayerList.GetPlayers()[0].HandValue > 21)
        {
            Player2Winner();
            return;
        }

        if (PlayerList.GetPlayers()[0].HandValue > 21 && PlayerList.GetPlayers()[1].HandValue > 21)
        {
            int player1Above21 = 0;
            int player2Above21 = 0;
            for (int i = PlayerList.GetPlayers()[0].HandValue - 1; i >= 21; i--)
            {
                player1Above21++;
            }

            for (int i = PlayerList.GetPlayers()[1].HandValue - 1; i >= 21; i--)
            {
                player2Above21++;
            }

            if  (player1Above21 > player2Above21)
            { 
                Player2Winner();
                return;
            }
            else if (player2Above21 > player1Above21)
            {
                Player1Winner();
                return;
            }
            else if (player1Above21 == player2Above21)
            {
                Draw();
                return;
            }
        }
        if (PlayerList.GetPlayers()[0].HandValue < 22 && PlayerList.GetPlayers()[1].HandValue < 22)
        {
                int player1To21 = 0;
                int player2To21 = 0;

          
                for (int i = PlayerList.GetPlayers()[0].HandValue; i < 22; i++)
                {
                    player1To21++;
                }
                for (int i = PlayerList.GetPlayers()[1].HandValue; i < 22; i++)
                {
                    player2To21++;
                }
                if  (player1To21 > player2To21)
                {
                    Player2Winner();
                    return;
                }
                if (player2To21 > player1To21)
                {
                    Player1Winner();
                    return;
                }
                if (player1To21 == player2To21)
                {
                    Draw();
                    return; 
                } 
                
        }       
    }

    IEnumerator ResetGame()
    {
        
        yield return new WaitForSeconds(4);
        winnerText.text = "Restarting...";
        foreach (var player in PlayerList.GetPlayers())
        {
            player.IsPassed = false;
            player.HandValue = 0;
            player.PlayerHand.transform.DetachChildren();
            player.DrawnCards.Clear();
            player.IsPlayersTurn = false;
        }
        // PlayingAgainstAI = false;
        PassedCounter = 0;
        RoundCounter = 0;
        Cards.DrawnCards.Clear();
        FirstRound = true;
        Setup();
        yield break;
        // player1Hand.transform.DetachChildren();
        // player2Hand.transform.DetachChildren();
        yield return new WaitForSeconds(2);
        // DrawButton.SetActive(true);
        // PassButton.SetActive(true);

        // int[] ints = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        // // // player1Turn = true;
        // ShuffleArray(ints);
        // // // Player1Passed = false;
        // // // Player2Passed = false;
        // AvaibleCards = new Stack(ints);
        // NewGame = true;
        // Start();
    }

    public void Player1Winner ()
    {
        PlayerList.GetPlayers()[0].PlayerWins++;
        winnerText.text = PlayerList.GetPlayers()[0].PlayerName + " is the winner!";
        this.StopAllCoroutines();
        StartCoroutine(ResetGame());
        return;
    }

    public void Player2Winner()
    {
        PlayerList.GetPlayers()[1].PlayerWins++;
        winnerText.text = PlayerList.GetPlayers()[1].PlayerName + " is the winner!";
        this.StopAllCoroutines();
        StartCoroutine(ResetGame());
        return;
    }

    public void Draw()
    {
        winnerText.text = "It's a draw!";
        this.StopAllCoroutines();
        StartCoroutine(ResetGame());
        return;
    }
}
