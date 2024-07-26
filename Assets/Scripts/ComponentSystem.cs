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
        public LayerView ActiveLayer => children[_currentView];

        [SerializeField] private SignalPort[] _outputs;
        
        private int _currentView = -1;
        [Header("System")] [SerializeField] private LayerView[] children;

        private void Awake()
        {
            //init
            foreach (var layer in children)
            {
                layer.Setup(this);
            }

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

        public void ZoomIn()
        {
            var zoom = _currentView+1;
            if (zoom < children.Length)
            {
                SetView(zoom);
            }
        }

        public void ZoomOut()
        {
            var zoom = _currentView - 1;
            if (zoom >=0)
            {
                SetView(zoom);
            }
        }
        private void SetView(int viewIndex)
        {
            if (_currentView == viewIndex)
            {
                return;
            }
            _currentView = viewIndex;
            if (viewIndex >= 0 && viewIndex < children.Length)
            {
                for (var i = 0; i < children.Length; i++)
                {
                    children[i].SetLayerEnabled(viewIndex == i);
                }
            }

            foreach (var input in _inputs)
            {
                input.Refresh();
            }
        }

        public void OnInputChange(int index, byte data)
        {
            if (_currentView == -1)
            {
                return;
            }
            //what's a good way to get an index?
            //indexof is.. odd?
            if (_currentView < 0 || _currentView >= children.Length)
            {
                Debug.LogError($"Bad Current View: {_currentView}. There are {children.Length} layers.",this);
                return;
            }
            var sh = children[_currentView].SignalHook;
            if (sh != null)
            {
                sh.OnInputChange(index,data);
            }
        }

        void Refresh()
        {
            
        }
    }

}