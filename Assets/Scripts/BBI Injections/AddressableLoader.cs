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

        [HideInInspector]
        public bool enableChildHardpoints = false;

        public List<string> disabledChildren = new List<string>();

        // TODO: Not working right now
        [HideInInspector]
        public List<Component> componentsOnChildren = new List<Component>();
        [HideInInspector]
        public List<string> componentsOnChildrenPaths = new List<string>();

        void OnValidate()
        {
            if (refs != null && refs.Count > 0 && assetGUID == "")
            {
                assetGUID = refs[0];
                refs.Clear();
            }
        }
    }
}
