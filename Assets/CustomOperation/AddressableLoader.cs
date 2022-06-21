using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBI.Unity.Game
{
    public class AddressableLoader : MonoBehaviour
    {
        [HideInInspector]
        [System.Obsolete("Please use assetGUID instead.")]
        public List<string> refs;

        public string assetGUID = "";
        public string childPath = "";

        void OnValidate()
        {
            if (refs != null)
            {
                assetGUID = refs[0];
            }
        }
    }
}
