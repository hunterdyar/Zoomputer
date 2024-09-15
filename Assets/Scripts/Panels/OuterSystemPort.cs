using System;
using UnityEngine;
using Zoompy;

namespace Panels
{
    public class OuterSystemPort : MonoBehaviour
    {
        public Material OffMaterial;
        public Material OnMaterial;

        private MeshRenderer _meshRenderer;
        private ZConnection _zConnection;
        private ConnectionHub _hub;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetConnection(ConnectionHub hub, ZConnection zConnection)
        {
            _hub = hub;
            _zConnection = zConnection;
         //  _zConnection.OnDidChangedAfterImpulse += OnConnectionChanged; 
            //listen!
            
        }
        
        void OnConnectionChanged(byte data)
        {
            if (data > 0)
            {
                _meshRenderer.material = OnMaterial;
            }
            else
            {
                _meshRenderer.material = OffMaterial;
            }
        }
        
        public void SetLabel(string label)
        {
            
        }
    }
}