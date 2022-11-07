using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine.AddressableAssets;
using BBI.Unity.Game;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(AddressableComponentLoader))]
public class AddressableComponentLoaderEditor : Editor
{
    UnityEditorInternal.ReorderableList reorderableList;

    bool[] foldoutsOpen = new bool[0];

    void OnEnable()
    {
        // Setup the SerializedProperties.
        var componentValues = serializedObject.FindProperty("componentValues");

        reorderableList = new UnityEditorInternal.ReorderableList(serializedObject, componentValues);

        reorderableList.headerHeight = 0;

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            
            if(foldoutsOpen.Length != reorderableList.serializedProperty.arraySize)
            {
                foldoutsOpen = new bool[reorderableList.serializedProperty.arraySize];
            }

            var position = new Rect(rect);

            EditorGUI.indentLevel++;

            foldoutsOpen[index] = EditorGUI.Foldout(new Rect(position.x, position.y, 10, EditorGUIUtility.singleLineHeight), foldoutsOpen[index], element?.FindPropertyRelative("component")?.objectReferenceValue?.GetType().ToString() ?? "Missing Component");

            // EditorGUI.PropertyField(new Rect(position.x + 200, position.y, rect.width - 200, EditorGUIUtility.singleLineHeight), element, GUIContent.none);

            if (foldoutsOpen[index])
            {
                var rect1 = new Rect(position.xMin, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
                var rect2 = new Rect(position.xMin, position.y + (EditorGUIUtility.singleLineHeight * 2), position.width, EditorGUIUtility.singleLineHeight);
                var rect3 = new Rect(position.xMin, position.y + (EditorGUIUtility.singleLineHeight * 3), position.width, EditorGUIUtility.singleLineHeight);
                var rect4 = new Rect(position.xMin, position.y + (EditorGUIUtility.singleLineHeight * 4), position.width, EditorGUIUtility.singleLineHeight);

                Component selectedComponent = (Component)element.FindPropertyRelative("component").objectReferenceValue;

                var fields = selectedComponent?.GetType()
                    .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Where(fi => !fi.FieldType.IsPrimitive)
                ;

                List<string> componentFields = fields == null ? new List<string>() : fields.Select(fi => fi.Name).ToList();
                int selectedIndex = componentFields.IndexOf(element.FindPropertyRelative("field").stringValue);

                serializedObject.Update();
                EditorGUI.PropertyField(rect1, element.FindPropertyRelative("component"));
                EditorGUI.BeginProperty(rect2, new GUIContent("Field"), element.FindPropertyRelative("field"));
                element.FindPropertyRelative("field").stringValue = componentFields.ElementAtOrDefault(EditorGUI.Popup(rect2, selectedIndex, componentFields.ToArray()));
                EditorGUI.EndProperty();
                // EditorGUI.PropertyField(rect2, element.FindPropertyRelative("field"));
                EditorGUI.PropertyField(rect3, element.FindPropertyRelative("address"));
                serializedObject.ApplyModifiedProperties();

                // Label for address
                var refPath = "Unknown Asset";
                var knownPath = AssetDatabase.GUIDToAssetPath(element.FindPropertyRelative("address").stringValue);
                if (knownPath == "")
                {
                    if (LoadGameAssets.knownAssetMap.TryGetValue(element.FindPropertyRelative("address").stringValue, out knownPath))
                    {
                        refPath = knownPath;
                    }
                }
                else
                {
                    refPath = knownPath;
                }

                // Asset reference type (Custom/Vanilla)
                EditorGUI.LabelField(rect4, refPath);
            }

            EditorGUI.indentLevel--;
        };

        reorderableList.elementHeightCallback = (index) => {
            return foldoutsOpen.ElementAtOrDefault(index) ? EditorGUIUtility.singleLineHeight * 6 : EditorGUIUtility.singleLineHeight;
        };
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update ();

        // Show the custom GUI controls.
        EditorGUI.BeginChangeCheck();
        reorderableList.DoLayoutList();
        EditorGUI.EndChangeCheck();
        

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties ();
    }
}