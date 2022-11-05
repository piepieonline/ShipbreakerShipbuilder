#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CustomStage : PreviewSceneStage
{
    public static GameObject go;

    protected override GUIContent CreateHeaderContent()
    {
        return new GUIContent(go.name);
    }

    protected override bool OnOpenStage()
    {
        base.OnOpenStage();

        if(go == null)
            return false;


        // Add the lights to the preview scene
        var lights = GameObject.Find("Lights");
        if(lights != null)
        {
            var previewLights = Instantiate(lights, Vector3.zero, Quaternion.identity);
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(previewLights, scene);
            previewLights.hideFlags = HideFlags.HideAndDontSave;
        }

        var customStageGoInstance = Instantiate(go, Vector3.zero, Quaternion.identity);

        // Replace materials so they render correctly
        var fakeShader = Shader.Find("Fake/_Lynx/Surface/HDRP/Lit");
        var replacementMaterialCache = new Dictionary<Material, Material>();

        foreach(var renderer in customStageGoInstance.GetComponentsInChildren<MeshRenderer>())
        {
            var mats = (Material[])renderer.sharedMaterials.Clone();
            for(var i = 0; i < mats.Length; i++)
            {
                if(mats[i] == null) continue;

                if(replacementMaterialCache.ContainsKey(mats[i]))
                {
                    mats[i] = replacementMaterialCache[mats[i]];
                }
                else if(mats[i]?.shader?.name == "_Lynx/Surface/HDRP/Lit")
                {
                    var mat = new Material(fakeShader);
                    mat.CopyPropertiesFromMaterial(mats[i]);
                    mats[i] = mat;
                }
            }
            renderer.sharedMaterials = mats;
        }

        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(customStageGoInstance, scene);

        return true;
    }
}

#endif