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
        GUILayout.Label("Create level asset", EditorStyles.boldLabel);

        levelName = Selection.activeObject.name ?? "";

        levelName = EditorGUILayout.TextField("Display name", levelName);
        thumbnail = (Texture2D)EditorGUILayout.ObjectField("Thumbnail", thumbnail, typeof(Texture2D), false);
        moduleConstructionAssetIndex = EditorGUILayout.Popup("Module Construction Asset", moduleConstructionAssetIndex, moduleConstructionOptions);

        if (GUILayout.Button($"Create {levelName} LevelAsset") &&
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
        string folderPath = assetPath.Remove(assetPath.LastIndexOf('/')); ;

        string levelPrefabGuid = AssetDatabase.GUIDFromAssetPath(assetPath).ToString();
        string thumbnailGuid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(thumbnail)).ToString();

        // Selection.activeObject
        var levelAsset = CreateInstance<LevelAsset>();

        levelAsset.Data.LevelDisplayName = levelName;
        levelAsset.Data.LevelDescriptionShort = Settings.GetAuthorName();
        levelAsset.Data.LevelDescriptionFull = $"CUSTOM:{levelPrefabGuid}";
        levelAsset.Data.LevelThumbnailImageRef = new AssetReferenceTexture(thumbnailGuid);
        levelAsset.Data.BaseSceneRef = new AssetReferenceScene("be5ef977dfbe2b344b39a1cd19df1fb1");
        levelAsset.Data.StartingShipRef = new AssetReferenceModuleConstructionAsset(moduleConstructionRefs[moduleConstructionAssetIndex]);

        levelAsset.Data.SessionType = GameSession.SessionType.FreeMode;
        levelAsset.Data.SortOrder = 100;
        levelAsset.Data.CompletionTimeInSeconds = 9999;
        levelAsset.Data.TimerCountsUp = true;
        levelAsset.Data.IsUnlocked = true;

        AssetDatabase.CreateAsset(levelAsset, $"{folderPath}/LevelAsset_{levelName}.asset");
        var levelAssetGuid = AssetDatabase.GUIDFromAssetPath($"{folderPath}/LevelAsset_{levelName}.asset").ToString();

        // Addressables
        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        var entriesAdded = new List<AddressableAssetEntry>();
        entriesAdded.Add(addressableSettings.CreateOrMoveEntry(levelPrefabGuid, addressableSettings.DefaultGroup));
        entriesAdded.Add(addressableSettings.CreateOrMoveEntry(thumbnailGuid, addressableSettings.DefaultGroup));

        var levelAssetEntry = addressableSettings.CreateOrMoveEntry(levelAssetGuid, addressableSettings.DefaultGroup);
        levelAssetEntry.labels.Add("GameMode_Free");
        levelAssetEntry.labels.Add("Release");
        entriesAdded.Add(levelAssetEntry);

        addressableSettings.DefaultGroup.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
        addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);

        AssetDatabase.SaveAssets();
        Selection.activeObject = levelAsset;

        EditorUtility.FocusProjectWindow();
    }
}