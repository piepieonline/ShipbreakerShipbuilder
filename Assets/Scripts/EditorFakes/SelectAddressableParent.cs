using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAddressableParent : MonoBehaviour
{
    GameObject parent;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        parent = GetComponentInParent<BBI.Unity.Game.AddressableLoader>()?.gameObject ?? GetComponentInParent<FakeHardpoint>()?.gameObject;

        if (parent == null) return;

        if (!UnityEditor.Selection.gameObjects.Contains(transform.gameObject)) { return; }

        if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null)
            UnityEditor.Selection.objects = new Object[] { parent };
    }
#endif
}
