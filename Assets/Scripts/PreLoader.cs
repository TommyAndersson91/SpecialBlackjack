// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
// using UnityEngine.ResourceManagement.AsyncOperations;
// using TMPro;

// public class PreLoader : MonoBehaviour
// {
//     public PlayerSO playerSO;
//     private AsyncOperationHandle<GameObject> player;
//     public TextMeshProUGUI loadingProgress;

//     void Start()
//     {
//         loadingProgress.text = string.Format("Loading: {0}%", 0);
//         if (!playerSO.mpc.player)
//         {
//             player = Addressables.LoadAssetAsync<GameObject>("Base");
//             player.Completed += OnLoadDone;
//         }
//         else
//         {
//             Addressables.LoadSceneAsync("Part 06 Addressable");
//         }
//     }

//     private void OnLoadDone(AsyncOperationHandle<GameObject> obj)
//     {

//         if (obj.Status == AsyncOperationStatus.Failed)
//         {
//             Debug.Log("Failed to load hazards, retrying in 1 second...");
//             Invoke("LoadHazards", 1);
//             return;
//         }
//         playerSO.mpc.player = obj.Result;
//     }

//     void Update()
//     {
//         if (player.IsValid())
//         {
//             loadingProgress.text = string.Format("Loading: {0}%", (int)(player.PercentComplete * 100));
//             if (playerSO.mpc.player)
//             {
//                 Addressables.ReleaseInstance(player);
//                 Addressables.LoadSceneAsync("Part 06 Addressable");
//             }
//         }
//     }
// }

