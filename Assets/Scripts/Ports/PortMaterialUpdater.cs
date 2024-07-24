using System;
using UnityEngine;

namespace Zoompy
{
	public class PortMaterialUpdater : MonoBehaviour
	{
		private SignalPort _port;
		private Wire _wire;
		private MeshRenderer _meshRenderer;
		[SerializeField] private Material offMaterial;
		[SerializeField] private Material onMaterial;
		private void Awake()
		{
			_meshRenderer = GetComponent<MeshRenderer>();
			_port = GetComponentInParent<SignalPort>();
			_wire = GetComponentInParent<Wire>();
		}

		private void OnEnable()
		{
			if (_port != null)
			{
				_port.OnInputChange += OnInputChange;
			}else if (_wire != null)
			{
				_wire.From.OnInputChange += OnInputChange;
			}
		}

		private void OnDisable()
		{
			if (_port != null)
			{
				_port.OnInputChange -= OnInputChange;
			}
			else if (_wire != null)
			{
				_wire.From.OnInputChange -= OnInputChange;
			}
		}

		private void OnInputChange(int index, byte data)
		{
			if (data == 0)
			{
				_meshRenderer.material = offMaterial;
			}
			else
			{
				_meshRenderer.material = onMaterial;
			}
		}

		private void OnMouseDown()
		{
			if (_port != null)
			{
				
			}
			else if (_wire != null)
			{
				_wire.From.SetSignal(!_wire.From.GetSignal());
			}
			
		}
	}
}