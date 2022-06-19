using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatTestScript : MonoBehaviour
{
    public bool swap = false;

    void OnValidate()
    {
        if(swap)
        {
            swap = false;

            GetComponent<MeshRenderer>().sharedMaterial.shader = Shader.Find("Shader Graphs/Custom");
        }
    }
}
