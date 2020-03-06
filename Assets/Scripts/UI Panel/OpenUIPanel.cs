using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using TMPro;
public class OpenUIPanel : MonoBehaviour
{
[SerializeField] private Button OpenUIPanelButton;
[SerializeField] private Button SubmitButton;
    private void Start() {
        Addressables.LoadAssetAsync<GameObject>("uipanel");
        OpenUIPanelButton.onClick.AddListener(OpenUI);
        SubmitButton.onClick.AddListener(gameObject.GetComponent<ListController>().OnSubmit);
    }

    private void OpenUI()
    {
       Addressables.InstantiateAsync("uipanel").Completed += OnLoadDone;
    }
    
    public void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        obj.Result.transform.parent = gameObject.GetComponent<ListController>().ContentPanel.transform;
        obj.Result.transform.localScale = Vector3.one;
        obj.Result.GetComponentInChildren<ListItemController>().PlayerName.text = UIPanel.PlayerNameEntered;
        obj.Result.GetComponentInChildren<ListItemController>().PlayerAvatar.color = gameObject.GetComponent<ListController>().CalcColorAvatar(UIPanel.PlayerAvatarEntered);
        obj.Result.GetComponentInChildren<ListItemController>().PlayerTrinket.color = gameObject.GetComponent<ListController>().CalcColorTrinket(UIPanel.PlayerTrinketEntered);
    }
}
