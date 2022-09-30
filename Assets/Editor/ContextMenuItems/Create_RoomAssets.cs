using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BBI.Unity.Game;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public class Create_RoomAssets : EditorWindow
{
    [MenuItem("Assets/Create/Shipbreaker/Create room asset")]
    public static void CreateRoomAsset()
    {
        EditorWindow.CreateInstance<Create_RoomAssets>().Show();
    }

    string roomName;

    void OnGUI()
    {
        {
            GUILayout.Label("Create room asset", EditorStyles.boldLabel);
            GUILayout.Label("Room name", EditorStyles.label);
            roomName = GUILayout.TextField(roomName);
            if (GUILayout.Button($"Create {roomName} room") && !string.IsNullOrWhiteSpace(roomName))
            {
                CreateAsset();
            }
        }
    }

    void CreateAsset()
    {
        string folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        if (folderPath.Contains("."))
            folderPath = folderPath.Remove(folderPath.LastIndexOf('/'));

        // Prefab
        var parent = new GameObject($"{roomName}_Room", new System.Type[] { typeof(ModuleDefinition) });

        var room = new GameObject("Room", new System.Type[] { typeof(RoomContainerDefinition) });
        room.transform.parent = parent.transform;
        var soLoader = room.AddComponent<AddressableSOLoader>();
        soLoader.comp.Add("BBI.Unity.Game.RoomContainerDefinition");
        soLoader.field.Add("m_DynamicRoomContainerAsset");
        soLoader.refs.Add("");

        var volume = new GameObject("Volume", new System.Type[] { typeof(RoomSubVolumeDefinition) });
        volume.transform.parent = room.transform;

        PrefabUtility.SaveAsPrefabAsset(parent, $"{folderPath}/{roomName}Room.prefab");
        var roomGuid = AssetDatabase.GUIDFromAssetPath($"{folderPath}/{roomName}Room.prefab").ToString();
        DestroyImmediate(parent);

        // Scriptable objects
        ModuleEntryDefinition moduleEntryDefinitionAsset = CreateInstance<ModuleEntryDefinition>();
        moduleEntryDefinitionAsset.ModuleDefRef = new AssetReferenceGameObject(roomGuid);
        AssetDatabase.CreateAsset(moduleEntryDefinitionAsset, $"{folderPath}/{roomName}_ModuleEntryDefinition.asset");

        ModuleEntryContainer moduleEntryContainerAsset = CreateInstance<ModuleEntryContainer>();
        moduleEntryContainerAsset.Data.Add(moduleEntryDefinitionAsset);
        AssetDatabase.CreateAsset(moduleEntryContainerAsset, $"{folderPath}/{roomName}_ModuleEntryContainer.asset");

        ModuleListAsset moduleListAsset = CreateInstance<ModuleListAsset>();
        moduleListAsset.Data.ModuleEntryContainer = moduleEntryContainerAsset;
        AssetDatabase.CreateAsset(moduleListAsset, $"{folderPath}/{roomName}_ModuleListAsset.asset");
        var moduleListAssetGuid = AssetDatabase.GUIDFromAssetPath($"{folderPath}/{roomName}_ModuleListAsset.asset").ToString();

        // Addressables
        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        var entriesAdded = new List<AddressableAssetEntry>();
        entriesAdded.Add(addressableSettings.CreateOrMoveEntry(roomGuid, addressableSettings.DefaultGroup));
        entriesAdded.Add(addressableSettings.CreateOrMoveEntry(moduleListAssetGuid, addressableSettings.DefaultGroup));
        addressableSettings.DefaultGroup.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
        addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);

        AssetDatabase.SaveAssets();
        Selection.activeObject = moduleEntryContainerAsset;

        EditorUtility.FocusProjectWindow();
    }
}