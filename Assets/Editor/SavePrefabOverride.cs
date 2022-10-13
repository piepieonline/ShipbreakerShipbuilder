using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;
using BBI.Unity.Game;

public class SavePrefabOverride : AssetModificationProcessor
{
    static string[] OnWillSaveAssets(string[] paths)
    {
        foreach (string path in paths)
        {
            try
            {
                var sceneObjects = SceneManager.GetSceneByPath(path).GetRootGameObjects();
                foreach(var obj in sceneObjects)
                {
                    if(obj.TryGetComponent(out ModuleDefinition module))
                    {
                        PrefabUtility.ApplyPrefabInstance(obj, InteractionMode.UserAction);
                        // Debug.Log(obj.name);
                    }
                }
            }
            catch(System.ArgumentException _)
            {
                // Do nothing, it triggered on the prefab save above
            }
        }
        return paths;
    }
}