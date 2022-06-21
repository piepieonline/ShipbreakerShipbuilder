using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CopyGuid
{
    [MenuItem("Assets/Copy GUID")]
    public static void Init()
    {
        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out string guid, out long localId))
        {
            // Debug.Log(guid);
            GUIUtility.systemCopyBuffer = guid;
        }
    }
}
