using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using TMPro;
public class OpenNewUIPanelObj : MonoBehaviour
{
[SerializeField] private Button OpenUIPanelButton;
    private void Start() {
        Addressables.LoadAssetAsync<GameObject>("uipanel");
        OpenUIPanelButton.onClick.AddListener(OpenUI);
    }

    private void OpenUI()
    {
       Addressables.InstantiateAsync("uipanel").Completed += OnLoadDone;
    }
    
    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        obj.Result.transform.parent = gameObject.GetComponent<ListController>().ContentPanel.transform;
        obj.Result.transform.localScale = Vector3.one;
    }
}
