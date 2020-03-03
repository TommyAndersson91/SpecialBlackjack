using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class OpenUIPanel : MonoBehaviour
{

    private void Start() {
        Addressables.LoadAssetAsync<GameObject>("uipanel");
    }

    private void OpenUI()
    {
        Addressables.InstantiateAsync("uipanel").Completed += OnLoadDone;
    }
    
    public void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
       
        gameObject.GetComponent<CardController>().card = obj.Result;  
    }


}
