using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zoompy;
using Zoompy.Logic;

public class LayerView : MonoBehaviour
{
    private ComponentSystem _container;
    private bool _enabled;
    public ISignalHook SignalHook => _signalHook;
    private ISignalHook _signalHook;

    private void Awake()
    {
        _signalHook = GetComponentInChildren<ISignalHook>();
    }

    public void Init(ComponentSystem componentSystem)
    {
        _enabled = false;
        _container = componentSystem;
        //pass along the init
        _signalHook?.SetComponenSystem(componentSystem);
        Disable();
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
}
