using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableCube : MonoBehaviour
{
    public bool createRef;

    public AssetReference reference;

    void OnValidate()
    {
        if(createRef)
        {
            reference = new AssetReference("3e84870b4ad837d458773aeaae90a447");
            createRef = false;
        }
    }
}
