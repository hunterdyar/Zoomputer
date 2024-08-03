using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zoompy.LogicImplementations;

namespace Zoompy
{
    /// <summary>
    /// A component is made up of one or more children components, systems, and views.
    /// It takes some number of inputs, and some number of outputs.
    /// </summary>
    public class ComponentSystem : MonoBehaviour
    {
        public string DisplayName => _name;
        [SerializeField] private string _name;
        
        public bool IsLeaf { get; private set; }

        private bool _viewingInside = false;
        public ISignalHook BaseLogic => _baseLogic;
        private ISignalHook _baseLogic;
        
        public SignalPort[] Inputs => _inputs;

        [Header("Connections")]
        [SerializeField] private SignalPort[] _inputs;

        public SignalPort[] Outputs => _outputs;

        [SerializeField] private SignalPort[] _outputs;
        
        private int _currentView = -1;

        public LayerView outsideView;
        public LayerView insideView;
        
        private void Awake()
        {
            _baseLogic = GetComponent<ISignalHook>();
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
    }
}