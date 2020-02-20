using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandArranger : MonoBehaviour
{
public static GridLayoutGroup gridLayoutGroup;
public static GridLayoutGroup gridLayoutGroup2;
public Vector2 vector;
public float zValue = 1f;
public static float Y {get; set;}
public static float X {get; set;}
public static float Y2 { get; set; }
public static float X2 { get; set; }
public static int CardCounter {get; set;}
public static int CardCounter2 {get; set;}
public static int YCounter { get; set; }
public static int YCounter2 { get; set; }

public static float GetXForLast(int playerIndex)
{
    if (playerIndex == 0)
    {
        if (PlayerList.GetPlayers()[0].DrawnCards.Count < 4)
        {

            X = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.x + gridLayoutGroup.cellSize.x * CardCounter;
            return X;
        }
        else
        {
            X = gridLayoutGroup.transform.GetChild(YCounter).position.x;
            return X;
        }
    }
    else
    {
        if (PlayerList.GetPlayers()[1].DrawnCards.Count < 4)
        {
            X2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.x + gridLayoutGroup2.cellSize.x * CardCounter2;
            return X2;
        }
        else
        {
            X2 = gridLayoutGroup2.transform.GetChild(YCounter2).position.x;
            return X2;
        }
    }
}

public static float GetX(int playerIndex)
{
    gridLayoutGroup.enabled = false;
    gridLayoutGroup2.enabled = false;
    if (playerIndex == 0)
    {
        CardCounter++;
        if (PlayerList.GetPlayers()[0].DrawnCards.Count < 4)
        {
           
            return X = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.x+gridLayoutGroup.cellSize.x*CardCounter;
        }
        else
        {
            X = gridLayoutGroup.transform.GetChild(YCounter).position.x;
            YCounter++;
            return X;
        }
    }
    else
    {
        CardCounter2++;
        if (PlayerList.GetPlayers()[1].DrawnCards.Count < 4)
        {
            return X2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.x+gridLayoutGroup2.cellSize.x*CardCounter2;
        }
        else
        {
            X2 = gridLayoutGroup2.transform.GetChild(YCounter2).position.x;
            YCounter2++;
            return X2;
        }
    }
}

public static float GetY(int playerIndex)
{
    gridLayoutGroup.enabled = false;
    gridLayoutGroup2.enabled = false;
    if (playerIndex == 0)
    {
        if (PlayerList.GetPlayers()[0].DrawnCards.Count < 4)
        {
            return Y = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.y;
        }
        else
        {
            return Y = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.y-gridLayoutGroup.cellSize.y;
        }
    }
    else
    {
        if (PlayerList.GetPlayers()[1].DrawnCards.Count < 4)
        {
            return Y2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.y;
        }
        else
        {
            return Y2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.y-gridLayoutGroup2.cellSize.y;
        }

    }
}

private void Awake() {

    // Arrange();
    gridLayoutGroup = GameObject.Find("CardsInHandPanel").GetComponentInChildren<GridLayoutGroup>();
    gridLayoutGroup2 = GameObject.Find("CardsInHandPanel2").GetComponentInChildren<GridLayoutGroup>();
    CardCounter = 0;
    CardCounter2 = 0;
    YCounter = 0;
    YCounter2 = 0;
}

// private void Update() {
//     // Arrange();
// }
// public void Arrange()
// {
//      //   GameObject go = GameObject.Find("CardsInHandPanel");
//         Debug.Log(name + " has " + transform.childCount + " children");
//         zValue = 1f;
//         for (int i = 0; i < transform.childCount; i++)
//         {
//             transform.GetChild(i).position = new Vector3(transform.GetChild(i).position.X, transform.GetChild(i).position.Y, zValue);
//             zValue++;
//         }
//         //SetSpacing(go.transform);
   
// }

// public void SetSpacing(Transform go) 
// {
//         switch (go.transform.childCount)
//         {
//             case 4:
//                 gridLayoutGroup.spacing = new Vector2(-0.50f, 0.0f);
//                 break;
//             case 5:
//                 gridLayoutGroup.spacing = new Vector2(-0.55f, 0.0f);
//                 break;
//             case 6:
//                 gridLayoutGroup.spacing = new Vector2(-0.6f, 0.0f);
//                 break;
//             case 7:
//                 gridLayoutGroup.spacing = new Vector2(-0.65f, 0.0f);
//                 break;
//             default:
//                 break;
//         }
// }

}
