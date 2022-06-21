using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using BBI.Unity.Game;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Clone_Asset : MonoBehaviour
{
    /*
    [MenuItem("Example/Clone asset")]
    static void BuildABs()
    {
        if(LoadAddressables.loadedBundles.Count() == 0)
        {
            foreach(var bundle in LoadAddressables.bundles)
            {
                LoadAddressables.loadedBundles[bundle] = AssetBundle.LoadFromFile("D:\\Games\\Xbox\\Hardspace- Shipbreaker\\Content\\Shipbreaker_Data\\StreamingAssets\\aa\\StandaloneWindows64\\" + bundle);
            }
        }

        foreach(var bundle in LoadAddressables.loadedBundles)
        {
            // bundle.Value.LoadAllAssets();
        }

        var loaded = LoadAddressables.loadedBundles["props-kitbash_assets_all_8fac8ebd27bc05c9cecbcaf042ce9cc1.bundle"].LoadAssetWithSubAssets<GameObject>("Assets/Content/Prefabs/Objects/Storage/PRF_Crate_Hard.prefab");

        Debug.Log(string.Join("\r\n", loaded.Count()));

        //var loaded = LoadAddressables.loadedBundles["props-kitbash_assets_all_8fac8ebd27bc05c9cecbcaf042ce9cc1.bundle"].LoadAllAssets<UnityEngine.Object>()[0];
        // GameObject.Instantiate(loaded[0]);


        var newObj = dupChild(loaded[0].transform, null);
        PrefabUtility.SaveAsPrefabAsset(newObj, "Assets/Exported/PRF_Crate_Hard.prefab", out bool success);
        /*
        foreach(var comp in loaded[0].GetComponentsInChildren<UnityEngine.Object>(true))
        {
            var compCopy = Instantiate(comp);
        }
        *



        // UnityEditor.EditorUtility.CopySerialized(loaded, newObj);
        //var newInst = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(loaded);
        //PrefabUtility.SaveAsPrefabAsset(newObj, "Assets/Exported/PRF_Crate_Hard.prefab", out bool success);
        //UnityEditor.AssetDatabase.CreateAsset(loaded, "Assets/Exported/texture.asset");
        //AssetDatabase.SaveAssets();

        foreach(var bundle in LoadAddressables.loadedBundles)
        {
            // loadedObjects.AddRange(bundle.Value.LoadAllAssets<UnityEngine.Object>());
            // loadedObjects.AddRange(bundle.Value.LoadAllAssets<GameObject>());
            // bundle.Value.Unload(false);
        }
    }
*/

    static int i = 0;

    static GameObject dupChild(Transform transform, Transform parent)
    {
        var obj = new GameObject();

        foreach(Transform child in transform)
        {
            dupChild(child, obj.transform);
        }

        string meshPath = "";

        foreach(var comp in transform.GetComponents<Component>())
        {
            if(new System.Type[] { typeof(Transform) }.Contains(comp.GetType())) continue;

            Component newComp = obj.AddComponent(comp.GetType());
            EditorUtility.CopySerialized(comp, newComp);

            var assetPath = $"Assets/Exported/{comp.name}_{comp.GetType()}_{i}.asset";

            switch(comp.GetType().ToString())
            {
                case "UnityEngine.MeshFilter":
                    {
                        if(meshPath == "")
                        {
                            AssetDatabase.CreateAsset(Instantiate(((MeshFilter)comp).sharedMesh), assetPath);
                            meshPath = assetPath;
                        }
                        ((MeshFilter)newComp).sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
                        break;
                    }
                case "UnityEngine.MeshCollider":
                    {
                        if(meshPath == "")
                        {
                            AssetDatabase.CreateAsset(Instantiate(((MeshCollider)comp).sharedMesh), assetPath);
                            meshPath = assetPath;
                        }
                        ((MeshCollider)newComp).sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
                        break;
                    }
                case "UnityEngine.MeshRenderer":
                    {
                        AssetDatabase.CreateAsset(Instantiate(((MeshRenderer)comp).sharedMaterial), assetPath);
                        ((MeshRenderer)newComp).sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                        break;
                    }
                case "UnityEngine.ScriptableObject":
                {
                    AssetDatabase.CreateAsset(Instantiate(comp), assetPath);
                    EditorUtility.CopySerialized(AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath), newComp);
                    break;
                }
                default:
                {
                    if (comp.GetType().IsSubclassOf(typeof(UnityEngine.Object)) && ((UnityEngine.Object)comp) != null)
                    {
                        EditorUtility.CopySerialized((UnityEngine.Object)CloneScriptable(comp, transform), newComp);
                    }
                    break;
                }
            }

            i++;
        }

        obj.transform.parent = parent;
        return obj;
    }

    static object CloneScriptable(object objSource, Transform parent)
    {
        //step : 1 Get the type of source object and create a new instance of that type
        Type typeSource = objSource.GetType();
        object objTarget;

        if (typeSource.IsArray || typeSource.IsGenericType && (typeSource.GetGenericTypeDefinition() == typeof(List<>)))
        {
            objTarget = Activator.CreateInstance(typeSource, new object[] { ((ICollection)objSource).Count });
        }
        else if (typeSource.IsSubclassOf(typeof(ScriptableObject)))
        {
            objTarget = ScriptableObject.CreateInstance(typeSource);
            EditorUtility.CopySerialized((ScriptableObject)objSource, (ScriptableObject)objTarget);
        }
        else if (typeSource.IsSubclassOf(typeof(MonoBehaviour)))
        {
            objTarget = parent.gameObject.GetComponent(typeSource);
        }
        else if (typeSource.GetConstructor(Type.EmptyTypes) == null)
        {
            objTarget = objSource;
        }
        else
        {
            objTarget = Activator.CreateInstance(typeSource);
        }

        /*
        if(objTarget == null || (typeSource.IsSubclassOf(typeof(UnityEngine.Object)) && ((UnityEngine.Object)objTarget) == null))
        {
            return objSource;
        }
        */

        //Step2 : Get all the properties of source object type
        FieldInfo[] propertyInfo = typeSource.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //Step : 3 Assign all source property to target object 's properties
        foreach (FieldInfo property in propertyInfo)
        {
            //Check whether property can be written to
            // if (property.CanWrite)
            if(
                (Attribute.IsDefined(property, typeof(UnityEngine.SerializeField)) && property.FieldType.IsSubclassOf(typeof(ScriptableObject))) ||
                Attribute.IsDefined(property.GetType(), typeof(System.SerializableAttribute))
            )
            // if(comp.GetType().IsSubclassOf(typeof(ScriptableObject))
            {
                //Step : 4 check whether property type is value type, enum or string type
                if (property.FieldType.IsValueType || property.FieldType.IsEnum || property.FieldType.Equals(typeof(System.String)))
                {
                    property.SetValue(objTarget, property.GetValue(objSource));
                }
                //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                else
                {
                    object objPropertyValue = property.GetValue(objSource);
                    if (objPropertyValue == null)
                    {
                        property.SetValue(objTarget, objPropertyValue);
                    }
                    else
                    {
                        if(property.FieldType.IsSubclassOf(typeof(UnityEngine.Object)) && property.FieldType != typeof(MonoBehaviour) && !property.FieldType.IsSubclassOf(typeof(ScriptableObject)))
                        {
                            // Unity fake null
                            if((UnityEngine.Object)objPropertyValue != null)
                            {
                                var assetPath = $"Assets/Exported/{property.Name}_{i}.asset";
                                // AssetDatabase.CreateAsset(Instantiate((UnityEngine.Object)objPropertyValue), assetPath);
                                // property.SetValue(objTarget, AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath));
                            }
                            property.SetValue(objTarget, property.GetValue(objSource));
                        }
                        else
                        {
                            Debug.Log(property.Name);
                            object obj = CloneScriptable(objPropertyValue, parent);
                             if (property.FieldType.IsSubclassOf(typeof(ScriptableObject)) && obj != null && (UnityEngine.Object)obj != null)
                            {
                                var so = (ScriptableObject)obj;
                                var assetPath = $"Assets/Exported/{property.Name}_{i}.asset";
               
                                AssetDatabase.CreateAsset(so, assetPath);
                                var newObj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                                if (newObj == null) throw new Exception();

                                obj = newObj;
                            }
                            property.SetValue(objTarget, obj);
                        }
                    }
                }
            }
        }
        return objTarget;
    }

    static object CloneScriptable_OLD(object objSource)
    {
        //step : 1 Get the type of source object and create a new instance of that type
        Type typeSource = objSource.GetType();
        object objTarget;

        if (typeSource.IsArray || typeSource.IsGenericType && (typeSource.GetGenericTypeDefinition() == typeof(List<>)))
        {
            objTarget = Activator.CreateInstance(typeSource, new object[] { 0 });
        }
        else if (typeSource.GetConstructor(Type.EmptyTypes) == null)
        {
            objTarget = objSource;
        }
        else
        {
            objTarget = Activator.CreateInstance(typeSource);
        }

        //Step2 : Get all the properties of source object type
        FieldInfo[] propertyInfo = typeSource.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //Step : 3 Assign all source property to target object 's properties
        foreach (FieldInfo property in propertyInfo)
        {
            //Check whether property can be written to
            // if (property.CanWrite)
            if (
                (Attribute.IsDefined(property, typeof(UnityEngine.SerializeField)) && property.FieldType.IsSubclassOf(typeof(ScriptableObject))) ||
                Attribute.IsDefined(property.GetType(), typeof(System.SerializableAttribute))
            )
            // if(comp.GetType().IsSubclassOf(typeof(ScriptableObject))
            {
                //Step : 4 check whether property type is value type, enum or string type
                if (property.FieldType.IsValueType || property.FieldType.IsEnum || property.FieldType.Equals(typeof(System.String)))
                {
                    property.SetValue(objTarget, property.GetValue(objSource));
                }
                //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                else
                {
                    object objPropertyValue = property.GetValue(objSource);
                    if (objPropertyValue == null)
                    {
                        property.SetValue(objTarget, null);
                    }
                    else
                    {
                        if (property.FieldType.IsSubclassOf(typeof(UnityEngine.Object)) && property.FieldType != typeof(MonoBehaviour) && !property.FieldType.IsSubclassOf(typeof(ScriptableObject)))
                        {
                            // Unity fake null
                            if ((UnityEngine.Object)objPropertyValue != null)
                            {
                                var assetPath = $"Assets/Exported/{property.Name}_{i}.asset";
                                // AssetDatabase.CreateAsset(Instantiate((UnityEngine.Object)objPropertyValue), assetPath);
                                // property.SetValue(objTarget, AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath));
                                property.SetValue(objTarget, property.GetValue(objSource));
                            }
                        }
                        else
                        {
                            object obj = CloneScriptable_OLD(objPropertyValue);
                            if (property.FieldType.IsSubclassOf(typeof(ScriptableObject)) && obj != null && (UnityEngine.Object)obj != null)
                            {
                                var so = (ScriptableObject)obj;
                                var assetPath = $"Assets/Exported/{property.Name}_{i}.asset";
                                try
                                {
                                    AssetDatabase.CreateAsset((ScriptableObject)CloneScriptable_OLD(so), assetPath);
                                    var newObj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                                    if (newObj == null) throw new Exception();

                                    obj = newObj;
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogError($"Failed on {property.Name}");
                                }
                            }
                            property.SetValue(objTarget, obj);
                        }
                    }
                }
            }
        }
        return objTarget;
    }
}