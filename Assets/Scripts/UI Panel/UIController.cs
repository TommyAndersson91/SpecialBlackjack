using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject ContentPanel;
    [SerializeField] private TMP_InputField NameInputField;
    [SerializeField] private TMP_InputField AvatarInputField;
    [SerializeField] private TMP_InputField TrinketInputField;
    [SerializeField] private Button SubmitButton;
    [SerializeField] private Button OpenUIPanelButton;

    private void Start() 
    {
        SubmitButton.onClick.AddListener(OnSubmit);
        Addressables.LoadAssetAsync<GameObject>("uipanel");
        OpenUIPanelButton.onClick.AddListener(OpenUI);
    }

    private void OpenUI()
    {
        Addressables.InstantiateAsync("uipanel").Completed += OnInstantiateDone;
    }

    private void OnSubmit()
    {
        UIPanelData.PlayerNameEntered = NameInputField.text;
        UIPanelData.PlayerAvatarEntered = AvatarInputField.text;
        UIPanelData.PlayerTrinketEntered = TrinketInputField.text;
    }

    private void OnInstantiateDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        obj.Result.transform.parent = ContentPanel.transform;
        obj.Result.transform.localScale = Vector3.one;
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
