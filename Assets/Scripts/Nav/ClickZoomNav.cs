using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;
using Zoompy;
using Zoompy.Nav;

public class ClickZoomNav : MonoBehaviour
{
    [SerializeField] private LayerMask _layerViewLayer;
    private RaycastHit _hitInfo;
    private Stack<ComponentSystem> _breadcrumbs = new Stack<ComponentSystem>();
    
    //HUD
    [SerializeField] private UIDocument _currentSystemDoc;
    private Label _currentLabel;
    private void Start()
    {
        _currentLabel = _currentSystemDoc.rootVisualElement.Q<Label>("current");
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (DidClickOnLayer(out var view))
            {
                ZoomInto(view.ComponentSystem);
            }
        }else if (Input.GetMouseButtonDown(1))
        {
            if (DidClickOnLayer(out var view))
            {
                //check if has interactor, give feedback light highlight.
                view.ComponentSystem.Interact();
            }
        }else
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                ZoomOut();
            }
        }
        
    }

    private void ZoomInto(ComponentSystem system)
    {
        //check if this is a valid (nested) thing to zoom into.
        if (!system.IsLeaf)
        {
            _breadcrumbs.Push(system);
            system.EnterSystem();
            CameraControl.FrameBoundingBox(system.insideView.Bounds);
            RefreshUI();
        }
        else
        {
            //Can't enter leaf node (lowest layer)
        }
        //move the camera to this position.
    }

    private void ZoomOut()
    {
        if (_breadcrumbs.Count > 0)
        {
            var container = _breadcrumbs.Pop();
            container.ExitSystem();
            RefreshUI();
            //peek the new top and move the camera to that.
            if (_breadcrumbs.Count > 0)
            {
                var top = _breadcrumbs.Peek();
                CameraControl.FrameBoundingBox(top.insideView.Bounds);
                RefreshUI();
            }
        }
  

        CameraControl.MoveToStartPosition();
    }

    private void RefreshUI()
    {
        if (_breadcrumbs.Count == 0)
        {
            _currentLabel.text = "Full System";
        }
        else
        {
            _currentLabel.text = _breadcrumbs.Peek().DisplayName;
        }
    }


    bool DidClickOnLayer(out LayerView view)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out _hitInfo, Mathf.Infinity, _layerViewLayer))
        {
            view = _hitInfo.collider.GetComponent<LayerView>();
            return view != null;
        }

        view = null;
        return false;
    }
}
