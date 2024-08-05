using System;
using TMPro;
using UnityEngine;

namespace Zoompy.SystemVisuals
{
	public class ContainerDisplay : MonoBehaviour
	{
		public TMP_Text containerText;
		private MeshRenderer _meshRenderer;
		private void Awake()
		{
			//todo: Set canvas camera to camera.
			_meshRenderer = GetComponentInChildren<MeshRenderer>();
		}

		public void SetName(string name)
		{
			containerText.text = name;
			//todo: scale is fucked
		}
		public void SetMaterial(Material material)
		{
			if (_meshRenderer == null)
			{
				_meshRenderer = GetComponentInChildren<MeshRenderer>();
			}
			GetComponentInChildren<MeshRenderer>().material = material;
		}

		public Bounds GetBounds()
		{
			if (_meshRenderer == null)
			{
				_meshRenderer = GetComponentInChildren<MeshRenderer>();
			}

			return _meshRenderer.bounds;
		}
	}
}