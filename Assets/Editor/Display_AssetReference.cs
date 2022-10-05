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
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 3;
    }
 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float offset = 170;

        EditorGUI.BeginProperty(position, label, property);
        
        var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label);
     
        var refPathRect = new Rect(position.x + offset, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var isLocalRect = new Rect(position.x + offset, position.y + (EditorGUIUtility.singleLineHeight * 1), position.width, EditorGUIUtility.singleLineHeight);

        var refPath = "Unknown Asset";
        var knownPath = AssetDatabase.GUIDToAssetPath(property.FindPropertyRelative("m_AssetGUID").stringValue);
        if(knownPath == "")
        {
            if(LoadGameAssets.knownAssetMap.TryGetValue(property.FindPropertyRelative("m_AssetGUID").stringValue, out knownPath))
            {
                refPath = "Vanilla Asset";
            }
        }
        else
        {
            refPath = "Custom Asset";
        }

        EditorGUI.LabelField(refPathRect, refPath);

        if(knownPath != "")
        {
            if(EditorGUI.DropdownButton(isLocalRect, new GUIContent(knownPath), FocusType.Keyboard, EditorStyles.label))
            {
                if(refPath == "Vanilla Asset")
                {
                    Addressables.LoadAssetAsync<UnityEngine.Object>(new AssetReference(property.FindPropertyRelative("m_AssetGUID").stringValue)).Completed += res =>
                    {
                        Selection.activeObject = res.Result;
                    };
                }
                else
                {
                    var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(property.FindPropertyRelative("m_AssetGUID").stringValue));
                    if(asset is GameObject)
                        AssetDatabase.OpenAsset(asset);
                    else
                        Selection.activeObject = asset;
                }
            }
        }
        else
        {
            EditorGUI.LabelField(isLocalRect, "Unknown Asset");
        }


        var guidRect = new Rect(position.x + offset, position.y + (EditorGUIUtility.singleLineHeight * 2), position.width - offset, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(guidRect, property.FindPropertyRelative("m_AssetGUID"), GUIContent.none);

        EditorGUI.EndProperty(); 
    } 
}

[CustomPropertyDrawer(typeof(AssetReferenceScene))] public class AssetReferenceScenePropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferenceScriptableObject))] public class AssetReferenceScriptableObjectPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferenceJointabilityAsset))] public class AssetReferenceJointabilityAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferenceCompanyInfoAsset))] public class AssetReferenceCompanyInfoAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferenceMainSettingsAsset))] public class AssetReferenceMainSettingsAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferenceShipClassAsset))] public class AssetReferenceShipClassAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferencePropertyContainerAsset))] public class AssetReferencePropertyContainerAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferenceModuleConstructionAsset))] public class AssetReferenceModuleConstructionAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferenceModuleListAsset))] public class AssetReferenceModuleListAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferenceModuleSkinAsset))] public class AssetReferenceModuleSkinAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(AssetReferenceLevelAsset))] public class AssetReferenceLevelAssetPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceGameObject))] public class AssetReferenceGameObjectPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceTexture))] public class AssetReferenceTexturePropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceTexture2D))] public class AssetReferenceTexture2DPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceTexture3D))] public class AssetReferenceTexture3DPropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceSprite))] public class AssetReferenceSpritePropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}
[CustomPropertyDrawer(typeof(UnityEngine.AddressableAssets.AssetReferenceAtlasedSprite))] public class AssetReferenceAtlasedSpritePropertyDrawerOverride : AddressableAssetPropertyDrawerOverride {}