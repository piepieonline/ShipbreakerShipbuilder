#if UNITY_EDITOR

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[ExecuteInEditMode]
public class DrawEditor : MonoBehaviour
{
    public bool updateView = false;
    public bool clearAddressables = false;

    static List<AddressableMapping> addressables = new List<AddressableMapping>();

    public Material testMat;

    [ExecuteInEditMode]
    void Update()
    {
        var cam = SceneView.currentDrawingSceneView ? SceneView.currentDrawingSceneView.camera : SceneView.lastActiveSceneView.camera;
        Draw(cam);
    }

    private void Draw(Camera camera)
    {
        foreach (var addressable in addressables)
        {
            if(addressable.parent == null) continue;
            // Matrix4x4 matrix = Matrix4x4.TRS(addressable.parent.TransformPoint(addressable.offset) + Vector3.up * 5, addressable.parent.rotation, addressable.parent.localScale);
            // Graphics.DrawMeshInstanced(addressable.mesh, 0, addressable.mat, new Matrix4x4[] { matrix }, 1);

            // Matrix4x4 matrix = Matrix4x4.TRS(addressable.offset, addressable.rotation, addressable.parent.localScale);
            Matrix4x4 parentMatrix = Matrix4x4.TRS(addressable.parent.position, addressable.parent.rotation, Vector3.one);
            Matrix4x4 childMatrix = Matrix4x4.TRS(addressable.offset, addressable.rotation, Vector3.one);
            // Graphics.DrawMesh(addressable.mesh, matrix, addressable.mat, gameObject.layer, camera);

            addressable.mat.enableInstancing = true;
            Graphics.DrawMeshInstanced(addressable.mesh, 0, addressable.mat, new Matrix4x4[] { parentMatrix * childMatrix }, 1);
        }
    }

    void OnValidate()
    {
        if(clearAddressables)
        {
            clearAddressables = false;
            addressables.Clear();
        }

        List<(string, Transform)> addressablesToLoad = new List<(string, Transform)>();

        if(updateView)
        {
            updateView = false;
            UpdateViewList();
        }
    }

    public static void UpdateViewList()
    {
        addressables.Clear();
        bool needToRefreshCache = false;
        List<(string, Transform)> addressablesToLoad = new List<(string, Transform)>();

        foreach (var rootGameObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if(rootGameObject.TryGetComponent<BBI.Unity.Game.ModuleDefinition>(out var moduleDefinition))
            {
                foreach (var addressable in rootGameObject.GetComponentsInChildren<BBI.Unity.Game.AddressableLoader>())
                {
                    foreach (var (addressRef, i) in addressable.refs.Select((addressRef, i) => (addressRef, i)))
                    {
                        addressablesToLoad.Add((addressRef, addressable.transform.GetChild(i)));

                        if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID($"Assets/EditorCache/{addressRef}.prefab")))
                        {
                            needToRefreshCache = true;
                        }
                    }
                }

                if (!needToRefreshCache || LoadAddressables.handle1.IsValid() && LoadAddressables.handle2.IsValid())
                {
                    foreach (var addressable in addressablesToLoad)
                    {
                        LoadAddress(addressable.Item1, addressable.Item2);
                    }
                }
                else
                {
                    Debug.Log("Please load the catalogs");
                }
            }
        }
    }

    public static void LoadAddress(string addressRef, Transform transform)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{addressRef}.prefab");

        if(!prefab)
        {
            Addressables.ResourceManager.CreateChainOperation<GameObject, GameObject>(
                Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(addressRef)),
                GameObjectReady
            ).Completed += res => {
                if(res.IsValid())
                {
                    TryCacheAsset(addressRef, res.Result, transform);
                }
            };
        }
        else
        {
            foreach(var meshFilter in prefab.GetComponentsInChildren<MeshFilter>())
            {
                addressables.Add(new AddressableMapping(addressRef, -1, meshFilter.sharedMesh, meshFilter.GetComponent<MeshRenderer>().sharedMaterial, transform, meshFilter.transform));
            }
            
        }
    }

    static int count = 0;
    static void TryCacheAsset(string address, GameObject obj, Transform parent)
    {
        count = 0;
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{address}.prefab");
        if (!prefab)
        {
            prefab = new GameObject(address);

            CloneMeshTree(address, obj.transform, prefab.transform);

            PrefabUtility.SaveAsPrefabAsset(prefab, $"Assets/EditorCache/{address}.prefab");
            DestroyImmediate(prefab);
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/EditorCache/{address}.prefab");
        }

        foreach (var meshFilter in prefab.GetComponentsInChildren<MeshFilter>())
        {
            addressables.Add(new AddressableMapping(address, count, meshFilter.sharedMesh, meshFilter.GetComponent<MeshRenderer>().sharedMaterial, parent, meshFilter.transform));
            count++;
        }
    }

    private static void CloneMeshTree(string address, Transform inTransform, Transform outParent)
    {
        var newPrefabChild = new GameObject($"{address}_{count}");
        newPrefabChild.transform.parent = outParent;
        newPrefabChild.transform.localPosition = inTransform.localPosition;
        newPrefabChild.transform.localRotation = inTransform.localRotation;
        foreach (Transform child in inTransform)
        {
            CloneMeshTree(address, child, newPrefabChild.transform);
        }

        var meshPath = $"Assets/EditorCache/{address}_mesh";
        var matPath = $"Assets/EditorCache/{address}_mat";
        var shaderPath = $"Assets/EditorCache/{address}_sha";
        var texturePath = $"/EditorCache/{address}_tex";

        if (inTransform.TryGetComponent<MeshRenderer>(out var meshRenderer))
        {
            var meshFilter = inTransform.GetComponent<MeshFilter>();
            var suffix = $"_{count}";

            var tempMesh = Instantiate(meshFilter.sharedMesh);

            var tempTexture = DuplicateTexture((Texture2D)meshRenderer.sharedMaterial.GetTexture("_BaseColorMap"));
            // var tempShader = Instantiate(meshFilter.GetComponent<MeshRenderer>().sharedMaterial.shader);
            // var tempMaterial = Instantiate(meshFilter.GetComponent<MeshRenderer>().sharedMaterial);
            var tempMaterial = new Material(Shader.Find("HDRP/Lit"));
            System.IO.File.WriteAllBytes($"{Application.dataPath}{texturePath}{suffix}.png", tempTexture.EncodeToPNG());
            AssetDatabase.Refresh();
            // tempMaterial.shader = testMat.shader;
            var tempTextureNew = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/{texturePath}{suffix}.png");
            EditorUtility.SetDirty(tempTextureNew);

            tempMaterial.SetTexture("_BaseColorMap", tempTextureNew);
            // EditorUtility.SetDirty(tempMaterial);

            // AssetDatabase.CreateAsset(tempShader, $"{shaderPath}{suffix}.shader");
            AssetDatabase.CreateAsset(tempMaterial, $"{matPath}{suffix}.mat");
            AssetDatabase.CreateAsset(tempMesh, $"{meshPath}{suffix}.asset");
            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>($"{meshPath}{suffix}.asset");
            var mat = AssetDatabase.LoadAssetAtPath<Material>($"{matPath}{suffix}.mat");
            mat.SetTexture("_BaseColorMap", tempTextureNew);
            mat.enableInstancing = true;
            EditorUtility.SetDirty(mat);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            newPrefabChild.AddComponent<MeshFilter>().sharedMesh = mesh;
            newPrefabChild.AddComponent<MeshRenderer>().sharedMaterial = mat;

            count++;
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

    static AsyncOperationHandle<GameObject> GameObjectReady(AsyncOperationHandle<GameObject> arg)
    {
        if(arg.Result.TryGetComponent<BBI.Unity.Game.AddressableLoader>(out var loader))
        {
            for(int i = 0; i < loader.refs.Count; i++)
            {
                LoadAddress(loader.refs[i], arg.Result.transform);
            }
        }

        return Addressables.ResourceManager.CreateCompletedOperation(arg.Result, string.Empty);
    }


    class AddressableMapping
    {
        public string hash;
        public int index;
        public Mesh mesh;
        public Material mat;
        public Transform parent;
        public Vector3 offset;
        public Quaternion rotation;

        public AddressableMapping(string _hash, int _index, Mesh _mesh, Material _mat, Transform _parent, Transform _offset)
        {
            hash = _hash;
            index = _index;
            mesh = _mesh;
            mat = _mat;
            parent = _parent;
            offset = _offset.position;
            rotation = _offset.rotation;
        }
    }
}

#endif