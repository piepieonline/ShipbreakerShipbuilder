using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BBI.Unity.Game;

public class GameRenderWindow : EditorWindow
{
    public static bool drawRooms = true;
    public static Color roomColorInclude = new Color(0, 1, 0, .2f);
    public static Color roomColorExclude = new Color(1, 0, 0, .2f);
    public static bool drawRoomOverlaps = true;
    public static Color roomOverlapColor = new Color(.14f, .63f, .58f, .35f);
    public static bool drawRoomOverlapFlows = false;
    public static Color roomOverlapFlowColor = new Color(1, .5f, 0, 1);

    [MenuItem("Shipbreaker/Show Render Controller", priority = 100)]
    public static void ShowRenderController()
    {
        EditorWindow.CreateInstance<GameRenderWindow>().Show();
    }
    
    void OnGUI()
    {
        if (GUILayout.Button("Redraw"))
        {
            AddressableRendering.UpdateViewList();
        }

        if (GUILayout.Button("Clear all"))
        {
            AddressableRendering.ClearView();
        }

        GUILayout.Label("Room volumes", EditorStyles.boldLabel);
        drawRooms = GUILayout.Toggle(drawRooms, "Draw Rooms");
        GUILayout.Label("Room volume colors", EditorStyles.label);
        roomColorInclude = EditorGUILayout.ColorField(roomColorInclude);
        roomColorExclude = EditorGUILayout.ColorField(roomColorExclude);

        GUILayout.Label("Room overlaps", EditorStyles.boldLabel);
        drawRoomOverlaps = GUILayout.Toggle(drawRoomOverlaps, "Draw Room Overlaps");
        GUILayout.Label("Overlap Color", EditorStyles.label);
        roomOverlapColor = EditorGUILayout.ColorField(roomOverlapColor);
        drawRoomOverlapFlows = GUILayout.Toggle(drawRoomOverlapFlows, "Draw Room Overlap Flows");
        GUILayout.Label("Overlap Color", EditorStyles.label);
        roomOverlapFlowColor = EditorGUILayout.ColorField(roomOverlapFlowColor);
    }
}
