using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            outsideView.Setup(this);
            IsLeaf = insideView == null;
            insideView?.Setup(this);

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

            if (outsideView.IsEnabled)
            {
                var sh = outsideView.SignalHook;
                if (sh != null)
                {
                    sh.OnInputChange(index, data);
                }
            }

            if (insideView.IsEnabled)
            {
                var sh = insideView.SignalHook;
                if (sh != null)
                {
                    sh.OnInputChange(index, data);
                }
            }
        }
    }

}