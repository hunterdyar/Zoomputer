using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zoompy;

public class LayerView : MonoBehaviour
{
    public ComponentSystem ComponentSystem => _container;
    private ComponentSystem _container;
    public bool IsEnabled => _enabled;
    private bool _enabled;
    public Bounds Bounds => _bounds; 
    private Bounds _bounds;

    public ComponentSystem[] Nodes;
    //Setup gets called by the ComponentSystem once on all layers during init.
    public void Setup(ComponentSystem componentSystem)
    {
        _enabled = false;
        _container = componentSystem;
        //pass along the init
      
        
        //create bounding box.
        _bounds = new Bounds();
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            _bounds.Encapsulate(mr.bounds);
        }
    }

    void Enable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        _enabled = true;
    }

    void Disable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        _enabled = false;
    }

    public void SetLayerEnabled(bool b)
    {
        if (b && !_enabled)
        {
            Enable();
        }
        else if(!b && _enabled)
        {
            Disable();
        }
    }

    private void Update()
    {
        //basically i want to do frustum culling - type math to check the bounds against the camera to decide if we should switch our view automatically or not.
        //this will just be for flyaround, but I would want to consider non-automatic versions of this for VR.
        //https://gamedev.stackexchange.com/questions/77579/determine-percentage-of-screen-covered-by-an-object-without-using-frustum-cullin
        //https://bruop.github.io/frustum_culling/
    }
}
