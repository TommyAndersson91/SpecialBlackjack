using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandArranger : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public GridLayoutGroup gridLayoutGroup2;
    public float Y {get; set;}
    public float X {get; set;}
    public float Y2 { get; set; }
    public float X2 { get; set; }
    public int CardCounter {get; set;}
    public int CardCounter2 {get; set;}
    public int YCounter { get; set; }
    public int YCounter2 { get; set; }

    public float GetX(int playerIndex)
    {
        gridLayoutGroup.enabled = false;
        gridLayoutGroup2.enabled = false;
        if (playerIndex == 0)
        {
            CardCounter++;
            if (Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[0].DrawnCards.Count < 4)
            {
                X = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.x + (gridLayoutGroup.cellSize.x/2*1.22f) * CardCounter;
                // X -= gridLayoutGroup.cellSize.x/20;
                // if (CardCounter > 1)
                // {
                // X -= gridLayoutGroup.cellSize.x / 4;
                // }
                return X;
            }
            else
            {
                X = gridLayoutGroup.transform.GetChild(YCounter).position.x;
                // X -= 10f;
                YCounter++;
                return X;
            }
        }
        else
        {
            CardCounter2++;
            if (Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[1].DrawnCards.Count < 4)
            {
                X2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.x + (gridLayoutGroup2.cellSize.x/2*1.22f) * CardCounter2;
                // X2 -= gridLayoutGroup2.cellSize.x/20;
                // if (CardCounter2 > 1)
                // {
                //     X -= gridLayoutGroup2.cellSize.x / 4;
                // }
                return X2;
            }
            else
            {
                X2 = gridLayoutGroup2.transform.GetChild(YCounter2).position.x;
                // X2 -= 10f;
                YCounter2++;
                return X2;
            }
        }
    }

    public float GetY(int playerIndex)
    {
        if (playerIndex == 0)
        {
            if (Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[0].DrawnCards.Count < 4)
            {
                return Y = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.y;
            }
            else
            {
                return Y = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.y - gridLayoutGroup.cellSize.y/2;
            }
        }
        else
        {
            if (Constants.FindObjectInChilds(transform.gameObject, "GameLogic").GetComponent<GameLogic>().PlayerList.GetPlayers()[1].DrawnCards.Count < 4)
            {
                return Y2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.y;
            }
            else
            {
                return Y2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.y - gridLayoutGroup2.cellSize.y/2;
            }

        }
    }

    private void Awake() 
    {
    CardCounter = 0;
    CardCounter2 = 0;
    YCounter = 0;
    YCounter2 = 0;
    }
}
