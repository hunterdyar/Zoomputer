using System;
using UnityEngine;

namespace Zoompy
{
	public class PortMaterialUpdater : MonoBehaviour
	{
		private SignalPort _port;
		private MeshRenderer _meshRenderer;
		[SerializeField] private Material offMaterial;
		[SerializeField] private Material onMaterial;
		private void Awake()
		{
			_meshRenderer = GetComponent<MeshRenderer>();
			_port = GetComponentInParent<SignalPort>();
		}

		private void OnEnable()
		{
			_port.OnInputChange += OnInputChange;
		}

		private void OnDisable()
		{
			_port.OnInputChange -= OnInputChange;
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
			_port.SetSignal(!_port.GetSignal());
		}
	}
}