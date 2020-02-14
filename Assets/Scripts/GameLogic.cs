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
    // public static bool Player1Passed = false;
    // public static bool Player2Passed = false;
    public static bool FirstRound = true;
    [SerializeField]
    private GameObject player1Hand;
    [SerializeField]
    private GameObject Player1TrumpCards;
    [SerializeField]
    private GameObject Player2TrumpCards;
    public static GameObject player2Hand;
    [SerializeField]
    private Button PassButton;
    [SerializeField]
    private Button DrawButton;
    [SerializeField]
    private Button PlayAIButton;
    [SerializeField]
    private Button TrumpCardButton;
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
    private static int HiddenValue;
    private static Boolean RoundOver = false;
    private static GameObject HiddenCard;

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
        TrumpCardButton.onClick.AddListener(UseTrumpCard);
    
        if (FirstRound)
        {
            Setup();
            FirstRound = false;
        }
        Index = 0;
    }

    private void Update() {
        HiddenValue = PlayerList.GetPlayers()[1].HandValue - PlayerList.GetPlayers()[1].DrawnCards[0];

        player1Score.text = PlayerList.GetPlayers()[0].PlayerName + ": " + PlayerList.GetPlayers()[0].HandValue + " / 21";
        if (!RoundOver)
        {
            winnerText.text = CurrentPlayer.PlayerName + "'s turn";
            player2Score.text = PlayerList.GetPlayers()[1].PlayerName + ": ? + " + HiddenValue + " / 21";
        }
        else
        {
            player2Score.text = PlayerList.GetPlayers()[1].PlayerName + ": " + PlayerList.GetPlayers()[1].HandValue + " / 21";
            HiddenCard.transform.GetComponentInChildren<TMP_Text>().SetText(PlayerList.GetPlayers()[1].DrawnCards[0].ToString());
        }

        if (CurrentPlayer.TrumpCards == 0)
        {
            TrumpCardButton.gameObject.SetActive(false);
        }
        else
        {
            TrumpCardButton.gameObject.SetActive(true);
        }
        // RoundScoreText.autoSizeTextContainer = true;
        RoundScoreText.text = "Wins \n" + PlayerList.GetPlayers()[0].PlayerName + ": " + PlayerList.GetPlayers()[0].PlayerWins +
        "\n" + PlayerList.GetPlayers()[1].PlayerName + ": " + PlayerList.GetPlayers()[1].PlayerWins;
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
            PlayerList.GetPlayers()[1].PlayerName = "Player 2";
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
        if (AI_logic.UseTrumpCard(PlayerList.GetPlayers()[1], PlayerList.GetPlayers()[0]) && PlayerList.GetPlayers()[1].TrumpCards > 0)
        {
            UseTrumpCard();
            if (PlayerList.GetPlayers()[0].HandValue > 21)
            {
            RoundFinished();
            }
        }

        if (AI_logic.CalculateMove(PlayerList.GetPlayers()[1], PlayerList.GetPlayers()[0]))
        {
            DrawCards();
            if (PlayerList.GetPlayers()[0].IsPassed && !RoundOver)
            {
                CheckAITurn();
            }
            yield break;
        }
        else
        {
            Pass();
            yield break; 
        }
    }

    public void UseTrumpCard()
    {
        if (CurrentPlayer.TrumpCards > 0)
        {
            if (CurrentPlayer.PlayerIndex == PlayerList.GetPlayers().Count - 1)
            {
                // var lastIndex = PlayerList.GetPlayers()[0].DrawnCards[PlayerList.GetPlayers()[0].DrawnCards.Count - 1];
                // PlayerList.GetPlayers()[1].HandValue += int.Parse(PlayerList.GetPlayers()[0].DrawnCards[lastIndex].ToString());
                // GameObject obj = Instantiate(Resources.Load<GameObject>("1"));
                // obj.transform.SetParent(CurrentPlayer.PlayerHand.transform);
                // obj.transform.GetComponentInChildren<TMP_Text>().SetText(PlayerList.GetPlayers()[0].DrawnCards[lastIndex].ToString());
                PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
                PlayerList.GetPlayers()[0].HandValue = PlayerList.GetPlayers()[0].HandValue +3;
                PlayerList.GetPlayers()[1].TrumpCards -= 1;
                int numChildren = Player2TrumpCards.transform.childCount;
                Destroy(Player2TrumpCards.transform.GetChild(numChildren - 1).gameObject);
                if (!PlayerList.GetPlayers()[0].IsPassed)
                {
                CurrentPlayer = PlayerList.GetPlayers()[0];
                winnerText.text = CurrentPlayer.PlayerName + "'s turn";
                }
            }
            else
            {
                // var lastIndex = PlayerList.GetPlayers()[1].DrawnCards[PlayerList.GetPlayers()[1].DrawnCards.Count - 1];
                // PlayerList.GetPlayers()[0].HandValue += int.Parse(PlayerList.GetPlayers()[1].DrawnCards[lastIndex].ToString());
                // GameObject obj = Instantiate(Resources.Load<GameObject>("1"));
                // obj.transform.SetParent(CurrentPlayer.PlayerHand.transform);
                // obj.transform.GetComponentInChildren<TMP_Text>().SetText(PlayerList.GetPlayers()[1].DrawnCards[lastIndex].ToString());
                // PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
                // CurrentPlayer = PlayerList.GetPlayers()[1];
                // winnerText.text = CurrentPlayer.PlayerName + "'s turn";

                PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
                PlayerList.GetPlayers()[1].HandValue = PlayerList.GetPlayers()[1].HandValue + 3;
                PlayerList.GetPlayers()[0].TrumpCards -= 1;
                int numChildren = Player1TrumpCards.transform.childCount;
                Destroy(Player1TrumpCards.transform.GetChild(numChildren - 1).gameObject);
                if (!PlayerList.GetPlayers()[1].IsPassed)
                {
                CurrentPlayer = PlayerList.GetPlayers()[1];
                winnerText.text = CurrentPlayer.PlayerName + "'s turn";
                }
                if (PlayingAgainstAI)
                {
                PlayerList.GetPlayers()[1].IsPlayersTurn = true;
                CheckAITurn();
                }
            }
        }

    }

    public void Setup()
    {
        if (PlayerList.GetPlayers()[1].PlayerWins >= PlayerList.GetPlayers()[0].PlayerWins +2 && PlayerList.GetPlayers()[0].TrumpCards < 3 || PlayerList.GetPlayers()[0].PlayerWins >= PlayerList.GetPlayers()[1].PlayerWins + 2 && PlayerList.GetPlayers()[1].TrumpCards < 3)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("1"));

            if (PlayerList.GetPlayers()[1].PlayerWins >= PlayerList.GetPlayers()[0].PlayerWins + 2)
            {
               PlayerList.GetPlayers()[0].TrumpCards++;
                obj.transform.SetParent(Player1TrumpCards.transform);
            }
            else if (PlayerList.GetPlayers()[0].PlayerWins >= PlayerList.GetPlayers()[1].PlayerWins + 2)
            {
                PlayerList.GetPlayers()[1].TrumpCards++;
                obj.transform.SetParent(Player2TrumpCards.transform);
            }

           
            obj.transform.localScale = new Vector3(1.35f, 1.35f, 1f);
            obj.transform.GetComponentInChildren<TMP_Text>().fontSize = 10;
            // obj.transform.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Bold;
            obj.transform.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Italic;
            obj.transform.GetComponentInChildren<TMP_Text>().SetText("Increase your opponents hand value by 3");
            
        }
        CurrentPlayer = new Player();
        RoundOver = false;
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

        Debug.Log("Player 1 index: " + PlayerList.GetPlayers()[0].PlayerIndex);
        Debug.Log("Player 2 index: " + PlayerList.GetPlayers()[1].PlayerIndex);

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
            
        }
        else
        {
            PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
            CurrentPlayer = PlayerList.GetPlayers()[1];
        }

        if (PlayerList.GetPlayers()[0].IsPassed)
        {
            PlayerList.GetPlayers()[1].IsPlayersTurn = true;
            CheckAITurn();
            if (PlayerList.GetPlayers()[1].IsPassed)
            {
                CheckFinished();
                return;
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

        if (RoundCounter == 2)
        {
            HiddenCard = Instantiate(Resources.Load<GameObject>("1"));
            HiddenCard.transform.SetParent(CurrentPlayer.PlayerHand.transform);
            HiddenCard.transform.GetComponentInChildren<TMP_Text>().SetText("?");
                
        }
        else
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("1"));
            obj.transform.SetParent(CurrentPlayer.PlayerHand.transform);
            obj.transform.GetComponentInChildren<TMP_Text>().SetText(AvaibleCards.Peek().ToString());
        }
        CurrentPlayer.HandValue += int.Parse(AvaibleCards.Peek().ToString());
        CurrentPlayer.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
        Cards.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
        AvaibleCards.Pop();
        }

        // if (CurrentPlayer.HandValue >= 21)
        // {
        //     Pass();
        // }

        if (CurrentPlayer.PlayerIndex == PlayerList.GetPlayers().Count-1 && !PlayerList.GetPlayers()[0].IsPassed)
        {
            PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
            if (CurrentPlayer.HandValue > 21 && PlayerList.GetPlayers()[0].HandValue <= 21)
            {
                RoundFinished();
                return;
            }
            CurrentPlayer = PlayerList.GetPlayers()[0];
        }
        else if (CurrentPlayer.PlayerIndex < PlayerList.GetPlayers().Count - 1 && !PlayerList.GetPlayers()[1].IsPassed)
        {
            PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
            if (CurrentPlayer.HandValue > 21 && PlayerList.GetPlayers()[1].HandValue <= 21)
            {
                RoundFinished();
                return;
            }
                PlayerList.GetPlayers()[1].IsPlayersTurn = true;
                CurrentPlayer = PlayerList.GetPlayers()[1];
                CheckAITurn();
        }
        
        if (PlayerList.GetPlayers()[0].HandValue > 21)
        {
            RoundFinished(); 
        }
       
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
        RoundOver = true;
        yield return new WaitForSeconds(4);
        foreach (var player in PlayerList.GetPlayers())
        {
            player.IsPassed = false;
            player.HandValue = 0;
            player.PlayerHand.transform.DetachChildren();
            player.DrawnCards.Clear();
            player.IsPlayersTurn = false;
            player.IsWinner = false;
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
        PlayerList.GetPlayers()[0].IsWinner = true;;
        PlayerList.GetPlayers()[0].PlayerWins++;
        this.StopAllCoroutines();
        winnerText.text = PlayerList.GetPlayers()[0].PlayerName + " is the winner!";
        
        StartCoroutine(ResetGame());
        return;
    }

    public void Player2Winner()
    {
        PlayerList.GetPlayers()[1].IsWinner = true;
        PlayerList.GetPlayers()[1].PlayerWins++;
        this.StopAllCoroutines();
        winnerText.text = PlayerList.GetPlayers()[1].PlayerName + " is the winner!";
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
