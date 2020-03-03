using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro; 

public class AssetRefObjData : MonoBehaviour
{
    [SerializeField] private AssetReference _sqrARef; 
    [SerializeField] private List<AssetReference> _references = new List<AssetReference>();
    [SerializeField] private List<GameObject> _completedObj = new List<GameObject>();
    // Utkommenterat under tiden jag jobbar med UI paneler 
    //   public TextMeshProUGUI loadingProgress;
    private void Start() {
        // _references.Add(_sqrARef);
        // Utkommenterat under tiden jag jobbar med UI paneler 
        // StartCoroutine(LoadAndWaitUntilComplete());
        Debug.Log("Running Start in AssetRefObjData");
    }

    private void Awake() {
        gameObject.AddComponent<AssetRefLoader>();
    }

    private IEnumerator LoadAndWaitUntilComplete()
    {
        yield return gameObject.GetComponent<AssetRefLoader>().CreateAssetsAddToList(_references, _completedObj);
    }
}
