using System.Collections;
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
    public static bool PlayingAgainstAI;
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
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;
    public TextMeshProUGUI winnerText;
    // public static int PlayerList.GetPlayers()[0].HandValue;
    // public static int PlayerList.GetPlayers()[0].HandValue;
    public static bool NewGame = true;
    public static System.Random random;
    public static PlayerList PlayerList = new PlayerList();
    public static int PassedCounter = 0;
    public static Player CurrentPlayer;
   
   

    // Start is called before the first frame update
    void Start()
    {
      
       


        PassButton.onClick.AddListener(Pass);
        DrawButton.onClick.AddListener(Draw);
        ShuffleArray(ints);
        AvaibleCards = new Stack(ints);
        if (FirstRound)
        {
            Setup();
            FirstRound = false;
        }
    // Player1Passed = false;
    // Player2Passed = false;
     
   
   
    //   player1Turn = random.Next(2) == 1;
        //För att se blandningen
        //  for (int i = 0; i < ints.Length; i++)
        //  {
        //      Debug.Log(ints[i]);
        //  }
       


        winnerText.text = CurrentPlayer.PlayerName + "'s turn";
        // if (player1Turn)
        // {
        //     winnerText.text = "Player 1's turn";
        // }
        // else
        // {
        //     winnerText.text = "Player 2's turn";
        // }

    }

    private void Update() {
        player1Score.text = PlayerList.GetPlayers()[0].PlayerName + ": " + PlayerList.GetPlayers()[0].HandValue + " / 21";
        player2Score.text = PlayerList.GetPlayers()[1].PlayerName + ": " + PlayerList.GetPlayers()[1].HandValue + " / 21";
        // if (NewGame)
        // {
        //     Setup();
        // }                                                                        
        CheckFinished();
    }

    public void Setup()
    {
        random = new System.Random();
        CurrentPlayer = new Player();
        PlayerList.AddPlayer("Player 1", GameObject.Find("CardsInHandPanel"));
        PlayerList.AddPlayer("Player 2", GameObject.Find("CardsInHandPanel2"));
        player1Hand = PlayerList.GetPlayers()[0].PlayerHand;
        player2Hand = PlayerList.GetPlayers()[1].PlayerHand;
        foreach (var item in PlayerList.GetPlayers())
        {
            Debug.Log(item.PlayerName);
        }
        CurrentPlayer = PlayerList.GetPlayers()[random.Next(PlayerList.GetPlayers().Count)];
        DrawCards();
        DrawCards();
        DrawCards();
        DrawCards();
        
    }


    public void Pass()
    {

        CurrentPlayer.IsPassed = true;

        // player1Turn = !player1Turn;

        // if  (player1Turn)
        // {
        //     Player2Passed = true;
        //     winnerText.text = "Player 1's turn";
        //     // player1Score.text = PlayerList.GetPlayers()[0].HandValue + " / 21";
        // } 
        // else 
        // {
        //     Player1Passed = true;
        //     winnerText.text = "Player 2's turn";
        //     // player2Score.text = PlayerList.GetPlayers()[0].HandValue + " / 21";
        // }
     
        // if (player1Hand.transform.childCount < 1 || player2Hand.transform.childCount < 1)
        // {
        //     firstDraw = true;
        // }
        foreach (var player in PlayerList.GetPlayers())
        {
            if (player.IsPassed)
            {
                PassedCounter++;
                if (PassedCounter == PlayerList.GetPlayers().Count)
                {
                    RoundFinished();
                }
            }
        }
        // if  (Player1Passed && Player2Passed)
        // {
        //     RoundFinished();
        // }

    }

    public void CheckFinished()
    {
        if (PassedCounter == PlayerList.GetPlayers().Count)
        {
            RoundFinished();
        }
    }

    public void DrawCards ()
    {
       NewGame = false;
        //    Debug.Log(player1Turn);
        GameObject obj = Instantiate(Resources.Load<GameObject>("1"));
        obj.transform.SetParent(CurrentPlayer.PlayerHand.transform);
        obj.transform.GetComponentInChildren<TMP_Text>().SetText(AvaibleCards.Peek().ToString());

        // GameObject nextCard = GameObject.Find("" + AvaibleCards.Peek());
        // Debug.Log(nextCard.name);
        // nextCard.transform.SetParent(CurrentPlayer.PlayerHand.transform);
        CurrentPlayer.HandValue += int.Parse(AvaibleCards.Peek().ToString());
        CurrentPlayer.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
        AvaibleCards.Pop();


        if (CurrentPlayer.HandValue >= 21)
        {
            Pass();

        }
        winnerText.text = CurrentPlayer.PlayerName + "'s turn";
        if (CurrentPlayer.PlayerName == "Player 1")
        {
            PlayerList.GetPlayers()[0] = CurrentPlayer;
            CurrentPlayer = PlayerList.GetPlayers()[1];
        }
        else
        {
            PlayerList.GetPlayers()[1] = CurrentPlayer;
            CurrentPlayer = PlayerList.GetPlayers()[0];
        }
        // if (player1Turn && !Player1Passed)
        // {
        //     nextCard.transform.SetParent(player1Hand.transform);
        //     PlayerList.GetPlayers()[0].HandValue += int.Parse(AvaibleCards.Peek().ToString());
        //     player1Score.text = PlayerList.GetPlayers()[0].HandValue + " / 21";
        //     AvaibleCards.Pop();
        // }
        // else if(!player1Turn && !Player2Passed)
        // {
        //     nextCard.transform.SetParent(player2Hand.transform);
        //     PlayerList.GetPlayers()[0].HandValue += int.Parse(AvaibleCards.Peek().ToString());
        //     player2Score.text = PlayerList.GetPlayers()[0].HandValue + " / 21";
        //     // if (PlayerList.GetPlayers()[0].HandValue > 21)
        //     // {
        //     //     Pass();
        //     // }
        //     AvaibleCards.Pop();
            // player1Turn = !player1Turn;
            // winnerText.color = Color.red;
            // winnerText.text = "Player 1's turn";
    
        // else if (!Player1Passed && !Player2Passed)
        // {
        //     player1Turn = !player1Turn;
        //     if (player1Turn)
        //     {
        //         winnerText.text = "Player 1's turn";
        //     }
        //     else
        //     {
        //         winnerText.text = "Player 2's turn";
        //     }
        // }
        // if (firstDraw)
        // {
        //     firstDraw = false;
        //     DrawCards();
        // }


        
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
        }
        else if (PlayerList.GetPlayers()[0].HandValue < 22 && PlayerList.GetPlayers()[1].HandValue > 21)
        {
            Player2Winner();
        }
        if (PlayerList.GetPlayers()[0].HandValue > 21 && PlayerList.GetPlayers()[1].HandValue > 21)
        {
            int player1Above21 = 0;
            int player2Above21 = 0;
            for (int i = PlayerList.GetPlayers()[0].HandValue; i < 21; i--)
            {
                player1Above21++;
            }
            for (int i = PlayerList.GetPlayers()[1].HandValue; i < 21; i--)
            {
                player2Above21++;
            }
            if  (player1Above21 > player2Above21)
            { 
                Player2Winner();
            }
            else if (player2Above21 > player1Above21)
            {
                Player1Winner();
            }
            else
            {
                Draw();
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
                }
                if (player2To21 > player1To21)
                {
                    Player1Winner();
                }
                if (player1To21 == player2To21)
                {
                    Draw();
                } 
                
        }
        StartCoroutine(ResetGame());

       
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(2);
        winnerText.text = "Restarting...";
        yield return new WaitForSeconds(2);
        // DrawButton.SetActive(true);
        // PassButton.SetActive(true);
        player1Hand.transform.DetachChildren();
        player2Hand.transform.DetachChildren();
        int[] ints = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        // player1Turn = true;
        ShuffleArray(ints);
        // Player1Passed = false;
        // Player2Passed = false;
        AvaibleCards = new Stack(ints);
        NewGame = true;
        foreach (var player in PlayerList.GetPlayers())
        {
            player.IsPassed = false;
            player.HandValue = 0;
        }
        Setup();
        // Start();
    }

    public void Player1Winner ()
    {
        winnerText.text = "Player 1 is the winner!";
    }

    public void Player2Winner()
    {
        winnerText.text = "Player 2 is the winner!";
    }

    public void Draw()
    {
        winnerText.text = "It's a draw!";
    }
}
