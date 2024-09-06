using UnityEngine;


namespace Zoompy.Generator
{
	[CreateAssetMenu(fileName = "GenSettings", menuName = "Component/Generation Settings", order = 0)]
	public class GenerationSettings : ScriptableObject
	{
		[Header("Prefabs")]
		public ContainerDisplay ContainerDisplayPrefab;
		public Wire WirePrefab;
		public SignalPort PortPrefab;
		
		[Header("Base Style Settings")]
		public Vector3 ContainerBaseSize = Vector3.one;
		public Material defaultSystemMaterial;
		public float containerMargin;
		public float portgap = 0.1f;
		public float PortSize;
		
		[Header("Unqiue Style Settings")]
		public Material litMaterial;
		public Material unlitMaterial;
	}
}