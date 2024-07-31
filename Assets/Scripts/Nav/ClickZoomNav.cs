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
                ZoomTo(view);
            }
        }
        else
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                ZoomOut();
            }
        }
        
    }

    private void ZoomTo(LayerView view)
    {
        //check if this is a valid (nested) thing to zoom into.
        _breadcrumbs.Push(view.ComponentSystem);
        view.ComponentSystem.ZoomIn();
        CameraControl.FrameBoundingBox(view.ComponentSystem.ActiveLayer.Bounds);
        RefreshUI();
        //move the camera to this position.
    }

    private void ZoomOut()
    {
        if (_breadcrumbs.Count > 0)
        {
            var container = _breadcrumbs.Pop();
            container.ZoomOut();
            RefreshUI();
            //peek the new top and move the camera to that.
        }
        else
        {
            //ehhhh its fine for now. 
            CameraControl.MoveToStartPosition();
        }
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
