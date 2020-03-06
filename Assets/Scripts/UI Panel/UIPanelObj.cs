using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class UIPanelObj : MonoBehaviour
{
 
    [SerializeField] public Button DeleteButton;
    [SerializeField] private Image PlayerAvatar;
    [SerializeField] private Image PlayerTrinket;
    [SerializeField] private TextMeshProUGUI PlayerName;

    private void OnNameChanged(string text)
    {
        Debug.Log("On Name Changed called");
        PlayerName.text = text; 
    }

    private void OnAvatarChanged (string color)
    {
        Debug.Log("On Avatar Changed called");
        PlayerAvatar.color = ListController.CalcColor(color);
    }

    private void OnTrinketChanged(string color)
    {
        Debug.Log("On Trinket Changed called");
        PlayerTrinket.color = ListController.CalcColor(color);
    }
    
    private void Start() {
    PlayerName.text = UIPanelData.PlayerNameEntered;
    PlayerAvatar.color = ListController.CalcColor(UIPanelData.PlayerAvatarEntered);
    PlayerTrinket.color = ListController.CalcColor(UIPanelData.PlayerTrinketEntered);
    UIPanelData.onNameChanged += OnNameChanged;
    UIPanelData.onAvatarChanged += OnAvatarChanged;
    UIPanelData.onTrinketChanged += OnTrinketChanged;
    DeleteButton.onClick.AddListener(OnDeleted);
    }

    private void OnDestroy() {
        UIPanelData.onNameChanged -= OnNameChanged;
        UIPanelData.onAvatarChanged -= OnAvatarChanged;
        UIPanelData.onTrinketChanged -= OnTrinketChanged;
    }

    private void OnDeleted()
    {
    Destroy(gameObject);
    }

}

