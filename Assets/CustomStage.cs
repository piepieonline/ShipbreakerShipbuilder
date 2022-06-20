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
        return GUIContent.none;
    }

    protected override bool OnOpenStage()
    {
        base.OnOpenStage();

        // UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);

        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(Instantiate(go, Vector3.zero, Quaternion.identity), scene);

        return true;
    }

    // SceneManager.SetActiveScene
}

#endif