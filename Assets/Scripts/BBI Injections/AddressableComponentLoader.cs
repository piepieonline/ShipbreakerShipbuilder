using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BBI.Unity.Game
{
    public class AddressableComponentLoader : MonoBehaviour, ISerializationCallbackReceiver
    {
        public List<AddressableComponentValue> componentValues = new List<AddressableComponentValue>();

        [SerializeField]
        private List<Component> components = new List<Component>();
        [SerializeField]
        private List<string> fields = new List<string>();
        [SerializeField]
        private List<string> addresses = new List<string>();


        public void OnBeforeSerialize()
        {
            components.Clear();
            fields.Clear();
            addresses.Clear();

            foreach(var componentValue in componentValues)
            {
                components.Add(componentValue.component);
                fields.Add(componentValue.field);
                addresses.Add(componentValue.address);
            }
        }

        public void OnAfterDeserialize()
        {

        }
    }

    [System.Serializable]
    public class AddressableComponentValue
    {
        public Component component;
        public string field;
        public string address;
    }
}