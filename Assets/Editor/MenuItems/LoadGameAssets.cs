using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[InitializeOnLoad]
public class LoadGameAssets
{
    public static AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> customAssetResourceHandle;
    public static AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> gameAssetResourceHandle;

    public static string knownAssetString;
    public static Dictionary<string, string> knownAssetMap = new Dictionary<string, string>();
    
    static Finalizer finalizer;

    static bool customAssetsLoaded = false;

    static LoadGameAssets()
    {
        finalizer = new Finalizer();
        ReloadAssets();
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    [MenuItem("Shipbreaker/Reload Assets", priority = 4)]
    public static void ReloadAssets()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        UnloadAssets();

        if(File.Exists(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "com.unity.addressables", "aa", "Windows", "catalog.json"))))
        {
            customAssetsLoaded = true;
            customAssetResourceHandle = Addressables.LoadContentCatalogAsync(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "com.unity.addressables", "aa", "Windows", "catalog.json")), false);
            customAssetResourceHandle.Completed += status => { Debug.Log($"Loading custom assets complete. Valid: {status.IsValid()}"); };
        }
        else
        {
            Debug.LogWarning("No custom assets loaded - assuming none have been built yet");
        }

        if(File.Exists(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "modded_catalog.json"))))
        {
            gameAssetResourceHandle = Addressables.LoadContentCatalogAsync(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "modded_catalog.json")), false);
            gameAssetResourceHandle.Completed += status => { Debug.Log($"Loading game assets complete. Valid: {status.IsValid()}"); };
        }
        else
        {
            Debug.LogError("No game assets loaded - Make sure you have built the catalog!");
        }

        knownAssetString = File.ReadAllText(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "known_assets.json")));
        knownAssetMap = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(knownAssetString);
        Debug.Log($"Known Asset list is {System.Text.Encoding.Unicode.GetByteCount(knownAssetString)} bytes in {knownAssetMap.Count} records");    
    }

    public static bool CheckHandlesValid()
    {
        if(customAssetsLoaded)
        {
            return customAssetResourceHandle.IsValid() && gameAssetResourceHandle.IsValid();
        }
        else
        {
            return gameAssetResourceHandle.IsValid();
        }
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

    static void UnloadAssets()
    {
        customAssetsLoaded = false;

        if (customAssetResourceHandle.IsValid())
        {
            Addressables.RemoveResourceLocator(customAssetResourceHandle.Result);
            Addressables.Release(customAssetResourceHandle);
        }
        if (gameAssetResourceHandle.IsValid())
        {
            Addressables.RemoveResourceLocator(gameAssetResourceHandle.Result);
            Addressables.Release(gameAssetResourceHandle); 
        }

        Addressables.ClearResourceLocators(); 
    }

    sealed class Finalizer {
        ~Finalizer()
        {
            Debug.Log("Unloading");
            LoadGameAssets.UnloadAssets();
        }
    }
}
