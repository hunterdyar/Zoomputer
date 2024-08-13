using System;
using UnityEngine;
using Zoompy;

[Logic(Path="Component/LED")]
public class LEDLogic : MonoBehaviour, ISignalHook
{
	//todo: injectable runtime settings! Through generation settings I guess.
	public Material OnMaterial;
	public Material OffMaterial;
	private ComponentSystem _parent;
	private MeshRenderer _mr;

	private void Awake()
	{
		_mr = GetComponentInChildren<MeshRenderer>();
	}
	
	public void OnAnyInputChange()
	{
		UpdateView();
		_parent.Outputs[0].SetSignal(_parent.Inputs[0].GetSignal());
	}

	private void UpdateView()
	{
		var d = _parent.Inputs[0].GetSignal();
		_mr.material = d ? OnMaterial : OffMaterial;
	}

	public void OnInputChange(int index, byte data)
	{
		UpdateView();
		_parent.Outputs[index].SetSignalByte(data);
	}

	public void SetComponenSystem(ComponentSystem parent)
	{
		_parent = parent;
	}
}
