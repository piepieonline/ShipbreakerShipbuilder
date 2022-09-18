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

        if (AddressableRendering.drawRooms)
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

                    Gizmos.color = roomSubVolumeDefinition.Mode == RoomSubVolumeDefinition.InclusionMode.Include ? new Color(0, 1, 0, AddressableRendering.roomOpacity) : new Color(1, 0, 0, AddressableRendering.roomOpacity);
                    Gizmos.DrawCube(Vector3.zero, Vector3.one);
                }
            }
        }

        if (AddressableRendering.drawRoomOverlaps)
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

                    Gizmos.color = new Color(1, .5f, 0, AddressableRendering.roomOpacity);
                    Gizmos.DrawCube(Vector3.zero, Vector3.one);

                    if (AddressableRendering.drawRoomOverlapFlows)
                    {
                        Gizmos.color = new Color(1, .5f, 0, 1);
                        DrawArrow.ForGizmo(Vector3.zero, roomOpeningDefinition.FlowAxis == 1 ? Vector3.up : Vector3.forward);
                        DrawArrow.ForGizmo(Vector3.zero, roomOpeningDefinition.FlowAxis == 1 ? Vector3.down : Vector3.back);
                    }
                }
            }
        }
    }
}
