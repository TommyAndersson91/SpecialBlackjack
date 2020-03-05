using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ListController : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

    public Color CalcColorAvatar(string color)
    {
        switch (color)
        {
            case "black":
                return Color.black;
            case "blue":
                return Color.blue;
            case "clear":
                return Color.clear;
            case "cyan":
                return Color.cyan;
            case "gray":
                return Color.gray;
            case "green":
                return Color.green;
            case "magenta":
                return Color.magenta;
            case "red":
                return Color.red;
            case "white":
                return Color.white;
            case "yellow":
                return Color.yellow;
            default:
                return Color.clear;
        }
    }

    public Color CalcColorTrinket(string color)
    {
        switch (color)
        {
            case "black":
                return Color.black;
            case "blue":
                return Color.blue;
            case "clear":
                return Color.clear;
            case "cyan":
                return Color.cyan;
            case "gray":
                return Color.gray;
            case "green":
                return Color.green;
            case "magenta":
                return Color.magenta;
            case "red":
                return Color.red;
            case "white":
                return Color.white;
            case "yellow":
                return Color.yellow;
            default:
                return Color.clear;
        }
    }
}
