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
public static Vector3 NewPos;
public static float y {get; set;}
public static float x {get; set;}
public static float y2 { get; set; }
public static float x2 { get; set; }
public static int CardCounter = 0;
public static int CardCounter2 = 0;
public static int YCounter = 0;
public static int YCounter2 = 0;


// public static Vector3 GetPos(int playerIndex)
// {
//     // gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount-1).Rotate(360f, 0f, 0f);   
//     if (playerIndex == 0)
//     {   
//         return NewPos = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount-1).position;
//     }
//     else
//     {
//         return NewPos = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position;
//     }
// }

public static float GetX(int playerIndex)
{
    gridLayoutGroup.enabled = false;
    gridLayoutGroup2.enabled = false;
    if (playerIndex == 0)
    {
        CardCounter++;
        if (PlayerList.GetPlayers()[0].DrawnCards.Count < 4)
        {
           
            return x = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.x+gridLayoutGroup.cellSize.x*CardCounter;
        }
        else
        {
            x = gridLayoutGroup.transform.GetChild(YCounter).position.x;
            YCounter++;
            return x;
        }
    }
    else
    {
        CardCounter2++;
        if (PlayerList.GetPlayers()[1].DrawnCards.Count < 4)
        {
            return x2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.x+gridLayoutGroup2.cellSize.x*CardCounter2;
        }
        else
        {
            x2 = gridLayoutGroup2.transform.GetChild(YCounter2).position.x;
            YCounter2++;
            return x2;
        }
    }
}

public static float GetY(int playerIndex)
{
        if (playerIndex == 0)
        {
            if (PlayerList.GetPlayers()[0].DrawnCards.Count < 4)
            {
                return y = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.y;
            }
            else
            {
                return y = gridLayoutGroup.transform.GetChild(gridLayoutGroup.transform.childCount - 1).position.y-gridLayoutGroup.cellSize.y;
            }
        }
        else
        {
            if (PlayerList.GetPlayers()[1].DrawnCards.Count < 4)
            {
                return y2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.y;
            }
            else
            {
                return y2 = gridLayoutGroup2.transform.GetChild(gridLayoutGroup2.transform.childCount - 1).position.y-gridLayoutGroup2.cellSize.y;
            }

        }
}

private void Start() {

        // Arrange();
        gridLayoutGroup = GameObject.Find("CardsInHandPanel").GetComponentInChildren<GridLayoutGroup>();
        gridLayoutGroup2 = GameObject.Find("CardsInHandPanel2").GetComponentInChildren<GridLayoutGroup>();

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
//             transform.GetChild(i).position = new Vector3(transform.GetChild(i).position.x, transform.GetChild(i).position.y, zValue);
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
