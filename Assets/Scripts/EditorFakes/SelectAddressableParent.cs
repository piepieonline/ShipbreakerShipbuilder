using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAddressableParent : MonoBehaviour
{
    public bool selectParent = false;

    GameObject parent;

#if UNITY_EDITOR
    void OnValidate()
    {
        if(selectParent)
        {
            selectParent = false;
            SelectParent();
        }
    }

    void SelectParent()
    {
        parent = GetComponentsInParent<BBI.Unity.Game.AddressableLoader>().FirstOrDefault()?.gameObject ?? GetComponentsInParent<BBI.Unity.Game.HardPoint>().FirstOrDefault()?.gameObject;

        if (parent == null) return;

        if (!UnityEditor.Selection.gameObjects.Contains(transform.gameObject)) { return; }

        if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null)
            UnityEditor.Selection.objects = new Object[] { parent };
    }

    void OnDrawGizmosSelected()
    {
        SelectParent();
    }
#endif
}
