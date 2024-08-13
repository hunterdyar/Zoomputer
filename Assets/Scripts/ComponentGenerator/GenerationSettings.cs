using UnityEngine;
using UnityEngine.UIElements;
using Zoompy;
using Zoompy.SystemVisuals;

namespace Zoompy.Generator
{
	[CreateAssetMenu(fileName = "GenSettings", menuName = "Component/Generation Settings", order = 0)]
	public class GenerationSettings : ScriptableObject
	{
		public Vector3 ContainerBaseSize = Vector3.one;
		public float PortSize;
		public ContainerDisplay ContainerDisplayPrefab;
		public Wire WirePrefab;
		public SignalPort PortPrefab;
		public Material defaultSystemMaterial;
		public float containerMargin;
		public float portgap = 0.1f;
	}
}