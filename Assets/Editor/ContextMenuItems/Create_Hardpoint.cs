using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BBI.Unity.Game;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public class Create_Hardpoint : EditorWindow
{
    [MenuItem("Assets/Create/Shipbreaker/Create hardpoint asset")]
    public static void CreateHardpointsset()
    {
        EditorWindow.CreateInstance<Create_Hardpoint>().Show();
    }

    string hardpointName;
    string assetRef;

    float weight;
    float weightEmpty;

    void OnGUI()
    {
        {
            GUILayout.Label("Create hardpoint asset", EditorStyles.boldLabel);

            GUILayout.Label("Hardpoint name", EditorStyles.label);
            hardpointName = GUILayout.TextField(hardpointName);
            GUILayout.Label("Hardpoint asset reference", EditorStyles.label);
            assetRef = GUILayout.TextField(assetRef);
            GUILayout.Label("Spawn weight", EditorStyles.label);
            weight = EditorGUILayout.FloatField(weight);
            GUILayout.Label("Spawn empty weight", EditorStyles.label);
            weightEmpty = EditorGUILayout.FloatField(weightEmpty);

            if (GUILayout.Button($"Create {hardpointName} hardpoint") && !string.IsNullOrWhiteSpace(hardpointName))
            {
                string folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
                if (folderPath.Contains("."))
                    folderPath = folderPath.Remove(folderPath.LastIndexOf('/'));
                
                var createdGUID = CreateHardpointAsset(folderPath, hardpointName, assetRef, weight, weightEmpty);
                
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(createdGUID));

                EditorUtility.FocusProjectWindow();
            }
        }
    }

    public static string CreateHardpointAsset(string folderPath, string hardpointName, string assetRef, float weight, float weightEmpty)
    {
        var moduleEntryContainer = CreateInstance<ModuleEntryContainer>();

        var moduleEntryDefinition = CreateInstance<ModuleEntryDefinition>();
        moduleEntryDefinition.ModuleDefRef = new AssetReferenceGameObject(assetRef);
        moduleEntryDefinition.Weight = weight;
        AssetDatabase.CreateAsset(moduleEntryDefinition, $"{folderPath}/{hardpointName}_ModuleEntryDefinition.asset");
        moduleEntryContainer.Add(moduleEntryDefinition);

        if(weightEmpty > 0)
        {
            var moduleEntryEmpty = CreateInstance<ModuleEntryEmpty>();
            moduleEntryEmpty.Weight = weightEmpty;
            AssetDatabase.CreateAsset(moduleEntryEmpty, $"{folderPath}/{hardpointName}_ModuleEntryEmpty.asset");
            moduleEntryContainer.Add(moduleEntryEmpty);
        }

        AssetDatabase.CreateAsset(moduleEntryContainer, $"{folderPath}/{hardpointName}_ModuleEntryContainer.asset");

        var moduleListAsset = CreateInstance<ModuleListAsset>();
        moduleListAsset.Data.ModuleEntryContainer = moduleEntryContainer;
        AssetDatabase.CreateAsset(moduleListAsset, $"{folderPath}/{hardpointName}_ModuleListAsset.asset");
        var moduleListAssetGuid = AssetDatabase.GUIDFromAssetPath($"{folderPath}/{hardpointName}_ModuleListAsset.asset").ToString();

        // Addressables
        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        var entriesAdded = new List<AddressableAssetEntry>();
        entriesAdded.Add(addressableSettings.CreateOrMoveEntry(moduleListAssetGuid, addressableSettings.DefaultGroup));
        addressableSettings.DefaultGroup.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
        addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);

        AssetDatabase.SaveAssets();

        return moduleListAssetGuid;
    }
}