using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class ListItemController : MonoBehaviour
{
    [SerializeField] public TMP_InputField NameInputField;
    [SerializeField] public TMP_InputField AvatarInputField;
    [SerializeField] public TMP_InputField TrinketInputField;
    [SerializeField] public Button DeleteButton;
    public string PlayerNameEntered;
    public string PlayerAvatarEntered;
    public string PlayerTrinketEntered;
    public Image PlayerAvatar;
    public Image PlayerTrinket;
    public TextMeshProUGUI PlayerName;

    private void Start() {
    DeleteButton.onClick.AddListener(OnDeleted);
    }

    public void OnDeleted()
    {
    Destroy(gameObject);
    }

    public void OnSubmit()
    {
        PlayerNameEntered = NameInputField.text;
        PlayerAvatarEntered = AvatarInputField.text;
        PlayerTrinketEntered = TrinketInputField.text;
        
        for (int i = 0; i < gameObject.GetComponent<ListController>().ContentPanel.transform.childCount; i++)
        {
        gameObject.GetComponent<ListController>().ContentPanel.transform.GetChild(i).GetComponentInChildren<ListItemController>().PlayerName.text = NameInputField.text;
        gameObject.GetComponent<ListController>().ContentPanel.transform.GetChild(i).GetComponentInChildren<ListItemController>().PlayerAvatar.color = gameObject.GetComponent<ListController>().CalcColorAvatar(PlayerAvatarEntered);
        gameObject.GetComponent<ListController>().ContentPanel.transform.GetChild(i).GetComponentInChildren<ListItemController>().PlayerTrinket.color = gameObject.GetComponent<ListController>().CalcColorTrinket(PlayerTrinketEntered);
        }
    }
}

