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
        PlayerName.text = text; 
    }

    private void OnAvatarChanged (string color)
    {
        PlayerAvatar.color = UIController.CalcColor(color);
    }

    private void OnTrinketChanged(string color)
    {
        PlayerTrinket.color = UIController.CalcColor(color);
    }
    
    private void Start() 
    {
        PlayerName.text = UIPanelData.PlayerNameEntered;
        PlayerAvatar.color = UIController.CalcColor(UIPanelData.PlayerAvatarEntered);
        PlayerTrinket.color = UIController.CalcColor(UIPanelData.PlayerTrinketEntered);
        UIPanelData.onNameChanged += OnNameChanged;
        UIPanelData.onAvatarChanged += OnAvatarChanged;
        UIPanelData.onTrinketChanged += OnTrinketChanged;
        DeleteButton.onClick.AddListener(OnDeleted);
    }

    private void OnDestroy() 
    {
        UIPanelData.onNameChanged -= OnNameChanged;
        UIPanelData.onAvatarChanged -= OnAvatarChanged;
        UIPanelData.onTrinketChanged -= OnTrinketChanged;
    }

    private void OnDeleted()
    {
    Destroy(gameObject);
    }
}

