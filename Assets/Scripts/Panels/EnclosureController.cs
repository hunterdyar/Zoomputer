using System.Collections.Generic;
using Panels;
using UnityEngine;

namespace Zoompy
{
    /// <summary>
    /// Just a wrapper component for the outer system things. Should be accessed through System Display Manager
    /// </summary>
    public class EnclosureController : MonoBehaviour
    {
        public OuterSystemPort LeftPortPrefab;
        public OuterSystemPort RightPortPrefab;

        private Dictionary<ZConnection, OuterSystemPort> _ports = new Dictionary<ZConnection, OuterSystemPort>();
        
        [Header("Config")]
        public float outerPortHeight;
        
        public void SetSystem(ZSystem system, Bounds bounds, ConnectionHub hub)
        {
            _ports.Clear();
            //Create/move the boundry objects.
            
            //Create the outer ports.
            
            for (int i = 0; i < system.inputs.Length; i++)
            {
                var left = Instantiate(LeftPortPrefab, transform);
                float z = Mathf.Lerp(bounds.min.z,bounds.max.z,(i+2f)/(float)(system.inputs.Length+2));
                left.transform.position = new Vector3(bounds.min.x, outerPortHeight,z);
                left.SetConnection(hub, system.inputs[i]);
                _ports.Add(system.inputs[i],left);
            }
            
            for (int i = 0; i < system.outputs.Length; i++)
            {
                var right = Instantiate(RightPortPrefab, transform);
                float z = Mathf.Lerp(bounds.min.z,bounds.max.z,(i+2f)/(float)(system.outputs.Length+2));
                right.transform.position = new Vector3(bounds.max.x, outerPortHeight,z);
                right.SetConnection(hub, system.outputs[i]);
                _ports.Add(system.outputs[i],right);

            }
        }

        public OuterSystemPort GetPort(ZConnection connection)
        {
            if (_ports.TryGetValue(connection, out OuterSystemPort port))
            {
                return port;
            }

            return null;
        }
    }
}