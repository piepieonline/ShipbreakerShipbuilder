#if UNITY_EDITOR

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor;

using BBI.Unity.Game;

public class GameInspectorWindow : EditorWindow
{
    public string address = "fd038d23f35b59747a22dec2f214b11f";

    [MenuItem("Shipbreaker/Show Game Inspector", priority = 101)]
    public static void CreateRoomAsset()
    {
        EditorWindow.CreateInstance<GameInspectorWindow>().Show();
    }

    void OnGUI()
    {
        {
            // GUILayout.Label("Create room asset", EditorStyles.boldLabel);
            GUILayout.Label("Asset GUID", EditorStyles.label);
            address = GUILayout.TextField(address).Trim();

            if (GUILayout.Button("Load GameObject") && !string.IsNullOrWhiteSpace(address))
            {
                Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(address)).Completed += res =>
                {
                    CustomStage.go = res.Result;
                    UnityEditor.SceneManagement.StageUtility.GoToStage(new CustomStage(), true);
                };
            }

            if (GUILayout.Button("Inspect Asset") && !string.IsNullOrWhiteSpace(address))
            {
                Addressables.LoadAssetAsync<UnityEngine.Object>(new AssetReference(address)).Completed += res =>
                {
                    Selection.activeObject = res.Result;
                };
            }
        }
    }

    public static AsyncOperationHandle<GameObject> GameObjectInstantiateReady(AsyncOperationHandle<GameObject> arg)
    {
        Debug.Log($"doin' work {System.DateTime.Now}");

        if(arg.Result.TryGetComponent<BBI.Unity.Game.AddressableLoader>(out var loader))
        {
            for(int i = 0; i < loader.refs.Count; i++)
            {
                Addressables.ResourceManager.CreateChainOperation<GameObject, GameObject>(
                    Addressables.InstantiateAsync(new AssetReferenceGameObject(loader.refs[i]), arg.Result.transform.GetChild(i)), GameObjectInstantiateReady)
                ;
            }
        }

        arg.Result.hideFlags = HideFlags.DontSaveInEditor;

        Debug.Log($"done work {System.DateTime.Now}");
        
        return Addressables.ResourceManager.CreateCompletedOperation(arg.Result, string.Empty);
    }
}

#endif