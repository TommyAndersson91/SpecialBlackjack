﻿using System.Reflection;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class GameLogic : MonoBehaviour
{
    public PlayerList PlayerList;
    public PlayerPanel playerPanel;
    public bool PlayingAgainstAI { get; set;}
    [SerializeField]
    private Button PassButton;
    [SerializeField]
    private Button DrawButton;
    [SerializeField]
    private Button PlayAIButton;
    [SerializeField]
    private Button NewGameButton;
    [SerializeField]
    private Button TrumpCardButton;
    [SerializeField]
    private PlayerPanel playerPanel_1;
    [SerializeField]
    private PlayerPanel playerPanel_2;
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI RoundScoreText;
    public TextMeshProUGUI player2Score;
    public TextMeshProUGUI winnerText;
    public System.Random random;
    public Player CurrentPlayer; 
    public int RoundCounter = 0;
    public int HiddenValue {get; set;}
    public Boolean RoundOver = false;
    public GameObject HiddenCard {get; set;}
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Text StartPos;
    public Vector3 endPos;
    int index = 0;

    public void NewGame()
    {
        CurrentPlayer = new Player();
        int index = 0;
        PlayingAgainstAI = false;
        endPos = new Vector3();
        animator = gameObject.GetComponent<Animator>();
        // animator.runtimeAnimatorController = Addressables.LoadAsset("anim") as RuntimeAnimatorController;
        gameObject.GetComponent<Cards>().InitCards();
        random = new System.Random();
        PlayerList.AddPlayer("Player 1");
        PlayerList.AddPlayer("Player 2");
        foreach (var player in PlayerList.GetPlayers())
        {
            player.PlayerIndex = index;
            index++;
        }
        playerPanel_1.SetupPanel(PlayerList.GetPlayers()[0]);
        playerPanel_2.SetupPanel(PlayerList.GetPlayers()[1]);
        Setup();
        index = 0;
    }

    public void TrumpCardLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        GameObject trumpCard;
        trumpCard = obj.Result;
        PlayerList.GetPlayers()[index].TrumpCards++;
        gameObject.GetComponent<TrumpCards>().AddRandomTrumpCard(trumpCard);
        trumpCard.transform.SetParent(playerPanel.GetTrumpCardPanel(index).transform, false);
        // trumpCard.transform.GetComponentInChildren<TMP_Text>().fontSize = 14;
        // trumpCard.transform.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Italic;
        // trumpCard.transform.GetComponentInChildren<TMP_Text>().SetText("Increase your opponents hand value by 3");
    }

    public void Setup()
    {
        if (PlayerList.GetPlayers()[1].PlayerWins >= PlayerList.GetPlayers()[0].PlayerWins + 1 && PlayerList.GetPlayers()[0].TrumpCards < 3)
        {
            index = 0;
            Addressables.InstantiateAsync("trump").Completed += TrumpCardLoaded;
        }
        else if (PlayerList.GetPlayers()[0].PlayerWins >= PlayerList.GetPlayers()[1].PlayerWins + 1 && PlayerList.GetPlayers()[1].TrumpCards < 3)
        {
            index = 1;
            Addressables.InstantiateAsync("trump").Completed += TrumpCardLoaded;
        }
        
        RoundOver = false;
        gameObject.GetComponent<CardController>().ShuffleArray(gameObject.GetComponent<CardController>().getInts());
        gameObject.GetComponent<Cards>().AvaibleCards = new Stack(gameObject.GetComponent<CardController>().getInts());
        gameObject.GetComponent<GameLogic>().CurrentPlayer = PlayerList.GetPlayers()[0];
        for (int i = 0; i < 4; i++)
        {
            DrawCards();
        }
        CurrentPlayer = PlayerList.GetPlayers()[0];
        PlayerList.GetPlayers()[1].IsPlayersTurn = false;
        UpdateUI();
    }

    void Start()
    {
        gameObject.AddComponent<HandArranger>();
        gameObject.AddComponent<TrumpCards>();
        PlayerList = gameObject.AddComponent<PlayerList>();
        gameObject.AddComponent<PlayerPanel>();
        gameObject.AddComponent<AIController>();
        gameObject.AddComponent<CardController>();
        gameObject.AddComponent<AI_logic>();
        gameObject.AddComponent<Cards>();
        NewGameButton.onClick.AddListener(NewGame);
        PassButton.onClick.AddListener(Pass);
        DrawButton.onClick.AddListener(DrawCards);
        PlayAIButton.onClick.AddListener(gameObject.GetComponent<AIController>().PlayAI);
        TrumpCardButton.onClick.AddListener(UseTrumpCard);
    }

    public int GetHiddenValue()
    {
        HiddenValue = PlayerList.GetPlayers()[1].HandValue - PlayerList.GetPlayers()[1].DrawnCards[0];
        return HiddenValue;
    }

    public void UpdateUI()
    {
        if (RoundOver)
        {
            playerPanel_2.SetPlayer2ScoreText(PlayerList.GetPlayers()[1].PlayerName, PlayerList.GetPlayers()[1].HandValue);
        }
        if (RoundCounter >= 2)
        {
            HiddenValue = PlayerList.GetPlayers()[1].HandValue - PlayerList.GetPlayers()[1].DrawnCards[0];
        }
        if (!RoundOver && RoundCounter >= 4)
        {
            winnerText.SetText(CurrentPlayer.PlayerName + "'s turn");
        }
        else
        {
            if (RoundCounter >= 4)
            {
            HiddenCard.transform.GetComponentInChildren<TMP_Text>().SetText(PlayerList.GetPlayers()[1].DrawnCards[0].ToString());
            }
        }
        if (CurrentPlayer.TrumpCards == 0)
        {
            TrumpCardButton.gameObject.SetActive(false);
        }
        else if (CurrentPlayer.PlayerIndex == 0 && PlayerList.GetPlayers()[0].TrumpCards > 0)
        {
            TrumpCardButton.gameObject.SetActive(true);
        }
        if (CurrentPlayer.PlayerIndex == 0 || CurrentPlayer.PlayerIndex == 1 && !RoundOver && !PlayingAgainstAI)
        {
            PassButton.gameObject.SetActive(true);
            DrawButton.gameObject.SetActive(true);
            PlayAIButton.gameObject.SetActive(true);
        }
        if (CurrentPlayer.PlayerIndex == 1 && PlayingAgainstAI || RoundOver)
        {
            TrumpCardButton.gameObject.SetActive(false);
            PassButton.gameObject.SetActive(false);
            DrawButton.gameObject.SetActive(false);
            PlayAIButton.gameObject.SetActive(false);
        }
    }

    public IEnumerator PlayAITUrn()
    {
        yield return new WaitForSeconds(2);
        if (gameObject.GetComponent<AI_logic>().UseTrumpCard(PlayerList.GetPlayers()[1], PlayerList.GetPlayers()[0]) && PlayerList.GetPlayers()[1].TrumpCards > 0 && gameObject.GetComponent<AIController>().FiftyPercent(random))
        {
            if (PlayerList.GetPlayers()[0].HandValue < 22)
            {
            UseTrumpCard();
            if (PlayerList.GetPlayers()[0].HandValue > 21 && !RoundOver)
            {
            RoundFinished();
            yield break;
            }
            }
            yield break;
        }
        if (gameObject.GetComponent<AI_logic>().CalculateMove(PlayerList.GetPlayers()[1], PlayerList.GetPlayers()[0]))
        {
            DrawCards();
            if (PlayerList.GetPlayers()[0].IsPassed && !RoundOver)
            {
                gameObject.GetComponent<AIController>().gameObject.GetComponent<AIController>().CheckAITurn(RoundCounter);
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
        TrumpCardButton.gameObject.SetActive(false);
        if (CurrentPlayer.TrumpCards > 0)
        {
        GetComponent<CardController>().UseTrumpCard(CurrentPlayer, playerPanel.GetTrumpCardPanel(0), playerPanel.GetTrumpCardPanel(1));
        }
        if (PlayingAgainstAI && !RoundOver && PlayerList.GetPlayers()[0].IsPassed)
        {
            PlayerList.GetPlayers()[1].IsPlayersTurn = true;
            gameObject.GetComponent<AIController>().CheckAITurn(RoundCounter);
        }
        UpdateUI();
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
            gameObject.GetComponent<AIController>().CheckAITurn(RoundCounter);
        }
        if (!RoundOver)
        {
        CheckFinished();
        }
        UpdateUI();   
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
        UpdateUI();
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
                PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
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
                PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
                CurrentPlayer = PlayerList.GetPlayers()[1];
                if (RoundCounter > 4)
                {
                gameObject.GetComponent<AIController>().CheckAITurn(RoundCounter);
                }
                return;
            } 
            return;  
        }
        return;
    }

    public void DrawCards()
    {
        Debug.Log("Draw Card Pressed");
        if (!CurrentPlayer.IsPassed && !RoundOver)
        {
            Addressables.InstantiateAsync("blankcard").Completed += OnLoadDone;
        }
    }

    public void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        Debug.Log("New card instantiated");
        RoundCounter++;
        gameObject.GetComponent<CardController>().DrawCard(obj.Result ,RoundCounter, StartPos, endPos);
        UpdateUI();

    }

    public void RoundFinished()
    {
        RoundOver = true;
        UpdateUI();
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
        return;
        }

    IEnumerator ResetGame()
    {
        HiddenValue = 0;
        RoundCounter = 0;
        yield return new WaitForSeconds(3);
        foreach (var player in PlayerList.GetPlayers())
        {
            player.IsPassed = false;
            player.HandValue = 0;
            foreach (Transform child in player.PlayerHand.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            player.DrawnCards.Clear();
            player.IsPlayersTurn = false;
            player.IsWinner = false;
        }
        gameObject.GetComponent<HandArranger>().gridLayoutGroup.enabled = true;
        gameObject.GetComponent<HandArranger>().gridLayoutGroup2.enabled = true;
        gameObject.GetComponent<HandArranger>().X = 0;
        gameObject.GetComponent<HandArranger>().X2 = 0;
        gameObject.GetComponent<HandArranger>().YCounter = 0;
        gameObject.GetComponent<HandArranger>().YCounter2 = 0;
        gameObject.GetComponent<HandArranger>().CardCounter = 0;
        gameObject.GetComponent<HandArranger>().CardCounter2 = 0;
        gameObject.GetComponent<CardController>().GetDrawnCards().Clear();
        for (int i = 0; i < winnerText.transform.childCount; i++)
        {
            Destroy(winnerText.gameObject.transform.GetChild(i).gameObject);
        }
        Setup();
        yield break;
    }

    public void Player1Winner ()
    {  
        PlayerList.GetPlayers()[0].PlayerWins++;
        PlayerList.GetPlayers()[0].IsWinner = true;
        StopAllCoroutines();
        winnerText.SetText(PlayerList.GetPlayers()[0].PlayerName + " is the winner!");
        StartCoroutine(ResetGame());
    }

    public void Player2Winner()
    {
        PlayerList.GetPlayers()[1].PlayerWins++;
        PlayerList.GetPlayers()[1].IsWinner = true;
        StopAllCoroutines();
        winnerText.SetText(PlayerList.GetPlayers()[1].PlayerName + " is the winner!");
        StartCoroutine(ResetGame());
    }

    public void Draw()
    {
        StopAllCoroutines();
        winnerText.SetText("It's a draw!");
        StartCoroutine(ResetGame());
    }
}