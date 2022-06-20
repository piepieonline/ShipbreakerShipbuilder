using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[InitializeOnLoad]
public class LoadGameAssets
{
    static LoadGameAssets()
    {
        ReloadAssets();
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    [MenuItem("Shipbreaker/Reload Assets", priority = 4)]
    public static void ReloadAssets()
    {
        if (LoadAddressables.handle1.IsValid()) Addressables.Release(LoadAddressables.handle1);
        if (LoadAddressables.handle2.IsValid()) Addressables.Release(LoadAddressables.handle2);

        LoadAddressables.handle1 = Addressables.LoadContentCatalogAsync(Application.dataPath + "\\..\\Library\\com.unity.addressables\\aa\\Windows\\catalog.json", false);
        LoadAddressables.handle2 = Addressables.LoadContentCatalogAsync(Application.dataPath + "\\..\\modded_catalog.json", false);

        LoadAddressables.handle1.Completed += status => { Debug.Log($"Loading custom assets complete. Valid: {status.IsValid()}"); };
        LoadAddressables.handle2.Completed += status => { Debug.Log($"Loading game assets complete. Valid: {status.IsValid()}"); };
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

        DrawEditor.UpdateViewList();
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
