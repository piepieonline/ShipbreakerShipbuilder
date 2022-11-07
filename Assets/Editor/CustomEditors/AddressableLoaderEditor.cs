using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using BBI.Unity.Game;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(AddressableLoader))]
public class AddressableLoaderEditor : Editor
{
    // SerializeField is used to ensure the view state is written to the window 
    // layout file. This means that the state survives restarting Unity as long as the window
    // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
    [SerializeField] TreeViewState m_TreeViewState;

    //The TreeView is not serializable, so it should be reconstructed from the tree data.
    GameObjectTreeView m_SimpleTreeView;

    GameObject loadedGameObject;
    bool needToPaintObject = false;
    string paintedAddress = "";

    private bool isDefaultOpen = false;

    void OnEnable ()
    {
        // Check whether there is already a serialized view state (state 
        // that survived assembly reloading)
        if (m_TreeViewState == null)
            m_TreeViewState = new TreeViewState ();

        LoadAddressable();
    }

    void LoadAddressable()
    {
        var address = serializedObject.FindProperty("assetGUID").stringValue;
        if(address != null && address != "" && address != paintedAddress)
        {
            Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(address)).Completed += res =>
            {
                loadedGameObject = res.Result;
                needToPaintObject = true;
                paintedAddress = address;
                
                m_SimpleTreeView = new GameObjectTreeView(m_TreeViewState, loadedGameObject, serializedObject);
            };
        }
    }

    override public bool RequiresConstantRepaint()
    {
        if(needToPaintObject)
        {
            needToPaintObject = false;
            return true;
        }
        return base.RequiresConstantRepaint();
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update ();

        if(m_SimpleTreeView != null)
        {
            var treeRect = EditorGUILayout.GetControlRect(true, m_SimpleTreeView.totalHeight);
            m_SimpleTreeView.OnGUI(treeRect);
        }

        bool wasChanged = base.DrawDefaultInspector();
        
        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();

        if(wasChanged)
        {
            paintedAddress = "";
            LoadAddressable();
        }
    }
}

class GameObjectTreeView : TreeView
{
    private GameObject baseObject;
    private SerializedObject serializedObject;

    private int currentId = 0;
    private Dictionary<int, string> idToPathMap = new Dictionary<int, string>();

    // Caching from the AddressableLoader object
    string childPath = "";
    public List<string> disabledChildren = new List<string>();

    // Prevent recursion crashes
    // TODO: Configure this number?
    const int MAX_RECURSIVE_DEPTH = 10;
    int currentRecursiveDepth = 0;

    public GameObjectTreeView(TreeViewState treeViewState, GameObject baseObject, SerializedObject serializedObject)
        : base(treeViewState)
    {
        this.baseObject = baseObject;
        this.serializedObject = serializedObject;
        Reload();
    }
        
    protected override TreeViewItem BuildRoot ()
    {
        // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
        // are created from data. Here we create a fixed set of items. In a real world example,
        // a data model should be passed into the TreeView and the items created from the model.

        currentId = 0;
        childPath = serializedObject.FindProperty("childPath").stringValue;
        disabledChildren = ((AddressableLoader)serializedObject.targetObject).disabledChildren;

        // This section illustrates that IDs should be unique. The root item is required to 
        // have a depth of -1, and the rest of the items increment from that.
        var root = new GameObjectTreeViewItem {id = 0, depth = -1, displayName = "Root"};

        if(baseObject?.name == null) return root;

        var GORoot = new GameObjectTreeViewItem { id = currentId++, displayName = baseObject.name, displayState = childPath == "" ? GameObjectTreeViewItem.DisplayState.RootObject : GameObjectTreeViewItem.DisplayState.Disabled };
        root.AddChild(GORoot);

        // Recursive list children
        AddChildrenElements(GORoot, baseObject.transform, false, "");

        SetupDepthsFromParentsAndChildren(root);

        // Return root of the tree
        return root;
    }

    void AddChildrenElements(GameObjectTreeViewItem parentTreeElement, Transform parentTransform, bool parentDisabled, string currentPath)
    {
        if(currentRecursiveDepth > MAX_RECURSIVE_DEPTH) return;

        currentRecursiveDepth++;

        foreach(Transform child in parentTransform)
        {
            var newPath = currentPath == "" ? child.name : currentPath + "/" + child.name;
            // Disabled if the parent is disabled
            var isDisabled = parentDisabled ||
            // Disabled if it doesn't match the child path
                (!childPath.StartsWith(newPath) && !newPath.StartsWith(childPath)) ||
            // Disabled if it's in the disabledChildren array
            // Also needs to work with the childPath, to ensure we have the full path
                (newPath.StartsWith(childPath) && newPath != childPath && disabledChildren.Contains(childPath == "" ? newPath : newPath.Remove(0, childPath.Length + 1)));
            var isChildRoot = childPath == newPath;

            idToPathMap[currentId] = newPath;

            var treeElement = new GameObjectTreeViewItem {
                id = currentId++,
                displayName = child.name,
                displayState = isChildRoot ? GameObjectTreeViewItem.DisplayState.RootObject : isDisabled ? GameObjectTreeViewItem.DisplayState.Disabled : GameObjectTreeViewItem.DisplayState.Enabled
            };
            parentTreeElement.AddChild(treeElement);

            AddChildrenElements(treeElement, child, isDisabled, newPath);
        }

        currentRecursiveDepth--;
    }
    
    protected override void RowGUI (RowGUIArgs args)
    {
        var item = (GameObjectTreeViewItem) args.item;
        var itemRect = new Rect(args.rowRect);
        itemRect.xMin += GetContentIndent(item);

        GUIStyle disabledStyle = new GUIStyle(EditorStyles.label);
        switch(item.displayState)
        {
            case GameObjectTreeViewItem.DisplayState.Disabled:
                disabledStyle.normal.textColor = new Color(.5f, .5f, .5f);
                break;
            case GameObjectTreeViewItem.DisplayState.RootObject:
                disabledStyle.normal.textColor = new Color(0, 0.6f, 1f);
                break;
        }
        EditorGUI.LabelField(itemRect, item.displayName, disabledStyle);
        // item.displayName
    }
    
    // Enable/disable children on double click
    protected override void DoubleClickedItem(int id)
    {
        if(idToPathMap.TryGetValue(id, out var path) && path.StartsWith(childPath) && path != childPath)
        {
            if(childPath.Length > 0)
            {
                path = path.Remove(0, childPath.Length + 1);
            }

            if(disabledChildren.Contains(path))
            {
                disabledChildren.Remove(path);
            }
            else
            {
                disabledChildren.Add(path);
            }
            
            EditorUtility.SetDirty(serializedObject.targetObject);
            Reload();
        }
    }

    [System.Serializable]
    //The TreeElement data class is extended to hold extra data, which you can show and edit in the front-end TreeView.
    internal class GameObjectTreeViewItem : TreeViewItem
    {
        public DisplayState displayState = DisplayState.Enabled;

        public enum DisplayState
        {
            Enabled,
            Disabled,
            RootObject
        }
    }
}