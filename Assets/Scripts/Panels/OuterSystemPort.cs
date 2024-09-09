using UnityEngine;
using Zoompy;

namespace Panels
{
    public class OuterSystemPort : MonoBehaviour
    {
        private ZConnection _zConnection;
        private ConnectionHub _hub;
        public void SetConnection(ConnectionHub hub, ZConnection zConnection)
        {
            _hub = hub;
            _zConnection = zConnection;
            
            //listen!
        }
        public void SetLabel(string label)
        {
            
        }
    }
}