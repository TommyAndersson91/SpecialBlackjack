using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class ListItemController : MonoBehaviour
{
    public delegate void NameChanged();
    public static NameChanged onNameChanged;
    [SerializeField] public Button DeleteButton;
    public Image PlayerAvatar;
    public Image PlayerTrinket;
    public TextMeshProUGUI PlayerName;
    private static string playerNameString;
    public static string PlayerNameString
    {
        get { return playerNameString; }
        set { onNameChanged?.Invoke(); 
            playerNameString = value; }
    }

    public void OnNameChanged()
    {
        Debug.Log("On Name Changed called");
        PlayerName.text = UIPanel.PlayerNameEntered; 
    }
    

    private void Start() {
    DeleteButton.onClick.AddListener(OnDeleted);
    onNameChanged += OnNameChanged;
    }

    public void OnDeleted()
    {
        
    Destroy(gameObject);
    }

}

