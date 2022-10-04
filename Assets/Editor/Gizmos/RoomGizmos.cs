using BBI.Unity.Game;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomGizmos
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NotInSelectionHierarchy)]
    static void DrawGizmoForMyScript(ModuleDefinition scr, GizmoType gizmoType)
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Contains(scr.gameObject))
            return;

        if (GameRenderWindow.drawRooms)
        {
            foreach (var room in AddressableRendering.rooms)
            {
                if (room.parent == null || !room.parent.gameObject.activeInHierarchy) continue;

                if (room.parent.TryGetComponent<RoomSubVolumeDefinition>(out var roomSubVolumeDefinition))
                {
                    Matrix4x4 parentMatrix = Matrix4x4.TRS(room.parent.position, room.parent.rotation, room.parent.lossyScale);
                    Matrix4x4 childMatrix = Matrix4x4.TRS(roomSubVolumeDefinition.Center, Quaternion.identity, roomSubVolumeDefinition.Size);

                    Matrix4x4 transformMatrix = parentMatrix * childMatrix;

                    Gizmos.matrix = transformMatrix;

                    Gizmos.color = roomSubVolumeDefinition.Mode == RoomSubVolumeDefinition.InclusionMode.Include ? GameRenderWindow.roomColorInclude : GameRenderWindow.roomColorExclude;
                    Gizmos.DrawCube(Vector3.zero, Vector3.one);
                }
            }
        }

        if (GameRenderWindow.drawRoomOverlaps)
        {
            foreach (var room in AddressableRendering.roomOverlaps)
            {
                if (room.parent == null || !room.parent.gameObject.activeInHierarchy) continue;

                if (room.parent.TryGetComponent<RoomOpeningDefinition>(out var roomOpeningDefinition))
                {
                    Matrix4x4 parentMatrix = Matrix4x4.TRS(room.parent.position, room.parent.rotation, room.parent.lossyScale);
                    Matrix4x4 childMatrix = Matrix4x4.TRS(roomOpeningDefinition.Center, Quaternion.identity, roomOpeningDefinition.Size);

                    Matrix4x4 transformMatrix = parentMatrix * childMatrix;

                    Gizmos.matrix = transformMatrix;

                    Gizmos.color = GameRenderWindow.roomOverlapColor;
                    
                    float overlapBorderSize = 0.02f;

                    Gizmos.DrawCube(new Vector3(-.5f, 0, 0), new Vector3(overlapBorderSize, 1f, 1f));
                    Gizmos.DrawCube(new Vector3(.5f, 0, 0), new Vector3(overlapBorderSize, 1f, 1f));
                    
                    Gizmos.DrawCube(new Vector3(0, 0, -.5f), new Vector3(1f, 1f, overlapBorderSize));
                    Gizmos.DrawCube(new Vector3(0, 0, .5f), new Vector3(1f, 1f, overlapBorderSize));

                    if (GameRenderWindow.drawRoomOverlapFlows)
                    {
                        Gizmos.color = GameRenderWindow.roomOverlapFlowColor;
                        DrawArrow.ForGizmo(Vector3.zero, roomOpeningDefinition.FlowAxis == 1 ? Vector3.up : Vector3.forward);
                        DrawArrow.ForGizmo(Vector3.zero, roomOpeningDefinition.FlowAxis == 1 ? Vector3.down : Vector3.back);
                    }
                }
            }
        }
    }
}
