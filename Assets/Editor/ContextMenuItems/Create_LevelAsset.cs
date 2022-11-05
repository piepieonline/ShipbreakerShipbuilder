using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BBI.Unity.Game;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public class Create_LevelAssets : EditorWindow
{
    static Create_LevelAssets window;

    [MenuItem("Assets/Create/Shipbreaker/Create level asset")]
    public static void CreateLevelAsset()
    {
        if(window)
        {
            window.Close();
        }
        window = EditorWindow.CreateInstance<Create_LevelAssets>();
        window.Show();
    }

    string levelName;
    Texture2D thumbnail;
    int moduleConstructionAssetIndex;
    string[] moduleConstructionOptions = new string[]
    {
        "Mackerel_Civilian_Cargo",
        "Mackerel_Civilian_Transit",
        "Mackerel_Industrial_Cargo",
        "Mackerel_Science_Shuttle",
        "Mistral_CARGOPrototype",
        "Mistral_PATROLPrototype",
        "Mistral_TUGPrototype",
        "Javelin_Industrial_Cargo_Lrg",
        "Javelin_Industrial_Cargo_Med",
        "Javelin_Industrial_Cargo_Sm",
        "Javelin_Industrial_Refueling_Lrg",
        "Javelin_Industrial_Refueling_Med",
        "Javelin_Industrial_Refueling_Sm",
        "Javelin_Science_Survey",
        "Gecko_Commercial_Transit",
        "Gecko_Industrial_Cargo",
        "Gecko_Industrial_Salvage",
        "Gecko_Science_Stargazer"
    };

    string[] moduleConstructionRefs = new string[]
    {
        "ffe15ab63fd88174786869d70ce973c3",
        "64a1a1f3e77574f468b15c16dfe0229f",
        "a488d3b761075ca44a3c5afaf63484db",
        "82a1edb7df47e36438f97962bc2219de",
        "aa2623567247318458c0714fadbb82e2",
        "6b7deaedb2090d8469184a78614eb64c",
        "f6794205b14ba4e4883f6ea8f339e5e3",
        "9900de72f695aa8458a72af8aca89a62",
        "118843c85e405524287bdd12fbaabb65",
        "e12fedd63b6c4b1468821b2ea98e2d95",
        "e552570cd6c5e7241b27591ebe4c7623",
        "d4ff9c7ba7d81304bb7dd4ef1d0ad8b1",
        "01189d84d812237458b862ff8fa84893",
        "21bd7beb03cdccb4bbfe90419f4f3223",
        "0c7331779b39c4741b64b932c6db8e6a",
        "76be97f4fa8f60b429ee1c8d98e47b85",
        "5e42e2c3f2323df48b5c6dda27db1824",
        "94e105acd4675b9458d5d3202bf71291"
    };

    void OnGUI()
    {
        GUILayout.Label("Create custom level", EditorStyles.boldLabel);

        levelName = AssetDatabase.GetAssetPath(Selection.activeInstanceID).Split('/').Last();

        levelName = EditorGUILayout.TextField("Display name", levelName);
        thumbnail = (Texture2D)EditorGUILayout.ObjectField("Thumbnail", thumbnail, typeof(Texture2D), false);
        moduleConstructionAssetIndex = EditorGUILayout.Popup("Module Construction Asset", moduleConstructionAssetIndex, moduleConstructionOptions);

        if (GUILayout.Button($"Create {levelName}") &&
            !string.IsNullOrWhiteSpace(levelName) &&
            thumbnail != null
        )
        {
            CreateAsset();
            window.Close();
        }
    }

    void CreateAsset()
    {
        string assetName = Selection.activeObject.name;
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        string folderPath = assetPath.Remove(assetPath.LastIndexOf('/')) + "/" + levelName;
        string spawnFolderName = "Spawn";
        string spawnerFolderPath = folderPath + "/" + spawnFolderName;

        string thumbnailGuid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(thumbnail)).ToString();

        AssetDatabase.CreateFolder(folderPath, spawnFolderName);

        // Create the ship prefab
        GameObject prefab = new GameObject($"{levelName}", new System.Type[] { typeof(ModuleDefinition) });
        PrefabUtility.SaveAsPrefabAsset(prefab, GetPathForAsset("", folderPath, "prefab"));
        DestroyImmediate(prefab);
        string prefabGuid = AssetDatabase.GUIDFromAssetPath(GetPathForAsset("", folderPath, "prefab")).ToString();

        // Create the hardpoint (Root prefab > Prefab hardpoint)
        string prefabHardpointGuid = Create_Hardpoint.CreateHardpointAsset(spawnerFolderPath, spawnFolderName, 
            new Create_Hardpoint.HardpointAssetReference[] { new Create_Hardpoint.HardpointAssetReference() { assetRef = prefabGuid, weight = 1 } },
        -1);

        // Create the root prefab ref (Construction Asset > Root prefab)
        GameObject rootPrefab = new GameObject($"{levelName}_RootRef", new System.Type[] { typeof(RootModuleDefinition) });
        var rootHardpoint = new GameObject($"{levelName}_Hardpoint", new System.Type[] { typeof(HardPoint) });
        rootHardpoint.transform.parent = rootPrefab.transform;
        
        typeof(HardPoint).GetField("m_AssetRef", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(rootHardpoint.GetComponent<HardPoint>(), new AssetReferenceModuleListAsset(prefabHardpointGuid));
        var newPrefab = PrefabUtility.SaveAsPrefabAsset(rootPrefab, GetPathForAsset("RootRef", spawnerFolderPath, "prefab"));
        DestroyImmediate(rootPrefab);
        string rootPrefabGuid = AssetDatabase.GUIDFromAssetPath(GetPathForAsset("RootRef", spawnerFolderPath, "prefab")).ToString();

        // Create the ModuleConstructionAsset, based on the given type (Level Asset > Construction Asset)
        var moduleConstructionAsset = CreateInstance<ModuleConstructionAsset>();
        typeof(ModuleConstructionAsset).GetField("AssetBasis").SetValue(moduleConstructionAsset, moduleConstructionRefs[moduleConstructionAssetIndex]);
        typeof(ModuleConstructionAsset.ModuleConstructionData).GetField("m_RootModuleRef", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(moduleConstructionAsset.Data, new AssetReferenceGameObject(rootPrefabGuid));

        AssetDatabase.CreateAsset(moduleConstructionAsset, GetPathForAsset("ModuleConstructionAsset", folderPath));
        var moduleConstructionAssetGuid = AssetDatabase.GUIDFromAssetPath(GetPathForAsset("ModuleConstructionAsset", folderPath)).ToString();

        var levelAsset = CreateInstance<LevelAsset>();

        levelAsset.Data.LevelDisplayName = levelName;
        levelAsset.Data.LevelDescriptionShort = Settings.GetAuthorName();
        levelAsset.Data.LevelDescriptionFull = $"CUSTOM:{rootPrefabGuid}";
        levelAsset.Data.LevelThumbnailImageRef = new AssetReferenceTexture(thumbnailGuid);
        levelAsset.Data.BaseSceneRef = new AssetReferenceScene("be5ef977dfbe2b344b39a1cd19df1fb1");
        levelAsset.Data.StartingShipRef = new AssetReferenceModuleConstructionAsset(moduleConstructionAssetGuid);

        levelAsset.Data.SessionType = GameSession.SessionType.FreeMode;
        levelAsset.Data.SortOrder = 100;
        levelAsset.Data.CompletionTimeInSeconds = 9999;
        levelAsset.Data.TimerCountsUp = true;
        levelAsset.Data.IsUnlocked = true;

        AssetDatabase.CreateAsset(levelAsset, GetPathForAsset("LevelAsset", folderPath));
        var levelAssetGuid = AssetDatabase.GUIDFromAssetPath(GetPathForAsset("LevelAsset", folderPath)).ToString();

        // Addressables
        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        var entriesAdded = new List<AddressableAssetEntry>();

        entriesAdded.Add(addressableSettings.CreateOrMoveEntry(prefabGuid, addressableSettings.DefaultGroup));
        entriesAdded.Add(addressableSettings.CreateOrMoveEntry(prefabHardpointGuid, addressableSettings.DefaultGroup));
        entriesAdded.Add(addressableSettings.CreateOrMoveEntry(rootPrefabGuid, addressableSettings.DefaultGroup));

        var levelAssetEntry = addressableSettings.CreateOrMoveEntry(levelAssetGuid, addressableSettings.DefaultGroup);
        levelAssetEntry.labels.Add("GameMode_Free");
        levelAssetEntry.labels.Add("Release");
        entriesAdded.Add(levelAssetEntry);

        entriesAdded.Add(addressableSettings.CreateOrMoveEntry(thumbnailGuid, addressableSettings.DefaultGroup));

        var moduleConstructionAssetEntry = addressableSettings.CreateOrMoveEntry(moduleConstructionAssetGuid, addressableSettings.DefaultGroup);
        moduleConstructionAssetEntry.labels.Add("ModdedLevel");
        entriesAdded.Add(moduleConstructionAssetEntry);

        addressableSettings.DefaultGroup.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
        addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);

        AssetDatabase.SaveAssets();
        Selection.activeObject = levelAsset;

        EditorUtility.FocusProjectWindow();
    }

    string GetPathForAsset(string asset, string folderPath, string ext = "asset")
    {
        return $"{folderPath}/{levelName}{(asset != "" ? "_" : "")}{asset}.{ext}";
    }
}