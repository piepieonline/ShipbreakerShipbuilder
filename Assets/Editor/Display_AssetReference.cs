using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine.AddressableAssets;
using BBI.Unity.Game;

[CustomPropertyDrawer(typeof(System.ObsoleteAttribute))]
public class ObsoleteAttributeHider : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
    }
}

[CustomPropertyDrawer(typeof(AssetReference))]
public class AddressableAssetPropertyDrawerOverride : PropertyDrawer
{
    public static bool shouldDisableOffset = false;

    UnityEngine.Object previewObj;

    static float previewImageHeight = 0;
    Texture2D previewImage;

    public static float GetPropertyHeight()
    {
        return EditorGUIUtility.singleLineHeight * 3 + previewImageHeight;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return GetPropertyHeight();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float editorIndentOffset = EditorGUI.indentLevel * 15;
        float offset = shouldDisableOffset ? 0 : 170;

        EditorGUI.BeginProperty(position, label, property);

        var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label);

        var refPathRect = new Rect(position.x + offset, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var isLocalRect = new Rect(position.x + offset + editorIndentOffset, position.y + (EditorGUIUtility.singleLineHeight * 1), position.width, EditorGUIUtility.singleLineHeight);

        var refPath = "Unknown Asset";
        var knownPath = AssetDatabase.GUIDToAssetPath(property.FindPropertyRelative("m_AssetGUID").stringValue);
        if (knownPath == "")
        {
            if (LoadGameAssets.knownAssetMap.TryGetValue(property.FindPropertyRelative("m_AssetGUID").stringValue, out knownPath))
            {
                refPath = "Vanilla Asset";
            }
        }
        else
        {
            refPath = "Custom Asset";
        }

        // Asset reference type (Custom/Vanilla)
        EditorGUI.LabelField(refPathRect, refPath);

        if (knownPath != "")
        {
            if (previewObj == null)
            {
                if (refPath == "Vanilla Asset")
                {
                    Addressables.LoadAssetAsync<UnityEngine.Object>(new AssetReference(property.FindPropertyRelative("m_AssetGUID").stringValue)).Completed += res =>
                    {
                        previewObj = res.Result;
                        previewImage = AssetPreview.GetAssetPreview(res.Result);
                        RepaintInspector(property.serializedObject);
                    };
                }
                else
                {
                    previewObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(property.FindPropertyRelative("m_AssetGUID").stringValue));
                    previewImage = AssetPreview.GetAssetPreview(previewObj);
                }
            }

            // Path, clickable to load the asset
            if (EditorGUI.DropdownButton(isLocalRect, new GUIContent(knownPath), FocusType.Keyboard, EditorStyles.label))
            {
                if (refPath == "Vanilla Asset")
                {
                    Selection.activeObject = previewObj;
                }
                else
                {
                    if (previewObj is GameObject)
                        AssetDatabase.OpenAsset(previewObj);
                    else
                        Selection.activeObject = previewObj;
                }
            }
        }
        else
        {
            EditorGUI.LabelField(isLocalRect, "Unknown Asset");
        }


        var guidRect = new Rect(position.x + offset, position.y + (EditorGUIUtility.singleLineHeight * 2), position.width - offset, EditorGUIUtility.singleLineHeight);

        // GUID textbox
        EditorGUI.PropertyField(guidRect, property.FindPropertyRelative("m_AssetGUID"), GUIContent.none);

        if (previewImage != null)
        {
            previewImageHeight = previewImage.height;
            var previewBackgroundRect = new Rect(position.x + offset + editorIndentOffset, guidRect.yMax + 2, position.width - offset - editorIndentOffset, previewImage.height);
            var previewRectXStart = position.x + offset + (((position.width - offset) / 2) - (previewImage.width / 2f));
            var previewRect = new Rect(previewRectXStart, guidRect.yMax + 2, previewImage.width, previewImage.height);

            EditorGUI.DrawRect(previewBackgroundRect, new Color(0.031f, 0.031f, 0.027f));
            EditorGUI.DrawPreviewTexture(previewRect, previewImage);
        }

        EditorGUI.EndProperty();
    }

    public static void RepaintInspector(SerializedObject BaseObject)
    {
        foreach (var item in ActiveEditorTracker.sharedTracker.activeEditors)
            if (item.serializedObject == BaseObject)
            { item.Repaint(); return; }
    }
}

[CustomPropertyDrawer(typeof(AssetReferenceScene))] public class AssetReferenceScenePropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferenceScriptableObject))] public class AssetReferenceScriptableObjectPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferenceJointabilityAsset))] public class AssetReferenceJointabilityAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferenceCompanyInfoAsset))] public class AssetReferenceCompanyInfoAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferenceMainSettingsAsset))] public class AssetReferenceMainSettingsAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferenceShipClassAsset))] public class AssetReferenceShipClassAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferencePropertyContainerAsset))] public class AssetReferencePropertyContainerAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferenceModuleConstructionAsset))] public class AssetReferenceModuleConstructionAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferenceModuleListAsset))] public class AssetReferenceModuleListAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferenceModuleSkinAsset))] public class AssetReferenceModuleSkinAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(AssetReferenceLevelAsset))] public class AssetReferenceLevelAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceGameObject))] public class AssetReferenceGameObjectPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceTexture))] public class AssetReferenceTexturePropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceTexture2D))] public class AssetReferenceTexture2DPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceTexture3D))] public class AssetReferenceTexture3DPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceSprite))] public class AssetReferenceSpritePropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceAtlasedSprite))] public class AssetReferenceAtlasedSpritePropertyDrawerOverride : AddressableAssetPropertyDrawerOverride { }