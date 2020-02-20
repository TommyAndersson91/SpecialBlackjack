﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class GameLogic : MonoBehaviour
{
    public static Stack AvaibleCards;
    int[] ints = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
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
    public static bool NewGame = true;
    public static System.Random random;
    public static PlayerList PlayerList = new PlayerList();
    public static int PassedCounter = 0;
    public static Player CurrentPlayer;
    public static int Index = 0;
    public static bool PlayingAgainstAI = false;
    public static int RoundCounter = 0;
    private static int HiddenValue;
    private static bool IsLastCard = true;
    private static Boolean RoundOver = false;
    private static GameObject HiddenCard;
    [SerializeField]
    private static Animator animator;
    public static bool IsCoroutineFinished = true;
    [SerializeField]
    private Text StartPos;
    public static Vector3 endPos;

    void Start()
    {
        endPos = new Vector3();
        animator = gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("anim") as RuntimeAnimatorController;
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
        else if (CurrentPlayer.PlayerIndex == 0)
        {
            TrumpCardButton.gameObject.SetActive(true);
        }
        if (CurrentPlayer.PlayerIndex == 1 && PlayingAgainstAI)
        {
            PassButton.gameObject.SetActive(false);
            DrawButton.gameObject.SetActive(false);
            PlayAIButton.gameObject.SetActive(false);
        }
        RoundScoreText.text = "Wins \n" + PlayerList.GetPlayers()[0].PlayerName + ": " + PlayerList.GetPlayers()[0].PlayerWins +
        "\n" + PlayerList.GetPlayers()[1].PlayerName + ": " + PlayerList.GetPlayers()[1].PlayerWins;  
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
        if (PlayerList.GetPlayers()[1].IsPlayersTurn && RoundCounter >= 4 && PlayingAgainstAI)
        {
            StartCoroutine(PlayAITUrn());
        }
    }
    
    public static bool FiftyPercent()
    {
        return random.Next() > (Int32.MaxValue / 2);
        // Next() returns an int in the range [0..Int32.MaxValue]
    }

    IEnumerator PlayAITUrn()
    {
        yield return new WaitForSeconds(2);
        if (AI_logic.UseTrumpCard(PlayerList.GetPlayers()[1], PlayerList.GetPlayers()[0]) && PlayerList.GetPlayers()[1].TrumpCards > 0 && FiftyPercent())
        {
            UseTrumpCard();
            if (PlayerList.GetPlayers()[0].HandValue > 21)
            {
            int numChildren = Player2TrumpCards.transform.childCount;
            Destroy(Player2TrumpCards.transform.GetChild(numChildren - 1).gameObject);
            RoundFinished();
            yield break;
            }
            yield break;
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
            StartCoroutine(EnableOrDisableButtons());
            if (CurrentPlayer.PlayerIndex == PlayerList.GetPlayers().Count - 1)
            {
                PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
                PlayerList.GetPlayers()[0].HandValue = PlayerList.GetPlayers()[0].HandValue +3;
                PlayerList.GetPlayers()[1].TrumpCards -= 1;
                int numChildren = Player2TrumpCards.transform.childCount;
                Player2TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = false;
                Player2TrumpCards.transform.GetChild(numChildren - 1).gameObject.GetComponent<Animation>().Play("animshrink");
                StartCoroutine(ShrinkTrumpCard(CurrentPlayer.PlayerIndex));
            }
            else
            {
                PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
                PlayerList.GetPlayers()[1].HandValue = PlayerList.GetPlayers()[1].HandValue + 3;
                PlayerList.GetPlayers()[0].TrumpCards -= 1;
                int numChildren = Player1TrumpCards.transform.childCount;
                Player1TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = false;
                Player1TrumpCards.transform.GetChild(numChildren - 1).GetComponent<Animation>().Play("animshrink");
                StartCoroutine(ShrinkTrumpCard(CurrentPlayer.PlayerIndex));
            }
        }
    }

    IEnumerator ShrinkTrumpCard(int playerIndex)
    {
        yield return new WaitForSeconds(1);
        if (playerIndex == PlayerList.GetPlayers().Count -1)
        {
            int numChildren = Player2TrumpCards.transform.childCount;
            Destroy(Player2TrumpCards.transform.GetChild(numChildren - 1).gameObject);
            if (!PlayerList.GetPlayers()[0].IsPassed)
            {
                CurrentPlayer = PlayerList.GetPlayers()[0];
                winnerText.text = CurrentPlayer.PlayerName + "'s turn";
            }
            yield break;
        } 
        else
        {
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
            yield break;
        }
    }

    public void Setup()
    {
        GameObject trumpCard = Instantiate(Resources.Load<GameObject>("1"));
        if (PlayerList.GetPlayers()[1].PlayerWins >= PlayerList.GetPlayers()[0].PlayerWins + 2 && PlayerList.GetPlayers()[0].TrumpCards < 3)
        {
            PlayerList.GetPlayers()[0].TrumpCards++;
            trumpCard.transform.SetParent(Player1TrumpCards.transform);
            trumpCard.gameObject.tag = "trumpcard";
        }
        else if (PlayerList.GetPlayers()[0].PlayerWins >= PlayerList.GetPlayers()[1].PlayerWins + 2 && PlayerList.GetPlayers()[1].TrumpCards < 3)
        {
            PlayerList.GetPlayers()[1].TrumpCards++;
            trumpCard.transform.SetParent(Player2TrumpCards.transform);
            trumpCard.gameObject.tag = "trumpcard";
        }
        trumpCard.transform.localScale = new Vector3(1.35f, 1.35f, 1f);
        trumpCard.transform.GetComponentInChildren<TMP_Text>().fontSize = 10;
        trumpCard.transform.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Italic;
        trumpCard.transform.GetComponentInChildren<TMP_Text>().SetText("Increase your opponents hand value by 3");
        CurrentPlayer = new Player();
        RoundOver = false;
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
        if (PlayerList.GetPlayers()[0].HandValue > 21)
            {
                RoundFinished();
                return;
            }
        else if (PlayerList.GetPlayers()[1].HandValue > 21)
            {
                RoundFinished();
                return;
            }
        if (!CurrentPlayer.IsPassed)
        {
            if (CurrentPlayer.PlayerIndex == PlayerList.GetPlayers().Count - 1 && !PlayerList.GetPlayers()[0].IsPassed)
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
        }
        playersPassed = 0;
    }
       
    IEnumerator CardAdded(GameObject card, int currentPlayerIndex)
    {
        card.transform.position = StartPos.transform.position;
        float elapsedTime = 0;
        float waitTime = 2f;
        endPos.Set(HandArranger.GetX(currentPlayerIndex), HandArranger.GetY(currentPlayerIndex), 0.0f);
        while (elapsedTime < waitTime)
        {
            card.transform.position = Vector3.Lerp(card.transform.position, endPos, elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
    
    public void AnimateCardFly(GameObject card, int currentPlayerIndex)
    {
        if (RoundCounter > 4)
        {
            card.transform.SetParent(PlayerList.GetPlayers()[currentPlayerIndex].PlayerHand.transform.GetComponentInParent<Canvas>().transform);
            StartCoroutine(CardAdded(card, currentPlayerIndex));
        }
    }

    public void DrawCards ()
    {
        StartCoroutine(EnableOrDisableButtons());
        RoundCounter++;
        NewGame = false;
        if (PlayingAgainstAI && !PlayerList.GetPlayers()[0].IsPassed)
        {
            PlayerList.GetPlayers()[1].IsPlayersTurn = !PlayerList.GetPlayers()[1].IsPlayersTurn;
        }
        if (!CurrentPlayer.IsPassed)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("1"));
            if (RoundCounter == 2)
            {
                HiddenCard = Instantiate(Resources.Load<GameObject>("1"));
                HiddenCard.transform.SetParent(CurrentPlayer.PlayerHand.transform);
                HiddenCard.transform.GetComponentInChildren<TMP_Text>().SetText("?");
            }
            else
            {
                obj.transform.GetComponentInChildren<TMP_Text>().SetText(AvaibleCards.Peek().ToString());
                if (RoundCounter < 5)
                {
                    obj.transform.SetParent(CurrentPlayer.PlayerHand.transform);
                }
                AnimateCardFly(obj, CurrentPlayer.PlayerIndex);
                Debug.Log(obj.transform.GetComponentInChildren<TMP_Text>().GetParsedText());
            }
            CurrentPlayer.HandValue += int.Parse(AvaibleCards.Peek().ToString());
            CurrentPlayer.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
            Cards.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
            AvaibleCards.Pop();
            CheckFinished();
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
        // AddLastCard();
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
        Player2TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = true;
        Player1TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = true;
        HandArranger.gridLayoutGroup.enabled = true;
        HandArranger.gridLayoutGroup2.enabled = true;
        HandArranger.X = 0;
        HandArranger.X2 = 0;
        HandArranger.YCounter = 0;
        HandArranger.YCounter2 = 0;
        HandArranger.CardCounter = 0;
        HandArranger.CardCounter2 = 0;
        IsLastCard = true;
        PassedCounter = 0;
        RoundCounter = 0;
        Cards.DrawnCards.Clear();
        FirstRound = true;
        GameObject[] cards;
        cards = GameObject.FindGameObjectsWithTag("card");
        foreach (GameObject card in cards)
        {
            Destroy(card);
        }
        StartCoroutine(EnableOrDisableButtons());
        Setup();
        yield break;
    }

    public void Player1Winner ()
    {
        PlayerList.GetPlayers()[0].IsWinner = true;;
        PlayerList.GetPlayers()[0].PlayerWins++;
        this.StopAllCoroutines();
        winnerText.text = PlayerList.GetPlayers()[0].PlayerName + " is the winner!";
        StartCoroutine(ResetGame());
        AddLastCard(CurrentPlayer.PlayerIndex);
        return;
    }

    public void Player2Winner()
    {
        PlayerList.GetPlayers()[1].IsWinner = true;
        PlayerList.GetPlayers()[1].PlayerWins++;
        this.StopAllCoroutines();
        winnerText.text = PlayerList.GetPlayers()[1].PlayerName + " is the winner!";
        StartCoroutine(ResetGame());
        AddLastCard(CurrentPlayer.PlayerIndex);
        return;
    }

    public void Draw()
    {
        winnerText.text = "It's a draw!";
        this.StopAllCoroutines();
        StartCoroutine(ResetGame());
        AddLastCard(CurrentPlayer.PlayerIndex);
        return;
    }

    public void AddLastCard(int currentPlayerIndex)
    {
        if (IsLastCard)
        {
            GameObject lastCard = Instantiate(Resources.Load<GameObject>("1"));
            lastCard.transform.GetComponentInChildren<TMP_Text>().SetText(CurrentPlayer.DrawnCards[CurrentPlayer.DrawnCards.Count - 1].ToString());
            Vector3 LastCardPos;
            LastCardPos = new Vector3(HandArranger.GetXForLast(currentPlayerIndex), HandArranger.GetY(currentPlayerIndex), 0.0f);
            // if (CurrentPlayer.PlayerIndex == 1)
            // {
            //     LastCardPos.Set(HandArranger.GetX(CurrentPlayer.PlayerIndex), HandArranger.GetY(CurrentPlayer.PlayerIndex), 0.0f);
            // }
            // endPos.Set(HandArranger.GetX(CurrentPlayer.PlayerIndex)-HandArranger.gridLayoutGroup.cellSize.x, HandArranger.GetY(CurrentPlayer.PlayerIndex), 0.0f);
            //Kanske denna 
            // endPos.Set(HandArranger.GetX(CurrentPlayer.PlayerIndex), HandArranger.GetY(CurrentPlayer.PlayerIndex), 0.0f);
            lastCard.transform.position = LastCardPos;
            lastCard.transform.SetParent(CurrentPlayer.PlayerHand.transform.GetComponentInParent<Canvas>().transform);
            IsLastCard = false;
            return;
        }
    }
    
    IEnumerator EnableOrDisableButtons()
    {
        yield return new WaitForSeconds(0.5f);
        PassButton.gameObject.SetActive(false);
        DrawButton.gameObject.SetActive(false);
        PlayAIButton.gameObject.SetActive(false);
        // if (PlayingAgainstAI && PlayerList.GetPlayers()[1].IsPlayersTurn)
        // {
        // TrumpCardButton.gameObject.SetActive(false);
        // }
        yield return new WaitUntil(() => CurrentPlayer.PlayerIndex == 0);
        PassButton.gameObject.SetActive(true);
        DrawButton.gameObject.SetActive(true);
        PlayAIButton.gameObject.SetActive(true);
    //     if (PlayerList.GetPlayers()[0].TrumpCards > 0)
    //     {
    //         TrumpCardButton.gameObject.SetActive(true);
        
    // }
    yield break;
    }
}
