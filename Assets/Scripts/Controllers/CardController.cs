using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CardController : MonoBehaviour
{
    void Start()
    {

    }

    public IEnumerator CardAdded2(GameObject card, int currentPlayerIndex, Text StartPos, Vector3 endPos)
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

    public void DrawCard(Player CurrentPlayer, int RoundCounter, Stack AvaibleCards, Text StartPos, Vector3 endPos)
    {  
        GameObject card = Instantiate(Resources.Load<GameObject>("1"));
        if (RoundCounter == 2)
        {
            card.transform.SetParent(CurrentPlayer.PlayerHand.transform);
            card.transform.GetComponentInChildren<TMP_Text>().SetText("?");
        }
        else
        {
            card.transform.GetComponentInChildren<TMP_Text>().SetText(AvaibleCards.Peek().ToString());
            if (RoundCounter < 5)
            {
                card.transform.SetParent(CurrentPlayer.PlayerHand.transform);
            }
            AnimateCardFly(card, CurrentPlayer.PlayerIndex, RoundCounter, StartPos, endPos);
        }
        CurrentPlayer.HandValue += int.Parse(AvaibleCards.Peek().ToString());
        CurrentPlayer.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
        Cards.DrawnCards.Add(int.Parse(AvaibleCards.Peek().ToString()));
    }

    public void AnimateCardFly(GameObject card, int currentPlayerIndex, int RoundCounter, Text StartPos, Vector3 endPos)
    {
        if (RoundCounter > 4)
        {
            card.transform.SetParent(PlayerList.GetPlayers()[currentPlayerIndex].PlayerHand.transform.GetComponentInParent<Canvas>().transform);
            Debug.Log("Card: " +card + " CurrentPlayerIndex: " + currentPlayerIndex + " StartPos: " +StartPos + " EndPos: " +endPos);
            StartCoroutine(CardAdded2(card, currentPlayerIndex, StartPos, endPos));
        }
    }
}
