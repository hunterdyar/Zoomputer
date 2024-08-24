using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using Zoompy.Interactors;

namespace Zoompy
{
    /// <summary>
    /// A component is made up of one or more children components, systems, and views.
    /// It takes some number of inputs, and some number of outputs.
    /// </summary>
    public class ComponentSystem : MonoBehaviour
    {
        public string Guid => _guid;
        private string _guid;
        public string DisplayName => _name;
        [SerializeField] private string _name;
        
        public bool IsLeaf { get; private set; }

        private bool _viewingInside = false;
        public ISignalHook BaseLogic => _baseLogic;
        private ISignalHook _baseLogic;

        private IComponentInteractor[] _interactors;
        public string inputNodeID;
        public SignalPort[] Inputs => _inputs;

        [Header("Connections")]
        [SerializeField] private SignalPort[] _inputs;

        public string outputNodeID;
        public SignalPort[] Outputs => _outputs;

        [SerializeField] private SignalPort[] _outputs;
        
        private int _currentView = -1;

        public LayerView outsideView;
        public LayerView insideView;
        
        private void Awake()
        {
            _baseLogic = GetComponent<ISignalHook>();
            _interactors = GetComponents<IComponentInteractor>();
            _baseLogic?.SetComponenSystem(this);
            
            outsideView.Setup(this);
            IsLeaf = insideView == null;
            insideView?.Setup(this);

            _viewingInside = false;
            foreach (var input in _inputs)
            {
                input.Setup();
            }

            foreach (var output in _outputs)
            {
                output.Setup();
            }
        }

        void Start()
        {
            foreach (var interactor in _interactors)
            {
                interactor.Configure(this);
            }
            
            for (var i = 0; i < _inputs.Length; i++)
            {
                var input = _inputs[i];
                input.ConnectedTo = this;
                input.ConnectedIndex = i;
            }
              for (var i = 0; i < _outputs.Length; i++)
            {
                var input = _outputs[i];
                input.ConnectedTo = this;
                input.ConnectedIndex = i;
            }
            outsideView.SetLayerEnabled(true);
            if (!IsLeaf)
            {
                insideView.SetLayerEnabled(false);
            }
        }

        public void Interact()
        {
            foreach (var interactor in _interactors)
            {
                interactor.Interact();
            }
        }
        private void OnEnable()
        {
            foreach (var input in _inputs)
            {
                input.OnInputChange += OnInputChange;
            }
        }

        private void OnDisable()
        {
            foreach (var input in _inputs)
            {
                input.OnInputChange -= OnInputChange;
            }
        }

        public void SetDisplayName(string newName)
        {
            _name = newName;
        }

        public void SetPorts(SignalPort[] ports, PortType portType)
        {
            switch (portType)
            {
                case PortType.Input:
                    _inputs = ports;
                    break;
                case PortType.Output:
                    _outputs = ports;
                    break;
            }
        }

        public void EnterSystem()
        {
            if (!IsLeaf)
            {
                _viewingInside = true;
                outsideView.SetLayerEnabled(false);
                insideView.SetLayerEnabled(true);
                foreach (var input in _inputs)
                {
                    input.Refresh();
                }
            }
        }

        public void ExitSystem()
        {
            if (!IsLeaf)
            {
                _viewingInside = false;
                outsideView.SetLayerEnabled(true);
                insideView.SetLayerEnabled(false);
                foreach (var input in _inputs)
                {
                    input.Refresh();
                }
            }
        }

        public void OnInputChange(int index, byte data)
        {
            if (_currentView == -1)
            {
                return;
            }

            if (!_viewingInside || IsLeaf)
            {
                 _baseLogic.OnInputChange(index, data);
            }
        }

        public void SetIsLeaf(bool isLeaf)
        {
            IsLeaf = isLeaf;
        }

        public void SetGuid(string guid)
        {
            _guid = guid;
        }

        public bool TryGetInnerPort(string nodeID, PortType portType, int portIndex, out SignalPort signalPort)
        {
            //connect to input from the inside
            if (nodeID == inputNodeID)
            {
                if (portIndex >= 0 && portIndex < _inputs.Length)
                {
                    signalPort = _inputs[portIndex];
                    return true;
                }

                signalPort = null;
                return false;
            }
            //connect to output from the inside
            if (nodeID == outputNodeID)
            {
                if (portIndex >= 0 && portIndex < _outputs.Length)
                {
                    signalPort = _outputs[portIndex];
                    return true;
                }

                signalPort = null;
                return false;
            }
            //connect to node from the outside
            var n = insideView.Nodes.First(x => x._guid == nodeID);
            if (n != null)
            {
                return n.TryGetPort(portType, portIndex, out signalPort);
            }

            signalPort = null;
            return false;
        }

        public bool TryGetPort(PortType portType, int portIndex, out SignalPort signalPort)
        {
            if (portType == PortType.Input)
            {
                if (portIndex >= 0 && portIndex < _inputs.Length)
                {
                    signalPort = _inputs[portIndex];
                    return true;
                }
            }else if (portType == PortType.Output)
            {
                if (portIndex >= 0 && portIndex < _outputs.Length)
                {
                    signalPort = _outputs[portIndex];
                    return true;
                }
            }

            signalPort = null;
            return false;
        }
    }
}