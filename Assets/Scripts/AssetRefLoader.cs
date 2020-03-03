using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class AssetRefLoader : MonoBehaviour
{
    private AsyncOperationHandle<IList<GameObject>> player;
   public async Task CreateAssetAddToList<T>(AssetReference reference, List<T> completedObjs) 
   where T : Object
   {
     completedObjs.Add(await reference.InstantiateAsync().Task as T);
   }

   public async Task CreateAssetsAddToList<T>(List<AssetReference> references, List<T> completedObjs)
   where T : Object
   {
       player = Addressables.LoadAssetsAsync<GameObject>("labeltest", 
        (obj)=>{completedObjs.Add(Instantiate(obj) as T); });
    //    foreach (var reference in references)
    //    {
    //        player = reference.InstantiateAsync();
    //     // completedObjs.Add(await reference.InstantiateAsync().Task as T);
        
    //     // gameObject.GetComponent<AssetRefObjData>().loadingProgress.text = string.Format("Loading: {0}%", (int)(reference.InstantiateAsync().PercentComplete * 100));
    //     // Debug.Log(reference.InstantiateAsync().PercentComplete);
    //     }
   }

   private void Update() {
        gameObject.GetComponent<AssetRefObjData>().loadingProgress.text = string.Format("Loading: {0}%", (int)(player.PercentComplete * 100));
   }

   private void Start() {
       
   }
}
