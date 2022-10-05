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

    public static string knownAssetString;
    public static Dictionary<string, string> knownAssetMap = new Dictionary<string, string>();
    
    static Finalizer finalizer;

    static LoadGameAssets()
    {
        finalizer = new Finalizer();
        ReloadAssets();
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    [MenuItem("Shipbreaker/Reload Assets", priority = 4)]
    public static void ReloadAssets()
    {
        UnloadAssets();

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

        knownAssetString = System.IO.File.ReadAllText(System.IO.Path.Combine(Application.dataPath, "../known_assets.json"));
        knownAssetMap = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(knownAssetString);
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
        if (handle1.IsValid())
        {
            Addressables.RemoveResourceLocator(handle1.Result);
            Addressables.Release(handle1);
        }
        if (handle2.IsValid())
        {
            Addressables.RemoveResourceLocator(handle2.Result);
            Addressables.Release(handle2); 
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
