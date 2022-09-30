#if UNITY_EDITOR

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor;

using BBI.Unity.Game;

public class GameInspectorWindow : EditorWindow
{
    public string searchTerm = "fd038d23f35b59747a22dec2f214b11f";
    Vector2 currentScrollPosition;
    string lastAddress;
    List<string> searchResultGUIDs = new List<string>();
    string selectedGuid;
    string foundGuid;
    bool inspectOnClick = true;

    [MenuItem("Shipbreaker/Show Game Inspector", priority = 101)]
    public static void CreateRoomAsset()
    {
        EditorWindow.CreateInstance<GameInspectorWindow>().Show();
    }

    void OnGUI()
    {
        inspectOnClick = EditorGUILayout.Toggle("Inspect asset on click", inspectOnClick);

        GUILayout.Label("Asset Search", EditorStyles.label);
        searchTerm = GUILayout.TextField(searchTerm).Trim();

        int max = 100;

        if(lastAddress != searchTerm)
        {
            selectedGuid = "";
            searchResultGUIDs.Clear();
            var matches = Regex.Matches(LoadGameAssets.knownAssetString, $"\r\n(.*{searchTerm}.*)\r\n", RegexOptions.IgnoreCase);
            if(matches.Count > 0)
            {
                for(int i = 0; i < Mathf.Min(matches.Count, max); i++)
                {
                    searchResultGUIDs.Add(matches[i].Value.Substring(3, 32));
                }
            }

            lastAddress = searchTerm;
        }

        GUILayout.Label("Selected GUID", EditorStyles.label);
        foundGuid = selectedGuid != "" ? selectedGuid : searchResultGUIDs.Count == 1 ? searchResultGUIDs[0] : "";
        GUILayout.TextField(foundGuid);

        currentScrollPosition = EditorGUILayout.BeginScrollView(currentScrollPosition);
        int count = 0;

        foreach(var assetGUID in searchResultGUIDs)
        {
            if(GUILayout.Button($"{LoadGameAssets.knownAssetMap[assetGUID]} ({assetGUID})", EditorStyles.label))
            {
                selectedGuid = assetGUID;

                if(inspectOnClick)
                {
                    Addressables.LoadAssetAsync<UnityEngine.Object>(new AssetReference(selectedGuid)).Completed += res =>
                    {
                        Selection.activeObject = res.Result;
                        Repaint();
                    };
                }
            }
            count++;
            if(count > max) break;
        }
        EditorGUILayout.EndScrollView();

        if (foundGuid != "" && LoadGameAssets.knownAssetMap[foundGuid].EndsWith(".prefab") && GUILayout.Button("Open prefab in preview scene (readonly)") && !string.IsNullOrWhiteSpace(searchTerm))
        {
            Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(searchTerm)).Completed += res =>
            {
                CustomStage.go = res.Result;
                UnityEditor.SceneManagement.StageUtility.GoToStage(new CustomStage(), true);
            };
        }
    }

    public static AsyncOperationHandle<GameObject> GameObjectInstantiateReady(AsyncOperationHandle<GameObject> arg)
    {
        Debug.Log($"doin' work {System.DateTime.Now}");

        if (arg.Result.TryGetComponent<BBI.Unity.Game.AddressableLoader>(out var loader))
        {
            for (int i = 0; i < loader.refs.Count; i++)
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