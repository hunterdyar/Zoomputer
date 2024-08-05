using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Zoompy.Generator
{
	[CreateAssetMenu(fileName = "Gneratore", menuName = "Component/Generator", order = 0)]
	public class ComponentGenerator : ScriptableObject
	{
		[Min(0)]
		public int numberInputs;
		[Min(0)]
		public int numberOutputs;
		
		[HideInInspector]
		public string baseLogicClassName;

		//Visual Details
		[SerializeField] private GenerationSettings _genSettings;

		private Material ContainerMaterial() => _overrideContainerMaterial == null ? _genSettings.defaultSystemMaterial : _overrideContainerMaterial;
		[Tooltip("Optional material for container object.")]
		[SerializeField] private Material _overrideContainerMaterial;

		private int MaxPorts => numberInputs > numberOutputs ? numberInputs : numberOutputs;
		private float Height() => _genSettings.containerMargin* 2 + MaxPorts* _genSettings.PortSize + (MaxPorts* _genSettings.portgap);

		public SystemDescription InnerSystem;

		[SerializeField] public bool IsLeaf;
		
		[ContextMenu("Generate")]
		private void Gen()
		{
			Generate(null);
		}
		public GameObject Generate(Transform parent = null)
		{
			var g = new GameObject();
			var cs = g.AddComponent<ComponentSystem>();
			//set display name
			string rootName = StripLogicSuffix(baseLogicClassName);
			cs.SetDisplayName(rootName);
			g.name = rootName;//may get renamed later.
			
			//spawn and position outer container
			var container = GenerateContainer(cs);//sets itself as child of sysetm
			
			//add logic
			var logicType = Type.GetType(baseLogicClassName);
			g.AddComponent(logicType);
			
			//spawn and position inputs
			var ig = new GameObject();
			ig.name = "Input Ports";
			ig.transform.SetParent(cs.transform);
			cs.SetPorts(new SignalPort[numberInputs], PortType.Input);
			for (int i = 0; i < numberInputs; i++)
			{
				var p = CreatePort(cs, i, PortType.Input, ig.transform);
			}
			
			//spawn and position outputs
			ig = new GameObject();
			ig.name = "Output Ports";
			ig.transform.SetParent(cs.transform);
			cs.SetPorts(new SignalPort[numberOutputs], PortType.Output);
			for (int i = 0; i < numberOutputs; i++)
			{
				var p = CreatePort(cs,i,PortType.Output, ig.transform);
			}

			GenerateInnerSystem(cs);
			
			//connect inner systems with wires.

			return g;
		}

		void GenerateInnerSystem(ComponentSystem cs)
		{
			cs.SetIsLeaf(IsLeaf);
			if (IsLeaf)
			{
				return;
			}
		
			GameObject innerSystem = new GameObject();
			innerSystem.name = "Inner System View";
			innerSystem.transform.SetParent(cs.transform);
			//set ... scale?
			//we can get bounds from the container.
			var bounds = cs.transform.GetChild(0).GetComponentInChildren<MeshRenderer>().bounds;
			
			//the .2 and the 5 come from an arbitraryscaleodwn
			var scale = 5;
			innerSystem.transform.localScale = Vector3.one * (1f /scale);
			
			foreach (var systemNode in InnerSystem.Nodes)
			{
				var relX = Mathf.InverseLerp(InnerSystem.Bounds.xMin, InnerSystem.Bounds.xMax, systemNode.Position.x+systemNode.Size.x/2);
				var relY = Mathf.InverseLerp(InnerSystem.Bounds.yMin, InnerSystem.Bounds.yMax, systemNode.Position.y+systemNode.Size.y/2);
				
				var gameNode = systemNode.System.Generate();
				gameNode.transform.SetParent(innerSystem.transform);
				//set position. 
				//todo: how do we transform these positions to the world space?
				gameNode.transform.localPosition = new Vector3(Mathf.Lerp(bounds.min.x, bounds.max.x,relX)*scale, 0, Mathf.Lerp(bounds.min.y, bounds.max.y, relY)*scale);
				//scale is size over area on each axis
				gameNode.transform.localScale = Vector3.one;
			}

			foreach (var edge in InnerSystem.Edges)
			{
				//todo: Instantiate a wire
			}
		}
		

		private GameObject GenerateContainer(ComponentSystem system)
		{
			//get max number of ports.
			int ports = MaxPorts;

			//configure layer view in contianer
			var layerg = new GameObject();
			layerg.name = system.DisplayName + " Outer View";
			layerg.transform.SetParent(system.transform);
			var lv = layerg.AddComponent<LayerView>();
			system.outsideView = lv;
			
			//create child for visuals. box with height = margin*2+ports*portSize;
			var display = Instantiate(_genSettings.ContainerDisplayPrefab, layerg.transform);
			display.transform.localScale = new Vector3(_genSettings.ContainerBaseSize.x, _genSettings.ContainerBaseSize.y, _genSettings.ContainerBaseSize.z*Height()); //*scale
			
			//apply material
			display.SetMaterial(ContainerMaterial());
			display.SetName(system.DisplayName.ToUpper());
			return layerg;
		}

		private SignalPort CreatePort(ComponentSystem cs, int index, PortType portType, Transform parent = null)
		{
			var port = Instantiate(_genSettings.PortPrefab, parent);
			port.name = (portType == PortType.Input ? "In " : "Out ") + index.ToString();
			
			//localposition calculation
			var xPos = _genSettings.ContainerBaseSize.x / 2;
			float centeringOffset = 0;
			if (portType == PortType.Input)
			{
				xPos = -xPos;
				cs.Inputs[index] = port;
				if (numberInputs < numberOutputs)
				{
					var diff = numberOutputs - numberInputs;
					centeringOffset = diff * _genSettings.PortSize / 2 + diff * _genSettings.portgap / 2 +_genSettings.portgap/2;
				}
			}
			else
			{
				if (numberOutputs < numberInputs)
				{
					var diff = numberInputs - numberOutputs;
					centeringOffset = diff * _genSettings.PortSize / 2 + diff * _genSettings.portgap / 2 + _genSettings.portgap / 2;
				}
				cs.Outputs[index] = port;
			}

			
			var zPos = -Height()/2 //base position is at the extent
			           + _genSettings.PortSize/2//minus half of own scale to center
				+ _genSettings.containerMargin //minus one of the margins
				+ _genSettings.PortSize*index//minus the number of ports
				+_genSettings.portgap*index 
			    +centeringOffset;//plus gap between ports
			
			var yPos = 0;
			port.transform.localPosition = new Vector3(xPos, yPos, zPos);
			port.transform.localScale = Vector3.one * _genSettings.PortSize;

			port.ConnectedIndex = index;
			port.ConnectedTo = cs;
			return port;
		}

		
		public static string StripLogicSuffix(string s)
		{
			if (s.Length < 6)
			{
				return s;
			}
			if (s.Substring(s.Length - 5, 5) == "Logic")
			{
				return s.Remove(s.Length - 5);
			}

			return s;
		}
	}
}