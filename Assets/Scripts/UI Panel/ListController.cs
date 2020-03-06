using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ListController : MonoBehaviour
{
    public GameObject ContentPanel;
    [SerializeField] private TMP_InputField NameInputField;
    [SerializeField] private TMP_InputField AvatarInputField;
    [SerializeField] private TMP_InputField TrinketInputField;
    [SerializeField] private Button SubmitButton;

    private void Start() {
        SubmitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        UIPanelData.PlayerNameEntered = NameInputField.text;
        UIPanelData.PlayerAvatarEntered = AvatarInputField.text;
        UIPanelData.PlayerTrinketEntered = TrinketInputField.text;
    }

    public static Color CalcColor(string color)
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
