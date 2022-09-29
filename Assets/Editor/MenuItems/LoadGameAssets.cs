using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[InitializeOnLoad]
public class LoadGameAssets
{
    public static AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> handle1;
    public static AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> handle2;

    public static Dictionary<string, string> knownVanillaGuids = new Dictionary<string, string>();
    
    static LoadGameAssets()
    {
        ReloadAssets();
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    [MenuItem("Shipbreaker/Reload Assets", priority = 4)]
    public static void ReloadAssets()
    {
        if (handle1.IsValid()) Addressables.Release(handle1);
        if (handle2.IsValid()) Addressables.Release(handle2);

        if(System.IO.File.Exists(Application.dataPath + "\\..\\modded_catalog.json"))
        {
            handle1 = Addressables.LoadContentCatalogAsync(Application.dataPath + "\\..\\modded_catalog.json", false);
            handle1.Completed += status => { Debug.Log($"Loading custom assets complete. Valid: {status.IsValid()}"); };
        }
        else
        {
            Debug.LogWarning("No custom assets loaded - assuming none have been built yet");
        }

        if(System.IO.File.Exists(Application.dataPath + "\\..\\modded_catalog.json"))
        {
            handle2 = Addressables.LoadContentCatalogAsync(Application.dataPath + "\\..\\modded_catalog.json", false);
            handle2.Completed += status => { Debug.Log($"Loading game assets complete. Valid: {status.IsValid()}"); };
        }
        else
        {
            Debug.LogError("No game assets loaded - Make sure you have built the catalog!");
        }

        knownVanillaGuids = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(System.IO.Path.Combine(Application.dataPath, "../known_assets.json")));
    }

    [MenuItem("Shipbreaker/Clear Asset Cache", priority = 20)]
    static void ClearAssetCache()
    {
        AssetDatabase.DeleteAssets(new string[] { "Assets/EditorCache" }, new List<string>());
        AssetDatabase.CreateFolder("Assets", "EditorCache");
        AssetDatabase.Refresh();
    }

    [MenuItem("Shipbreaker/Force View Refresh", priority = 1)]
    static void ViewRefresh()
    {
        lastNumRoot = UnityEngine.SceneManagement.SceneManager.GetActiveScene().rootCount;
        AddressableRendering.UpdateViewList();
    }

    static int lastNumRoot;
    static void OnHierarchyChanged()
    {
        if(lastNumRoot != UnityEngine.SceneManagement.SceneManager.GetActiveScene().rootCount)
        {
            ViewRefresh();
        }
    }
}
