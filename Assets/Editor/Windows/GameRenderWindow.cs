using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BBI.Unity.Game;

public class GameRenderWindow : EditorWindow
{
    [MenuItem("Shipbreaker/Show Render Controller", priority = 100)]
    public static void ShowRenderController()
    {
        EditorWindow.CreateInstance<GameRenderWindow>().Show();
    }
    
    void OnGUI()
    {
        // GUILayout.Label("Create room asset", EditorStyles.boldLabel);
        GUILayout.Label("Asset GUID", EditorStyles.label);

        if (GUILayout.Button("Redraw"))
        {
            AddressableRendering.UpdateViewList();
        }

        if (GUILayout.Button("Clear all"))
        {
            AddressableRendering.ClearView();
        }

        AddressableRendering.drawRooms = GUILayout.Toggle(AddressableRendering.drawRooms, "Draw Rooms");
        AddressableRendering.drawRoomOverlaps = GUILayout.Toggle(AddressableRendering.drawRoomOverlaps, "Draw Room Overlaps");
        AddressableRendering.drawRoomOverlapFlows = GUILayout.Toggle(AddressableRendering.drawRoomOverlapFlows, "Draw Room Overlap Flows");
        AddressableRendering.roomOpacity = float.Parse(GUILayout.TextField(AddressableRendering.roomOpacity.ToString(), "Rooms Opacity"));
    }
}
