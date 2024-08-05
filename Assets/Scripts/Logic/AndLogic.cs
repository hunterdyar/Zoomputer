using UnityEngine;
using Zoompy;

[Logic]
public class AndLogic : MonoBehaviour, ISignalHook
{
	private ComponentSystem _parent;

	//todo: validate input counts

	public void SetComponenSystem(ComponentSystem parent)
	{
		this._parent = parent;
	}

	private void OnEnable()
	{
		if (_parent != null)
		{
			OnAnyInputChange();
		}
	}

	public void OnAnyInputChange()
	{
		if (_parent == null)
		{
			Debug.LogWarning("No component system set/initialized for logic", this);
			return;
		}
		_parent.Outputs[0].SetSignal((_parent.Inputs[0].GetSignal()) && (_parent.Inputs[1].GetSignal()));
	}

	public void OnInputChange(int index, byte data)
	{
		OnAnyInputChange();
	}
}
