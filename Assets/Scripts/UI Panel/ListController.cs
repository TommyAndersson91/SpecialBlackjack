using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ListController : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    [SerializeField] public TMP_InputField NameInputField;
    [SerializeField] public TMP_InputField AvatarInputField;
    [SerializeField] public TMP_InputField TrinketInputField;


    public void OnSubmit()
    {
        
        UIPanel.PlayerNameEntered = NameInputField.text;
        UIPanel.PlayerAvatarEntered = AvatarInputField.text;
        UIPanel.PlayerTrinketEntered = TrinketInputField.text;
        ListItemController.PlayerNameString = UIPanel.PlayerNameEntered;

        // for (int i = 0; i < gameObject.GetComponent<ListController>().ContentPanel.transform.childCount; i++)
        // {
        // gameObject.GetComponent<ListController>().ContentPanel.transform.GetChild(i).GetComponentInChildren<ListItemController>().PlayerName.text = NameInputField.text;
        // gameObject.GetComponent<ListController>().ContentPanel.transform.GetChild(i).GetComponentInChildren<ListItemController>().PlayerAvatar.color = gameObject.GetComponent<ListController>().CalcColorAvatar(PlayerAvatarEntered);
        // gameObject.GetComponent<ListController>().ContentPanel.transform.GetChild(i).GetComponentInChildren<ListItemController>().PlayerTrinket.color = gameObject.GetComponent<ListController>().CalcColorTrinket(PlayerTrinketEntered);
        // }
    }

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
