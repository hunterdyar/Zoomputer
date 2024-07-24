using UnityEngine;
using Zoompy;
using Zoompy.Logic;

public class NandLogic : MonoBehaviour, ISignalHook
{
    private ComponentSystem _parent;

    //todo: validate input counts
    
    public void SetComponenSystem(ComponentSystem parent)
    {
        this._parent = parent;
    }

    public void OnAnyInputChange()
    {
        if (_parent == null)
        {
            Debug.LogWarning("No component system set/initialized for logic",this);
            return;
        }

    //okay so here we use NOR logic to make a NAND logic. lol.
        _parent.Outputs[0].SetSignal((!_parent.Inputs[0].GetSignal()) || !(_parent.Inputs[1].GetSignal()));
    }

    public void OnInputChange(int index, byte data)
    {
       OnAnyInputChange();
    }
}
