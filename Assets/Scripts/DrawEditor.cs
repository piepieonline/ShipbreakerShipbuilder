#if UNITY_EDITOR

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using BBI.Unity.Game;

/* TODO
 * Load from editor assets first, before checking addressables/cache
*/ 

[ExecuteInEditMode]
public class DrawEditor : MonoBehaviour
{
    public static DrawEditor Instance;

    public bool updateView = false;
    public bool clearAddressables = false;

    public bool drawRooms = true;
    public bool drawRoomOverlaps = true;
    public float roomOpacity = .1f;

    static List<RenderableMapping> addressables = new List<RenderableMapping>();
    static List<RenderableMapping> rooms = new List<RenderableMapping>();
    static List<RenderableMapping> roomOverlaps = new List<RenderableMapping>();

    static List<GameObject> fakes = new List<GameObject>();

    static Dictionary<string, string> prefabToHardpoint = new Dictionary<string, string>();

    [ExecuteInEditMode]
    void OnEnable()
    {
        Instance = this;
    }

    [ExecuteInEditMode]
    void Update()
    {
        var cam = SceneView.currentDrawingSceneView ? SceneView.currentDrawingSceneView.camera : SceneView.lastActiveSceneView.camera;
        Draw(cam);
    }

    void OnDrawGizmos()
    {
        if(drawRooms)
        {
            foreach (var room in rooms)
            {
                if (room.parent == null || !room.parent.gameObject.activeInHierarchy) continue;

                Matrix4x4 parentMatrix = Matrix4x4.TRS(room.parent.position, room.parent.rotation, room.parent.lossyScale); 

                Gizmos.matrix = parentMatrix;
                
                Gizmos.color = new Color(0, 1, 0, roomOpacity);
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
            }
        }
        
        if(drawRoomOverlaps)
        {
            foreach (var room in roomOverlaps)
            {
                if (room.parent == null || !room.parent.gameObject.activeInHierarchy) continue;

                if(room.parent.TryGetComponent<RoomOpeningDefinition>(out var roomOpeningDefinition))
                {
                    Matrix4x4 parentMatrix = Matrix4x4.TRS(room.parent.position, room.parent.rotation, room.parent.lossyScale);
                    Matrix4x4 childMatrix = Matrix4x4.TRS(roomOpeningDefinition.Center, Quaternion.identity, roomOpeningDefinition.Size);

                    Gizmos.matrix = parentMatrix * childMatrix;

                    Gizmos.color = new Color(1, 0, 0, roomOpacity);
                    Gizmos.DrawCube(Vector3.zero, Vector3.one);
                }
            }
        }
    }

    private void Draw(Camera camera)
    {
        foreach (var addressable in addressables) DrawRenderable(addressable);
    }

    private void DrawRenderable(RenderableMapping renderableMapping)
    {
        if (renderableMapping.parent == null || !renderableMapping.parent.gameObject.activeInHierarchy) return;

        Matrix4x4 parentMatrix = Matrix4x4.TRS(renderableMapping.parent.position, renderableMapping.parent.rotation, Vector3.one);
        Matrix4x4 childMatrix = Matrix4x4.TRS(renderableMapping.offset, renderableMapping.rotation, renderableMapping.scale);

        for (int i = 0; i < renderableMapping.mesh.subMeshCount; i++)
        {
            Graphics.DrawMeshInstanced(renderableMapping.mesh, i, renderableMapping.mats[i], new Matrix4x4[] { parentMatrix * childMatrix }, 1);
        }
    }

    void OnDisable()
    {
        isUpdating = false;

        foreach (var fakePrefab in fakes)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(fakePrefab);
            };
        }
    }

    void OnValidate()
    {
        if(clearAddressables)
        {
            clearAddressables = false;
            addressables.Clear();
            rooms.Clear();
            roomOverlaps.Clear();

            foreach (var fakePrefab in fakes)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(fakePrefab);
                };
            }
        }

        if(updateView)
        {
            isUpdating = false;
            updateView = false;
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UpdateViewList();
            }; 
        }
    }

    static bool isUpdating = false;

    public async static void UpdateViewList()
    {
        if(isUpdating) return;
        isUpdating = true; 

        foreach (var fakePrefab in fakes)
        {
            DestroyImmediate(fakePrefab);
        }

        addressables.Clear();
        rooms.Clear();
        roomOverlaps.Clear();
        bool needToRefreshCache = false;
        List<(string, Transform)> addressablesToLoad = new List<(string, Transform)>();
        List<HardPoint> hardPoints = new List<HardPoint>();

        var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var rootGameObject in rootObjects)
        {
            if(rootGameObject.TryGetComponent<BBI.Unity.Game.ModuleDefinition>(out var moduleDefinition))
            {
                foreach (var addressable in rootGameObject.GetComponentsInChildren<BBI.Unity.Game.AddressableLoader>())
                {
                    foreach (var addressRef in addressable.refs)
                    {
                        addressablesToLoad.Add((addressRef, addressable.transform));

                        if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID($"Assets/EditorCache/{addressRef}.prefab")))
                        {
                            needToRefreshCache = true;
                        }
                    }
                }

                foreach(var hardpoint in rootGameObject.GetComponentsInChildren<HardPoint>())
                {
                    hardPoints.Add(hardpoint);

                    if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID($"Assets/EditorCache/{hardpoint.AssetRef.AssetGUID}.prefab")))
                    {
                        needToRefreshCache = true;
                    }
                }

                if (!needToRefreshCache || LoadAddressables.handle1.IsValid() && LoadAddressables.handle2.IsValid())
                {
                    foreach (var addressable in addressablesToLoad)
                    {
                        await LoadAddress(addressable.Item1, addressable.Item2, false, true);
                    }

                    foreach(var hardpoint in hardPoints)
                    {
                        var realID = await LoadHardpoint(hardpoint);
                        await LoadAddress(realID, hardpoint.transform, true, true);
                    }
                }
                else
                {
                    Debug.Log("Please load the catalogs");
                }
            }
        }

        isUpdating = false;
    }

    async static System.Threading.Tasks.Task<string> LoadHardpoint(HardPoint hardPoint)
    {
        if(System.IO.File.Exists($"{Application.dataPath}/EditorCache/{hardPoint.AssetRef.AssetGUID}.prefab"))
        {
            return hardPoint.AssetRef.AssetGUID;
        }
        else if (!string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(hardPoint.AssetRef.AssetGUID)))
        {
            var moduleEntry = AssetDatabase.LoadAssetAtPath<ModuleListAsset>(AssetDatabase.GUIDToAssetPath(hardPoint.AssetRef.AssetGUID)).Data.ModuleEntryContainer.Data.FirstOrDefault();
            // if (moduleEntry == null) return;
            if (moduleEntry.GetType() == typeof(ModuleEntryDefinition))
            {
                prefabToHardpoint[((ModuleEntryDefinition)moduleEntry).ModuleDefRef.AssetGUID] = hardPoint.AssetRef.AssetGUID;
                return ((ModuleEntryDefinition)moduleEntry).ModuleDefRef.AssetGUID;
            }
        }
        else
        {
            var guid = await LoadHardpointGuidFromModuleListAsset(hardPoint.AssetRef.AssetGUID);
            prefabToHardpoint[guid] = hardPoint.AssetRef.AssetGUID;
            return guid;
        }

        throw new System.Exception("LoadHardpoint");
    }

    async static System.Threading.Tasks.Task<string> LoadHardpointGuidFromModuleListAsset(string moduleListAssetGuid)
    {
        var res = Addressables.LoadAssetAsync<ModuleListAsset>(moduleListAssetGuid);
        await res.Task;

        if (res.IsValid())
        {
            var moduleEntry = res.Result.Data.ModuleEntryContainer.Data.FirstOrDefault();
            // if (moduleEntry == null) return;
            if (moduleEntry.GetType() == typeof(ModuleEntryDefinition))
            {
                return ((ModuleEntryDefinition)moduleEntry).ModuleDefRef.AssetGUID;
            }
            else if (moduleEntry.GetType() == typeof(ModuleEntryList))
            {
                return await LoadHardpointGuidFromModuleListAsset(((ModuleEntryList)moduleEntry).ModuleListRef.AssetGUID);
            }
        }

        throw new System.Exception("LoadHardpointGuidFromModuleListAsset");
    }

    async static System.Threading.Tasks.Task<GameObject> LoadAddress(string addressRef, Transform parent, bool isHardpoint, bool addToRenderList)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{addressRef}.prefab");

        if(!prefab)
        {
            var res = Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(addressRef));
            await res.Task;

            if (res.IsValid())
            {
                if (res.Result.TryGetComponent<BBI.Unity.Game.AddressableLoader>(out var loader))
                {
                    for (int i = 0; i < loader.refs.Count; i++)
                    {
                        await LoadAddress(loader.refs[i], res.Result.transform, false, addToRenderList);
                    }
                }

                await TryCacheAsset(addressRef, res.Result, parent, isHardpoint);

                foreach(var hardpoint in res.Result.GetComponentsInChildren<HardPoint>())
                {
                    var hardpointAddress = await LoadHardpoint(hardpoint);
                    await LoadAddress(hardpointAddress, hardpoint.transform, isHardpoint, addToRenderList);
                }

                return res.Result;
            }
        }
        else
        {
            var temp = Instantiate(prefab, parent);
            temp.name = addressRef;
            temp.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;

            if (isHardpoint)
                temp.transform.GetChild(0).localPosition = Vector3.zero;

            foreach (var room in temp.GetComponentsInChildren<RoomSubVolumeDefinition>())
            {
                rooms.Add(RenderableMapping.RoomMapping(room.transform, isHardpoint));
            }

            foreach(var roomOverlap in temp.GetComponentsInChildren<RoomOpeningDefinition>())
            {
                roomOverlaps.Add(RenderableMapping.RoomMapping(roomOverlap.transform, isHardpoint));
            }

            fakes.Add(temp);

            /*
            foreach (var hardpoint in temp.GetComponentsInChildren<HardPoint>())
            {
                var hardpointPrefab = await LoadAddress(hardpoint.AssetRef.AssetGUID, hardpoint.transform, true, false);
            }
            */

            foreach (var hardpoint in temp.GetComponentsInChildren<FakeHardpoint>())
            {
                var hardpointPrefab = await LoadAddress(hardpoint.AssetGUID, hardpoint.transform, true, false);
            }
        }

        return prefab;
    }

    static int count = 0;
    async static System.Threading.Tasks.Task TryCacheAsset(string address, GameObject obj, Transform parent, bool isHardpoint)
    {
        count = 0;
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{address}.prefab");
        if (!prefab)
        {
            prefab = new GameObject(address);

            await CloneMeshTree(address, obj.transform, prefab.transform);

            PrefabUtility.SaveAsPrefabAsset(prefab, $"Assets/EditorCache/{address}.prefab");
            DestroyImmediate(prefab);
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{address}.prefab");
        }

        if(isHardpoint)
        {
            var hardpointPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{prefabToHardpoint[address]}.prefab");

            if(!hardpointPrefab)
            {
                var tempGO = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                PrefabUtility.SaveAsPrefabAsset(tempGO, $"Assets/EditorCache/{prefabToHardpoint[address]}.prefab");
                DestroyImmediate(tempGO);
            }
        }

        foreach (var hardpoint in prefab.GetComponentsInChildren<FakeHardpoint>())
        {
            await LoadAddress(hardpoint.AssetGUID, hardpoint.transform, isHardpoint, true);
        }
    }

    private async static System.Threading.Tasks.Task CloneMeshTree(string address, Transform inTransform, Transform outParent)
    {
        var newPrefabChild = new GameObject($"{inTransform.name}_{address}");
        newPrefabChild.transform.parent = outParent;
        newPrefabChild.transform.localPosition = inTransform.localPosition;
        newPrefabChild.transform.localRotation = inTransform.localRotation;
        newPrefabChild.transform.localScale = inTransform.localScale;
        newPrefabChild.AddComponent<SelectAddressableParent>();
        foreach (Transform child in inTransform)
        {
            await CloneMeshTree(address, child, newPrefabChild.transform);
        }

        var meshPath = $"Assets/EditorCache/{address}_mesh";
        
        var shaderPath = $"Assets/EditorCache/{address}_sha";

        if (inTransform.TryGetComponent<MeshRenderer>(out var meshRenderer))
        {
            var meshFilter = inTransform.GetComponent<MeshFilter>();
            var suffix = $"_{count}";

            AssetDatabase.CreateAsset(Instantiate(meshFilter.sharedMesh), $"{meshPath}{suffix}.asset");
            newPrefabChild.AddComponent<MeshFilter>().sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>($"{meshPath}{suffix}.asset");

            MeshRenderer newRenderer = newPrefabChild.AddComponent<MeshRenderer>();
            newRenderer.sharedMaterials = meshRenderer.sharedMaterials.Select((mat, matIndex) => CloneMaterial(address, suffix, mat, matIndex)).ToArray();

            count++;
        }

        if(inTransform.TryGetComponent<RoomSubVolumeDefinition>(out var roomVolume))
        {
            var newRoomVolume = newPrefabChild.AddComponent<RoomSubVolumeDefinition>();
            EditorUtility.CopySerialized(roomVolume, newRoomVolume);
            newRoomVolume.transform.localScale = roomVolume.Size;
            newRoomVolume.transform.localPosition = roomVolume.Center;
        }

        if (inTransform.TryGetComponent<RoomOpeningDefinition>(out var roomOpening))
        {
            var newRoomOpening = newPrefabChild.AddComponent<RoomOpeningDefinition>();
            EditorUtility.CopySerialized(roomOpening, newRoomOpening);
            // newRoomOpening.transform.localScale = roomOpening.Size;
            // newRoomOpening.transform.localPosition = roomOpening.Center;
        }

        if (inTransform.TryGetComponent<HardPoint>(out var hardPoint))
        {
            var assetGUID = await LoadHardpoint(hardPoint);

            var newHardpoint = newPrefabChild.AddComponent<FakeHardpoint>();
            newHardpoint.AssetGUID = assetGUID;
        }
    }

    static Texture2D DuplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.sRGB);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    static Material CloneMaterial(string address, string suffix, Material material, int materialIndex)
    {
        var matPath = $"Assets/EditorCache/{address}_mat_{suffix}_{materialIndex}";
        var texturePath = $"/EditorCache/{address}_tex_{suffix}_{materialIndex}";
        
        System.IO.File.WriteAllBytes($"{Application.dataPath}{texturePath}.png", DuplicateTexture((Texture2D)material.GetTexture("_BaseColorMap")).EncodeToPNG());
        AssetDatabase.Refresh();

        var tempTexture = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/{texturePath}.png");
        EditorUtility.SetDirty(tempTexture);

        var tempMaterial = new Material(Shader.Find("HDRP/Lit")); // Currently can't use the game's material, so just use the default
        tempMaterial.SetTexture("_BaseColorMap", tempTexture);
        tempMaterial.enableInstancing = true;
        AssetDatabase.CreateAsset(tempMaterial, $"{matPath}.mat");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return AssetDatabase.LoadAssetAtPath<Material>($"{matPath}.mat");
    }

    class RenderableMapping
    {
        public Mesh mesh;
        public Material[] mats;
        public Transform parent;
        public Vector3 offset;
        public Quaternion rotation;
        public Vector3 scale;

        private RenderableMapping() { }

        public static RenderableMapping AddressableMapping(Mesh _mesh, Material[] _mats, Transform _parent, Transform _offsetParent, Transform _offset, bool _hardpoint)
        {
            return new RenderableMapping()
            {
                mesh = _mesh,
                mats = _mats,
                parent = _parent,
                offset = _hardpoint ? _offset.position - _offsetParent.GetChild(0).position : _offset.position,
                rotation = _offset.rotation,
                scale = _offset.lossyScale
            };
        }

        public static RenderableMapping AddressableHardpointMapping(Mesh _mesh, Material[] _mats, Transform _parent, Transform _offsetParent, Transform _offset)
        {
            return new RenderableMapping()
            {
                mesh = _mesh,
                mats = _mats,
                parent = _parent,
                offset = _offsetParent.position + _offset.position,
                rotation = _offsetParent.rotation * _offset.rotation,
                scale = Vector3.Scale(_offsetParent.lossyScale, _offset.lossyScale)
            };
        }

        public static RenderableMapping RoomMapping(Transform _parent, bool _hardpoint)
        {
            return new RenderableMapping()
            {
                parent = _parent
            };
        }
    }
}

#endif