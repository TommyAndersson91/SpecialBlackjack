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
        Debug.Log("Loading OpenUIpanel");
        OpenUIPanelButton.onClick.AddListener(OpenUI);
        SubmitButton.onClick.AddListener(gameObject.GetComponent<ListItemController>().OnSubmit);
    }

    private void OpenUI()
    {
       Addressables.InstantiateAsync("uipanel").Completed += OnLoadDone;
    }
    
    public void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        obj.Result.GetComponentInChildren<ListItemController>().DeleteButton.onClick.AddListener(gameObject.GetComponent<ListItemController>().OnDeleted);
        // obj.Result.GetComponent<ListItemController>().PlayerName.text = gameObject.GetComponent<ListItemController>().NameInputField.text;
        obj.Result.transform.parent = gameObject.GetComponent<ListController>().ContentPanel.transform;
        obj.Result.transform.localScale = Vector3.one;
        obj.Result.GetComponentInChildren<ListItemController>().PlayerName.text = gameObject.GetComponent<ListItemController>().PlayerNameEntered;
        obj.Result.GetComponentInChildren<ListItemController>().PlayerAvatar.color = gameObject.GetComponent<ListController>().CalcColorAvatar(gameObject.GetComponent<ListItemController>().PlayerAvatarEntered);
        obj.Result.GetComponentInChildren<ListItemController>().PlayerTrinket.color = gameObject.GetComponent<ListController>().CalcColorTrinket(gameObject.GetComponent<ListItemController>().PlayerTrinketEntered);
    }
}
