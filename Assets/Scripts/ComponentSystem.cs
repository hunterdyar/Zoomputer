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
        public SignalPort[] Inputs => _inputs;

        [Header("Connections")]
        [SerializeField] private SignalPort[] _inputs;

        public SignalPort[] Outputs => _outputs;
        [SerializeField] private SignalPort[] _outputs;
        
        private int _currentView = 0;
        [Header("System")] [SerializeField] private LayerView[] children;

        void Start()
        {
            //init
            foreach (var layer in children)
            {
                layer.Init(this);
            }

            for (var i = 0; i < _inputs.Length; i++)
            {
                var input = _inputs[i];
                input.ConnectedTo = this;
                input.ConnectedIndex = i;
            }

            SetView(0);
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

        private void SetView(int viewIndex)
        {
            _currentView = viewIndex;
            if (viewIndex >= 0 && viewIndex < children.Length)
            {
                for (var i = 0; i < children.Length; i++)
                {
                    children[i].SetLayerEnabled(viewIndex == i);
                }
            }
        }

        public void OnInputChange(int index, byte data)
        {
            //what's a good way to get an index?
            //indexof is.. odd?
            var sh = children[_currentView].SignalHook;
            if (sh != null)
            {
                sh.OnInputChange(index,data);
            }
        }
        
        void OnValidate()
        {
            if (children == null)
            {
                children = GetComponentsInChildren<LayerView>();
            }
        }
    }

}