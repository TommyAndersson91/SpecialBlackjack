using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
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

    public void NewGame()
    {
        CurrentPlayer = new Player();
        int index = 0;
        // winnerText = GameObject.Find("WinnerText").GetComponentInChildren<TextMeshProUGUI>();
        PlayingAgainstAI = false;
        gameObject.AddComponent<AIController>();
        gameObject.AddComponent<CardController>();
        gameObject.AddComponent<AI_logic>();
        gameObject.AddComponent<Cards>();
        gameObject.AddComponent<UIController>();
        PlayerList = gameObject.AddComponent<PlayerList>();
        gameObject.AddComponent<HandArranger>();
        gameObject.AddComponent<Player>();
        gameObject.AddComponent<PlayerPanel>();
        endPos = new Vector3();
        animator = gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("anim") as RuntimeAnimatorController;
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
        PassButton.onClick.AddListener(Pass);
        DrawButton.onClick.AddListener(DrawCards);
        PlayAIButton.onClick.AddListener(gameObject.GetComponent<AIController>().PlayAI);
        TrumpCardButton.onClick.AddListener(UseTrumpCard);
        Setup();
        index = 0;
        // gameObject.GetComponent<HandArranger>().gridLayoutGroup = Constants.FindObjectInChilds(transform.gameObject.GetComponent<PlayerPanel>().gameObject, "CardsInHandPanel").GetComponentInChildren<GridLayoutGroup>();
        // gameObject.GetComponent<HandArranger>().gridLayoutGroup2 = Constants.FindObjectInChilds(transform.gameObject.GetComponent<PlayerPanel>().gameObject, "CardsInHandPanel2").GetComponentInChildren<GridLayoutGroup>();
    }

    public void Setup()
    {
        GameObject trumpCard = Instantiate(Resources.Load<GameObject>("1"));
        if (PlayerList.GetPlayers()[1].PlayerWins >= PlayerList.GetPlayers()[0].PlayerWins + 2 && PlayerList.GetPlayers()[0].TrumpCards < 3)
        {
            PlayerList.GetPlayers()[0].TrumpCards++;
            trumpCard.transform.SetParent(playerPanel.GetTrumpCardPanel(0).transform);
            trumpCard.gameObject.tag = "trumpcard";
        }
        else if (PlayerList.GetPlayers()[0].PlayerWins >= PlayerList.GetPlayers()[1].PlayerWins + 2 && PlayerList.GetPlayers()[1].TrumpCards < 3)
        {
            PlayerList.GetPlayers()[1].TrumpCards++;
            trumpCard.transform.SetParent(playerPanel.GetTrumpCardPanel(1).transform);
            trumpCard.gameObject.tag = "trumpcard";
        }
        trumpCard.transform.localScale = new Vector3(1.35f, 1.35f, 1f);
        trumpCard.transform.GetComponentInChildren<TMP_Text>().fontSize = 10;
        trumpCard.transform.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Italic;
        trumpCard.transform.GetComponentInChildren<TMP_Text>().SetText("Increase your opponents hand value by 3");
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
        HiddenValue = PlayerList.GetPlayers()[1].HandValue - PlayerList.GetPlayers()[1].DrawnCards[0];
        UpdateUI();
    }

    void Start()
    {
      NewGameButton.onClick.AddListener(NewGame);
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
        RoundCounter++;
        if (!CurrentPlayer.IsPassed && !RoundOver)
        {
            gameObject.GetComponent<CardController>().DrawCard(RoundCounter, StartPos, endPos);
            CheckFinished();
        }
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
            player.PlayerHand.transform.DetachChildren();
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

//OLD CODE FROM BEFORE REFACTORING

// public void PlayAI()
// {
//     PlayerList.GetPlayers()[0].PlayerWins = 0;
//     PlayerList.GetPlayers()[1].PlayerWins = 0;

//     PlayerList.GetPlayers()[1].PlayerName = "MasterBot";
//     PlayingAgainstAI = !PlayingAgainstAI;
//     if (!PlayingAgainstAI)
//     {
//         PlayerList.GetPlayers()[1].PlayerName = "Player 2";
//     }
// }

// public void gameObject.GetComponent<AIController>().CheckAITurn(RoundCounter);// {
//     if (PlayerList.GetPlayers()[1].IsPlayersTurn && RoundCounter >= 4 && PlayingAgainstAI)
//     {
//         StartCoroutine(PlayAITUrn());
//     }
// }

// public static bool FiftyPercent()
// {
//     return random.Next() > (Int32.MaxValue / 2);
//     // Next() returns an int in the range [0..Int32.MaxValue]
// }

// public void UseTrumpCard()
// {
//     if (CurrentPlayer.TrumpCards > 0)
//     {
//         StartCoroutine(EnableOrDisableButtons());
//         if (CurrentPlayer.PlayerIndex == PlayerList.GetPlayers().Count - 1)
//         {
//             PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
//             PlayerList.GetPlayers()[0].HandValue = PlayerList.GetPlayers()[0].HandValue +3;
//             PlayerList.GetPlayers()[1].TrumpCards -= 1;
//             int numChildren = Player2TrumpCards.transform.childCount;
//             Player2TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = false;
//             Player2TrumpCards.transform.GetChild(numChildren - 1).gameObject.GetComponent<Animation>().Play("animshrink");
//             StartCoroutine(ShrinkTrumpCard(CurrentPlayer.PlayerIndex));
//             CurrentPlayer = PlayerList.GetPlayers()[0];
//         }
//         else
//         {
//             PlayerList.GetPlayers()[CurrentPlayer.PlayerIndex] = CurrentPlayer;
//             PlayerList.GetPlayers()[1].HandValue = PlayerList.GetPlayers()[1].HandValue + 3;
//             PlayerList.GetPlayers()[0].TrumpCards -= 1;
//             int numChildren = Player1TrumpCards.transform.childCount;
//             Player1TrumpCards.GetComponentInChildren<GridLayoutGroup>().enabled = false;
//             Player1TrumpCards.transform.GetChild(numChildren - 1).GetComponent<Animation>().Play("animshrink");
//             StartCoroutine(ShrinkTrumpCard(CurrentPlayer.PlayerIndex));
//             CurrentPlayer = PlayerList.GetPlayers()[1];
//         }
//     }
// }

// IEnumerator ShrinkTrumpCard(int playerIndex)
// {
//     yield return new WaitForSeconds(1);
//     if (playerIndex == PlayerList.GetPlayers().Count -1)
//     {
//         int numChildren = Player2TrumpCards.transform.childCount;
//         Destroy(Player2TrumpCards.transform.GetChild(numChildren - 1).gameObject);
//         if (!PlayerList.GetPlayers()[0].IsPassed)
//         {
//             CurrentPlayer = PlayerList.GetPlayers()[0];
//             winnerText.text = CurrentPlayer.PlayerName + "'s turn";
//         }
//         yield break;
//     } 
//     else
//     {
//         int numChildren = Player1TrumpCards.transform.childCount;
//         Destroy(Player1TrumpCards.transform.GetChild(numChildren - 1).gameObject);
//         if (!PlayerList.GetPlayers()[1].IsPassed)
//         {
//             CurrentPlayer = PlayerList.GetPlayers()[1];
//             winnerText.text = CurrentPlayer.PlayerName + "'s turn";
//         }
//         if (PlayingAgainstAI)
//         {
//             PlayerList.GetPlayers()[1].IsPlayersTurn = true;
//             gameObject.GetComponent<AIController>().CheckAITurn(RoundCounter);
//         }
//         yield break;
//     }
// }

// public void AddLastCard(int currentPlayerIndex)
// {
//     if (IsLastCard)
//     {
//         GameObject lastCard = Instantiate(Resources.Load<GameObject>("1"));
//         lastCard.transform.GetComponentInChildren<TMP_Text>().SetText(CurrentPlayer.DrawnCards[CurrentPlayer.DrawnCards.Count - 1].ToString());
//         Vector3 LastCardPos;
//         if (PlayerList.GetPlayers()[currentPlayerIndex].DrawnCards.Count > 4)
//         {
//             LastCardPos = new Vector3(HandArranger.GetXForLast(currentPlayerIndex) - HandArranger.gridLayoutGroup.cellSize.x, HandArranger.GetY(currentPlayerIndex), 0.0f);
//         }
//         else
//         {
//             LastCardPos = new Vector3(HandArranger.GetXForLast(currentPlayerIndex), HandArranger.GetY(currentPlayerIndex), 0.0f);
//         }
//         lastCard.transform.position = LastCardPos;
//         lastCard.transform.SetParent(CurrentPlayer.PlayerHand.transform.GetComponentInParent<Canvas>().transform);
//         IsLastCard = false;
//         return;
//     }
// }

// IEnumerator CardAdded(GameObject card, int currentPlayerIndex)
// {
//     card.transform.position = StartPos.transform.position;
//     float elapsedTime = 0;
//     float waitTime = 2f;
//     endPos.Set(HandArranger.GetX(currentPlayerIndex), HandArranger.GetY(currentPlayerIndex), 0.0f);
//     while (elapsedTime < waitTime)
//     {
//         card.transform.position = Vector3.Lerp(card.transform.position, endPos, elapsedTime / waitTime);
//         elapsedTime += Time.deltaTime;
//         yield return null;
//     }
//     yield break;
// }

// public void AnimateCardFly(GameObject card, int currentPlayerIndex)
// {
//     if (RoundCounter > 4)
//     {
//         card.transform.SetParent(PlayerList.GetPlayers()[currentPlayerIndex].PlayerHand.transform.GetComponentInParent<Canvas>().transform);
//         StartCoroutine(CardAdded(card, currentPlayerIndex));
//     }
// }

// public void DrawCards ()
// {
//     StartCoroutine(EnableOrDisableButtons());
//     RoundCounter++;
//     if (PlayingAgainstAI && !PlayerList.GetPlayers()[0].IsPassed)
//     {
//         PlayerList.GetPlayers()[1].IsPlayersTurn = !PlayerList.GetPlayers()[1].IsPlayersTurn;
//     }
//     if (!CurrentPlayer.IsPassed)
//     {
//         GameObject obj = Instantiate(Resources.Load<GameObject>("1"));
//         if (RoundCounter == 2)
//         {
//             HiddenCard = Instantiate(Resources.Load<GameObject>("1"));
//             HiddenCard.transform.SetParent(CurrentPlayer.PlayerHand.transform);
//             HiddenCard.transform.GetComponentInChildren<TMP_Text>().SetText("?");
//         }
//         else
//         {
//             obj.transform.GetComponentInChildren<TMP_Text>().SetText(AvaibleCards.Peek().ToString());
//             if (RoundCounter < 5)
//             {
//                 obj.transform.SetParent(CurrentPlayer.PlayerHand.transform);
//             }
//             AnimateCardFly(obj, CurrentPlayer.PlayerIndex);
//         }
//         CurrentPlayer.HandValue += int.Parse(AvaibleCards.Peek().ToString());
//         CurrentPlayer.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
//         Cards.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
//         AvaibleCards.Pop();
//         CheckFinished();
//     } 
// }

// IEnumerator EnableOrDisableButtons()
// {
//     // yield return new WaitForSeconds(0.5f);
//     PassButton.gameObject.SetActive(false);
//     DrawButton.gameObject.SetActive(false);
//     PlayAIButton.gameObject.SetActive(false);
//     //  yield return new WaitUntil(() => CurrentPlayer.PlayerIndex == 0);
//     yield return new WaitForSeconds(0.5f);
//     PassButton.gameObject.SetActive(true);
//     DrawButton.gameObject.SetActive(true);
//     PlayAIButton.gameObject.SetActive(true);
//     yield break;
// }

// public void ShuffleArray(int[] a)
// {
//     int length = a.Length;
//     for (int i = 0; i < length; i++)
//     {
//         Swap(a, i, i + random.Next(length - i));
//     }
// }

// public static void Swap(int[] arr, int a, int b)
// {
//     int temp = arr[a];
//     arr[a] = arr[b];
//     arr[b] = temp;
// }