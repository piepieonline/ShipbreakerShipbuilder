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
    HardpointAssetReference[] assetReferences = new HardpointAssetReference[0];
    bool assetReferencesOpen;
    float weightEmpty;

    void OnGUI()
    {
        {
            GUILayout.Label("Create hardpoint asset", EditorStyles.boldLabel);

            GUILayout.Label("Hardpoint name", EditorStyles.label);
            hardpointName = GUILayout.TextField(hardpointName);

            assetReferences = HardpointAssetReferenceArrayField("Asset references", ref assetReferencesOpen, assetReferences);
        
            GUILayout.Label("Spawn empty weight", EditorStyles.label);
            weightEmpty = EditorGUILayout.FloatField(weightEmpty);

            if (GUILayout.Button($"Create {hardpointName} hardpoint") && !string.IsNullOrWhiteSpace(hardpointName))
            {
                string folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
                if (folderPath.Contains("."))
                    folderPath = folderPath.Remove(folderPath.LastIndexOf('/'));
                
                var createdGUID = CreateHardpointAsset(folderPath, hardpointName, assetReferences, weightEmpty);
                
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(createdGUID));

                EditorUtility.FocusProjectWindow();
            }
        }
    }

    public static string CreateHardpointAsset(string folderPath, string hardpointName, HardpointAssetReference[] assetReferences, float weightEmpty)
    {
        var moduleEntryContainer = CreateInstance<ModuleEntryContainer>();

        foreach(var assetRef in assetReferences)
        {
            var moduleEntryDefinition = CreateInstance<ModuleEntryDefinition>();
            moduleEntryDefinition.ModuleDefRef = new AssetReferenceGameObject(assetRef.assetRef);
            moduleEntryDefinition.Weight = assetRef.weight;
            AssetDatabase.CreateAsset(moduleEntryDefinition, $"{folderPath}/{hardpointName}_ModuleEntryDefinition.asset");
            moduleEntryContainer.Add(moduleEntryDefinition);
        }

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

    public static HardpointAssetReference[] HardpointAssetReferenceArrayField(string label, ref bool open, HardpointAssetReference[] array) {
        open = EditorGUILayout.Foldout(open, label);
        int newSize = array.Length;

        if (open) {
            newSize = EditorGUILayout.IntField("Size", newSize);
            newSize = newSize < 0 ? 0 : newSize;

            if (newSize != array.Length) {
                array = ResizeArray<HardpointAssetReference>(array, newSize);
            }

            for (var i = 0; i < newSize; i++) {
                EditorGUILayout.BeginHorizontal();
                array[i].assetRef = EditorGUILayout.TextField("Reference: ", array[i].assetRef, GUILayout.ExpandWidth(true));
                array[i].weight = EditorGUILayout.FloatField("Weight: ", array[i].weight, GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
            }
        }
        return array;
    }

    private static T[] ResizeArray<T>(T[] array, int size) where T : new()
    {
        T[] newArray = new T[size];

        for (var i = 0; i < size; i++) {
            if (i < array.Length) {
                newArray[i] = array[i];
            }
            else
            {
                newArray[i] = new T();
            }
        }

        return newArray;
    }

    [System.Serializable]
    public class HardpointAssetReference
    {
        public string assetRef;
        public float weight;
    }
}