using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CopyGuid
{
    [MenuItem("Assets/Copy GUID")]
    public static void CopyGuidAction()
    {
        // If it's being copied from the editor cache prefab library, get the name (the games guid), not the cached asset guid
        var regexMatch = System.Text.RegularExpressions.Regex.Match(AssetDatabase.GetAssetPath(Selection.activeObject), "Assets/EditorCache/([A-Za-z0-9]{32})\\.prefab");
        if (regexMatch.Success)
        {
            GUIUtility.systemCopyBuffer = regexMatch.Groups[1].Value;
        }
        else if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out string guid, out long localId))
        {
            GUIUtility.systemCopyBuffer = guid;
        }
    }
}
