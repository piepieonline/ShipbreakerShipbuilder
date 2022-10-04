using BBI.Unity.Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableRendering : MonoBehaviour
{
    

    public static List<RenderableMapping> rooms = new List<RenderableMapping>();
    public static List<RenderableMapping> roomOverlaps = new List<RenderableMapping>();

    static bool isUpdating = false;

    static List<GameObject> fakes = new List<GameObject>();

    static Dictionary<string, string> prefabToHardpoint = new Dictionary<string, string>();

    static Dictionary<string, Mesh> meshCache = new Dictionary<string, Mesh>();

    public static void ClearView()
    {
        foreach (var fakePrefab in fakes)
        {
            DestroyImmediate(fakePrefab.gameObject);
        }

        fakes.Clear();
        rooms.Clear();
        roomOverlaps.Clear();
    }

    public async static void UpdateViewList()
    {
        if (isUpdating) return;
        isUpdating = true;

        ClearView();

        try
        {
            bool needToRefreshCache = false;
            List<AddressableLoader> addressablesToLoad = new List<AddressableLoader>();
            List<HardPoint> hardPoints = new List<HardPoint>();

            var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var rootGameObject in rootObjects)
            {
                if (rootGameObject.TryGetComponent<BBI.Unity.Game.ModuleDefinition>(out var moduleDefinition))
                {
                    foreach (var addressable in rootGameObject.GetComponentsInChildren<BBI.Unity.Game.AddressableLoader>())
                    {
                        addressablesToLoad.Add(addressable);

                        if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID($"Assets/EditorCache/{addressable.assetGUID ?? addressable.refs[0]}.prefab")))
                        {
                            needToRefreshCache = true;
                        }
                    }

                    foreach (var hardpoint in rootGameObject.GetComponentsInChildren<HardPoint>())
                    {
                        hardPoints.Add(hardpoint);

                        if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID($"Assets/EditorCache/{hardpoint.AssetRef.AssetGUID}.prefab")))
                        {
                            needToRefreshCache = true;
                        }
                    }

                    if (!needToRefreshCache || LoadGameAssets.handle1.IsValid() && LoadGameAssets.handle2.IsValid())
                    {
                        foreach (var addressable in addressablesToLoad)
                        {
                            await LoadAddress(addressable.assetGUID ?? addressable.refs[0], addressable.transform, false, addressable.childPath, addressable.disabledChildren);
                        }

                        foreach (var hardpoint in hardPoints)
                        {
                            var assetGUID = await LoadHardpoint(hardpoint);
                            if (!string.IsNullOrEmpty(assetGUID))
                                await LoadAddress(assetGUID, hardpoint.transform, true);
                        }
                    }
                    else
                    {
                        Debug.Log("Please load the catalogs");
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }



        isUpdating = false;
    }

    async static System.Threading.Tasks.Task<string> LoadHardpoint(HardPoint hardPoint)
    {
        if (System.IO.File.Exists($"{Application.dataPath}/EditorCache/{hardPoint.AssetRef.AssetGUID}.prefab"))
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
            if (!string.IsNullOrEmpty(guid))
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
            else if (moduleEntry.GetType() == typeof(ModuleEntryEmpty))
            {
                return "";
            }
        }

        throw new System.Exception("LoadHardpointGuidFromModuleListAsset");
    }

    async static System.Threading.Tasks.Task<GameObject> LoadAddress(string addressRef, Transform parent, bool isHardpoint, string assetPath = "", List<string> disabledChildren = null)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(addressRef));

        // Only treat it as a hardpoint if it's loading from the game files, for some reason?
        if (prefab)
        {
            isHardpoint = false;
        }
        else
        {
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{addressRef + assetPath.Replace("/", "_")}.prefab");
        }

        if (!prefab)
        {
            var res = Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(addressRef));
            await res.Task;

            if (res.IsValid())
            {
                GameObject result;

                Vector3 cachedPosition;

                if (string.IsNullOrEmpty(assetPath))
                {
                    result = res.Result;
                    cachedPosition = result.transform.localPosition;
                }
                else
                {
                    result = res.Result.transform.Find(assetPath)?.gameObject;

                    if (result == null)
                    {
                        throw new System.Exception($"Can't find {assetPath} in {addressRef}.");
                    }

                    cachedPosition = result.transform.localPosition;
                    result.transform.localPosition = Vector3.zero;
                }

                if (result.TryGetComponent<BBI.Unity.Game.AddressableLoader>(out var loader))
                {
                    await LoadAddress(loader.assetGUID ?? loader.refs[0], res.Result.transform, false, loader.childPath, loader.disabledChildren);
                }

                await TryCacheAsset(addressRef, result, isHardpoint, addressRef + assetPath.Replace("/", "_"));

                result.transform.localPosition = cachedPosition;

                foreach (var hardpoint in result.GetComponentsInChildren<HardPoint>())
                {
                    var assetGUID = await LoadHardpoint(hardpoint);
                    if (!string.IsNullOrEmpty(assetGUID))
                        await LoadAddress(assetGUID, hardpoint.transform, isHardpoint);
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

            foreach (var roomOverlap in temp.GetComponentsInChildren<RoomOpeningDefinition>())
            {
                roomOverlaps.Add(RenderableMapping.RoomMapping(roomOverlap.transform, isHardpoint));
            }

            if (disabledChildren != null)
            {
                foreach (var disabledChild in disabledChildren)
                {
                    GameObject foundChild = null;
                    IEnumerable<Transform> children = temp.transform.GetChild(0).Cast<Transform>();
                    var cList = children.ToList();
                    foreach (var childPathPart in disabledChild.Split('/'))
                    {
                        foundChild = children.Where(c => c.name.StartsWith(childPathPart)).First()?.gameObject;
                        children = foundChild.transform.Cast<Transform>();
                    }
                    if (foundChild != null)
                        GameObject.DestroyImmediate(foundChild);
                }
            }

            temp.AddComponent<FakePrefabDisplay>();
            fakes.Add(temp);

            /*
            foreach (var hardpoint in temp.GetComponentsInChildren<HardPoint>())
            {
                var hardpointPrefab = await LoadAddress(hardpoint.AssetRef.AssetGUID, hardpoint.transform, true, false);
            }
            */

            foreach (var hardpoint in temp.GetComponentsInChildren<FakeHardpoint>())
            {
                var hardpointPrefab = await LoadAddress(hardpoint.AssetGUID, hardpoint.transform, true);
            }
        }

        return prefab;
    }

    static int count = 0;
    async static System.Threading.Tasks.Task TryCacheAsset(string address, GameObject obj, bool isHardpoint, string cachePath)
    {
        count = 0;
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(address)) ?? AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{cachePath}.prefab");

        if (!prefab)
        {
            prefab = new GameObject(address);

            await CloneMeshTree(address, obj.transform, prefab.transform, cachePath);

            PrefabUtility.SaveAsPrefabAsset(prefab, $"Assets/EditorCache/{cachePath}.prefab");
            DestroyImmediate(prefab);
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{cachePath}.prefab");
        }

        if (isHardpoint)
        {
            var hardpointPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{prefabToHardpoint[address]}.prefab");

            if (!hardpointPrefab)
            {
                var tempGO = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                PrefabUtility.SaveAsPrefabAsset(tempGO, $"Assets/EditorCache/{prefabToHardpoint[address]}.prefab");
                DestroyImmediate(tempGO);
            }
        }

        foreach (var hardpoint in prefab.GetComponentsInChildren<FakeHardpoint>())
        {
            await LoadAddress(hardpoint.AssetGUID, hardpoint.transform, isHardpoint);
        }
    }

    private async static System.Threading.Tasks.Task CloneMeshTree(string address, Transform inTransform, Transform outParent, string cachePath)
    {
        var newPrefabChild = new GameObject($"{inTransform.name}_{address}");
        newPrefabChild.transform.parent = outParent;
        newPrefabChild.transform.localPosition = inTransform.localPosition;
        newPrefabChild.transform.localRotation = inTransform.localRotation;
        newPrefabChild.transform.localScale = inTransform.localScale;
        newPrefabChild.AddComponent<SelectAddressableParent>();
        foreach (Transform child in inTransform)
        {
            await CloneMeshTree(address, child, newPrefabChild.transform, cachePath);
        }

        var meshPath = $"Assets/EditorCache/";

        if (inTransform.TryGetComponent<MeshRenderer>(out var meshRenderer))
        {
            var meshFilter = inTransform.GetComponent<MeshFilter>();

            // TODO: Is this stable enough?
            var dataArray = Mesh.AcquireReadOnlyMeshData(meshFilter.sharedMesh);

            var meshHashText = "";
            for(int i = 0; i < dataArray.Length; i++)
            {
                var data = dataArray[0];
                meshHashText += Hash128.Compute(ref data).ToString();
            }
            dataArray.Dispose();

            if(!System.IO.File.Exists($"{Application.dataPath}/EditorCache/{meshHashText}.asset"))
            {
                AssetDatabase.CreateAsset(Instantiate(meshFilter.sharedMesh), $"{meshPath}{meshHashText}.asset");
            }

            if(meshCache.ContainsKey(meshHashText))
            {
                newPrefabChild.AddComponent<MeshFilter>().sharedMesh = meshCache[meshHashText];
            }
            else
            {
                var newMesh = AssetDatabase.LoadAssetAtPath<Mesh>($"{meshPath}{meshHashText}.asset");
                newPrefabChild.AddComponent<MeshFilter>().sharedMesh = newMesh;
                meshCache.Add(meshHashText, newMesh);
            }

            MeshRenderer newRenderer = newPrefabChild.AddComponent<MeshRenderer>();
            newRenderer.sharedMaterials = meshRenderer.sharedMaterials.Select((mat, matIndex) => CloneMaterial(mat)).ToArray();

            count++;
        }

        if (inTransform.TryGetComponent<RoomSubVolumeDefinition>(out var roomVolume))
        {
            var newRoomVolume = newPrefabChild.AddComponent<RoomSubVolumeDefinition>();
            EditorUtility.CopySerialized(roomVolume, newRoomVolume);
        }

        if (inTransform.TryGetComponent<RoomOpeningDefinition>(out var roomOpening))
        {
            var newRoomOpening = newPrefabChild.AddComponent<RoomOpeningDefinition>();
            EditorUtility.CopySerialized(roomOpening, newRoomOpening);
        }

        if (inTransform.TryGetComponent<HardPoint>(out var hardPoint))
        {
            var assetGUID = await LoadHardpoint(hardPoint);
            if (!string.IsNullOrEmpty(assetGUID))
            {
                var newHardpoint = newPrefabChild.AddComponent<FakeHardpoint>();
                newHardpoint.AssetGUID = assetGUID;
            }
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

    static Material CloneMaterial(Material material)
    {
        var matPath = $"EditorCache/{material.ComputeCRC()}.mat";

        if (!System.IO.File.Exists($"{Application.dataPath}/{matPath}"))
        {
            Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
            Dictionary<string, string> validTextures = new Dictionary<string, string>();

            foreach (var textureName in material.GetTexturePropertyNames())
            {
                var orgTexture = (Texture2D)material.GetTexture(textureName);
                if (orgTexture != null)
                {
                    Texture2D newTexture = DuplicateTexture(orgTexture);
                    if (!System.IO.File.Exists($"{Application.dataPath}/EditorCache/{newTexture.imageContentsHash.ToString()}.png"))
                        System.IO.File.WriteAllBytes($"{Application.dataPath}/EditorCache/{newTexture.imageContentsHash.ToString()}.png", newTexture.EncodeToPNG());

                    validTextures.Add(textureName, newTexture.imageContentsHash.ToString());
                }
            }

            AssetDatabase.Refresh();

            foreach (var textureName in validTextures)
            {
                if (!textureCache.ContainsKey(textureName.Key))
                {
                    textureCache.Add(textureName.Key, AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/EditorCache/{textureName.Value}.png"));
                    EditorUtility.SetDirty(textureCache[textureName.Key]);
                }
            }

            Material tempMaterial;
            switch (material.shader.name)
            {
                case "_Lynx/Surface/HDRP/Lit":
                    tempMaterial = new Material(Shader.Find("Fake/_Lynx/Surface/HDRP/Lit"));
                    tempMaterial.CopyPropertiesFromMaterial(material);
                    break;
                default:
                    tempMaterial = new Material(Shader.Find("HDRP/Lit")); // Unknown material, use the default
                    Debug.LogWarning($"Unknown shader {material.shader.name}");
                    break;
            }

            foreach (var textureName in validTextures)
            {
                tempMaterial.SetTexture(textureName.Key, textureCache[textureName.Key]);
            }

            tempMaterial.enableInstancing = true;
            AssetDatabase.CreateAsset(tempMaterial, $"Assets/{matPath}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        return AssetDatabase.LoadAssetAtPath<Material>($"Assets/{matPath}");
    }

    public class RenderableMapping
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
