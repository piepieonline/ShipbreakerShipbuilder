#if UNITY_EDITOR

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using BBI.Unity.Game;

public class DebugLoadAddressableSO : MonoBehaviour
{
    public bool loadAddressableSOs = false;

    Dictionary<FieldInfo, Component> fieldToIndex = new Dictionary<FieldInfo, Component>();

    void OnValidate()
    {
        if (loadAddressableSOs)
        {
            loadAddressableSOs = false;

            foreach (var addressable in FindObjectsOfType<BBI.Unity.Game.AddressableSOLoader>())
            {
                for (int i = 0; i < addressable.refs.Count; i++)
                {
                    var comp = addressable.GetComponents<Component>().Where(comp => comp.GetType().ToString() == addressable.comp[i]).First();
                    FieldInfo fi = comp.GetType().GetField(addressable.field[i], BindingFlags.NonPublic | BindingFlags.Instance);
                    fieldToIndex[fi] = comp;

                    Addressables.LoadAssetAsync<ScriptableObject>(new AssetReferenceScriptableObject(addressable.refs[i])).Completed += res => {
                        if (res.IsValid())
                        {
                            fi.SetValue(fieldToIndex[fi], res.Result);
                        }
                        fieldToIndex.Remove(fi);
                    };
            }
            }
        }
    }
}

#endif