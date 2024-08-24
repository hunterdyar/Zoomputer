using System;
using UnityEngine;
using Zoompy;
using Zoompy.Generator;

[Logic(Path="Component/LED")]
public class LEDLogic : MonoBehaviour, ISignalHook
{
	//todo: injectable runtime settings! Through generation settings I guess.
	[SerializeField, HideInInspector]
	private bool SwitchMaterial;
	[SerializeField, HideInInspector]
	public Material OnMaterial;
	[SerializeField, HideInInspector]
	public Material OffMaterial;
	public GameObject OnChild;
	public GameObject OffChild;
	private ComponentSystem _parent;
	private MeshRenderer _mr;

	private void Awake()
	{
		_mr = GetComponentInChildren<MeshRenderer>();
	}

	public void ApplyConfiguration(ComponentSystem cs, GenerationSettings genSettings)
	{
		SwitchMaterial = true;
		OnMaterial = genSettings.litMaterial;
		OffMaterial = genSettings.unlitMaterial == null ? genSettings.defaultSystemMaterial : genSettings.unlitMaterial;
	}
	
	public void OnAnyInputChange()
	{
		UpdateView();
		_parent.Outputs[0].SetSignal(_parent.Inputs[0].GetSignal());
	}

	private void UpdateView()
	{
		var d = _parent.Inputs[0].GetSignal();
		if (SwitchMaterial)
		{
			_mr.material = d ? OnMaterial : OffMaterial;
		}

		if (OnChild != null)
		{
			OnChild.SetActive(d);
		}

		if (OffChild != null)
		{
			OffChild.SetActive(!d);
		}

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
