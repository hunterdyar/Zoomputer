using System;
using TMPro;
using UnityEngine;

namespace Zoompy.SystemVisuals
{
	public class ContainerDisplay : MonoBehaviour
	{
		public TMP_Text containerText;

		private void Awake()
		{
			//todo: Set canvas camera to camera.
		}

		public void SetName(string name)
		{
			containerText.text = name;
			//todo: scale is fucked
		}
		public void SetMaterial(Material material)
		{
			GetComponentInChildren<MeshRenderer>().material = material;
		}
	}
}