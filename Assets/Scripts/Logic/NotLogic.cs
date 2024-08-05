using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zoompy;

[Logic]
public class NotLogic : MonoBehaviour, ISignalHook
{
    private ComponentSystem _parent;

    //todo: validate input counts
    
    public void SetComponenSystem(ComponentSystem parent)
    {
        this._parent = parent;
    }

    public void OnAnyInputChange()
    {
        //loop through all connections and match the same output
        _parent.Outputs[0].SetSignal(_parent.Inputs[0].GetSignal()==false);
    }

    public void OnInputChange(int index, byte data)
    {
        if (_parent != null)
        {
            _parent.Outputs[index].SetSignal(data == 0);
        }
    }
}
