using System.IO;
using UnityEngine;
using Zoompy;

[Logic(Path="Gate/And")]
public class AndLogic : Logic
{
	private ZSystem _parent;
	
	// public void OnAnyInputChange()
	// {
	// 	if (_parent == null)
	// 	{
	// 		Debug.LogWarning("No component system set/initialized for logic", this);
	// 		return;
	// 	}
	// 	_parent.Outputs[0].SetSignal((_parent.Inputs[0].GetSignal()) && (_parent.Inputs[1].GetSignal()));
	// }

	public override void OnInputChange(ZConnection c, byte d)
	{
		
	}
}
