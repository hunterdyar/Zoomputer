using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Zoompy;

namespace Zoompy.Generator
{
	[CreateAssetMenu(fileName = "Generator", menuName = "Component/Component Generator", order = 0)]
	public class ComponentGenerator : ScriptableObject
	{
		[Min(0)]
		public int numberInputs;
		[Min(0)]
		public int numberOutputs;
		
		[HideInInspector]
		public string baseLogicClassName;
		public string baseInteractorClassName;
		
		//Visual Details
		[SerializeField] private GenerationSettings _genSettings;

		private Material ContainerMaterial() => _overrideContainerMaterial == null ? _genSettings.defaultSystemMaterial : _overrideContainerMaterial;
		[Tooltip("Optional material for container object.")]
		[SerializeField] private Material _overrideContainerMaterial;
		//todo: override generated object (buttons, LED's, etc)
		private int MaxPorts => numberInputs > numberOutputs ? numberInputs : numberOutputs;
		private float Height() => _genSettings.containerMargin* 2 + MaxPorts* _genSettings.PortSize + (MaxPorts* _genSettings.portgap);
		
		public SystemDescription InnerSystem;
		[SerializeField] public bool IsLeaf;
		
		[ContextMenu("Generate")]
		private void Gen()
		{
			Generate(null);
		}
		public ComponentSystem Generate(Transform parent = null)
		{
			if (_genSettings == null)
			{
				Debug.LogError("Can't generate, No Generation Settings!");
				return null;
			}
			//make the game object
			var g = new GameObject();
			var cs = g.AddComponent<ComponentSystem>();
			
			//set display name
			string rootName = name;//StripLogicSuffix(baseLogicClassName);
			cs.SetDisplayName(rootName);
			g.name = rootName;//may get renamed later.
			
			//spawn and position outer container
			var containerDisplay = GenerateContainer(cs);//sets itself as child of sysetm
			
			//add logic
			//todo: check for validity
			ISignalHook logic = null;
			if (baseLogicClassName != "None" && baseLogicClassName !="")
			{
				var logicType = Type.GetType(baseLogicClassName);
				if (logicType != null)
				{
					logic = (ISignalHook)g.AddComponent(logicType);
				}
				else
				{
					Debug.LogWarning($"Could not Get Type for Logic: {baseLogicClassName}");
				}
			}

			if (numberInputs > 0)
			{
				//spawn and position inputs
				var ig = new GameObject();
				ig.name = "Input Ports";
				ig.transform.SetParent(cs.transform);
				cs.SetPorts(new SignalPort[numberInputs], PortType.Input);
				cs.inputNodeID = InnerSystem.Input; //reference for node lookup
				for (int i = 0; i < numberInputs; i++)
				{
					var p = CreatePort(cs, i, PortType.Input, ig.transform);
				}
			}


			//spawn and position outputs
			if (numberOutputs > 0)
			{
				var ig = new GameObject();
				ig.name = "Output Ports";
				ig.transform.SetParent(cs.transform);
				cs.SetPorts(new SignalPort[numberOutputs], PortType.Output);
				cs.outputNodeID = InnerSystem.Output; //reference for node lookup
				for (int i = 0; i < numberOutputs; i++)
				{
					var p = CreatePort(cs, i, PortType.Output, ig.transform);
				}
			}

			GenerateInnerSystem(containerDisplay, cs);
			
			//connect inner systems with wires.
			//do this after nodes so we have all the port worldPositions.
			GenerateWires(cs);
			
			//after the rest has been generated.
			if (logic != null)
			{
				logic.ApplyConfiguration(cs,_genSettings);
			}

			IComponentInteractor interactor = null;
			if (baseInteractorClassName != "None" && !string.IsNullOrEmpty(baseInteractorClassName))
			{
				var interactorType = Type.GetType(baseInteractorClassName);
				if (interactorType != null)
				{
					interactor = (IComponentInteractor)g.AddComponent(interactorType);
				}
				else
				{
					Debug.LogWarning($"Could not Get Type for Logic: {baseLogicClassName}");
				}

				if (interactor != null)
				{
					//we do configure on start instead of serialized...
					// interactor.Configure();
				}
			}
			
			cs.Init();
			return cs;
		}

		private void GenerateWires(ComponentSystem cs)
		{
			if (InnerSystem.Edges.Length == 0)
			{
				return;
			}
			var wireParent = new GameObject();
			wireParent.name = "Connections";
			wireParent.transform.SetParent(cs.transform);
			foreach (var edge in InnerSystem.Edges)
			{
				var fb = cs.TryGetInnerPort(edge.FromNode,PortType.Output, edge.FromIndex, out SignalPort fromPort);
				var ft = cs.TryGetInnerPort(edge.ToNode, PortType.Input, edge.ToIndex, out SignalPort toPort);
				if (fb && ft)
				{
					Wire w = Instantiate(_genSettings.WirePrefab, wireParent.transform);
					w.Configure(fromPort, toPort);
				}
				else
				{
					Debug.LogWarning("uh oh");
				}
			}
		}

		string PortLookupKey(string nodeID, int index)
		{
			return nodeID + "_" + index.ToString();
		}

		void GenerateInnerSystem(ContainerDisplay outerContainer, ComponentSystem cs)
		{
			cs.SetIsLeaf(IsLeaf);
			if (IsLeaf)
			{
				return;
			}
		
			GameObject innerSystem = new GameObject();
			cs.insideView = innerSystem.AddComponent<LayerView>();
			
			innerSystem.name = "Inner System View";
			innerSystem.transform.SetParent(cs.transform);
			//set ... scale?
			var outerBounds = outerContainer.GetBounds();
			//inner is a rect in 2D space. Here we make a 3D bounds for it on xz plane, using the outer settings for the y.
			var innerBounds = new Bounds(new Vector3(InnerSystem.Bounds.center.x,outerBounds.center.y,InnerSystem.Bounds.y),
				new Vector3(InnerSystem.Bounds.size.x, outerBounds.size.y, InnerSystem.Bounds.y));
			var xscale = innerBounds.size.x / outerBounds.size.x;
			var zscale = innerBounds.size.z / outerBounds.size.z;
			//map normalized scale to outer bounds scale
			var scale = 1 / outerBounds.size.x;

			cs.insideView.Nodes = new ComponentSystem[InnerSystem.Nodes.Length];
			innerSystem.transform.localScale = Vector3.one * scale;
			for (var i = 0; i < InnerSystem.Nodes.Length; i++)
			{
				var systemNode = InnerSystem.Nodes[i];
				var relX = Mathf.InverseLerp(InnerSystem.Bounds.xMin, InnerSystem.Bounds.xMax,
					systemNode.Position.x + systemNode.Size.x / 2); //+systemNode.Size.x/2
				//1- to flip from top-left origin of graph to bottom-left origin of world.
				var relZ = 1 - Mathf.InverseLerp(InnerSystem.Bounds.yMin, InnerSystem.Bounds.yMax,
					systemNode.Position.y + systemNode.Size.y / 2); //

				//percentage of entire bounds. 
				var widthScale = InnerSystem.Bounds.width / systemNode.Size.x;
				var heightScale = InnerSystem.Bounds.height / systemNode.Size.y;

				var gameNode = systemNode.System.Generate();
				cs.insideView.Nodes[i] = gameNode;//parent reference of inner node
				gameNode.SetGuid(systemNode.NodeID);//inner node keeps id for lookups, for wires.
				
				gameNode.transform.SetParent(innerSystem.transform);
				//set position. 
				gameNode.transform.position = new Vector3(
					Mathf.Lerp(outerBounds.min.x, outerBounds.max.x, relX),
					outerBounds.center.y,
					Mathf.Lerp(outerBounds.min.y, outerBounds.max.y, relZ));
				//scale is size over area on each axis
				gameNode.transform.localScale = Vector3.one / Mathf.Lerp(1, outerBounds.size.x, widthScale) * scale;
			}
			
		}
		

		private ContainerDisplay GenerateContainer(ComponentSystem system)
		{
			//get max number of ports.
			int ports = MaxPorts;

			if (ports == 0)
			{
				//The inner system use the bounds to set the scale of everything
				//so... we need to figure out what to do to set a scale for a non-outer thing.
				//the size is also not calculated correctly when ports are zero.
				//return null;
			}
			
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
			return display;
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
			if(string.IsNullOrEmpty(s))
			{
				Debug.LogError("No Logic! This is ... wrong?");
				return "";
			}
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

		public ISignalHook GetLogic()
		{
			ISignalHook logic = null;
			if (baseLogicClassName != "None" && baseLogicClassName != "")
			{
				var logicType = Type.GetType(baseLogicClassName);
				if (logicType != null)
				{
					//new Logic() oncne not monoBehaviour
					//logic = (ISignalHook)g.AddComponent(logicType);
				}
				else
				{
					Debug.LogWarning($"Could not Get Type for Logic: {baseLogicClassName}");
				}
			}

			return logic;
		}

		public ZSystem GetSystem(ConnectionHub hub)
		{
			ZSystem system = new ZSystem();
			system.IsLeaf = IsLeaf;
			system.name = this.name;
			system.Internals = new ZSystemContainer();
			system.Internals.Systems = new ZSystem[InnerSystem.Nodes.Length];
			system.Internals.Connections = new (ZConnection,ZSystem,ZSystem)[InnerSystem.Edges.Length];
			system.Logic = GetLogic();
			//pos is set by outer.... see below.
			
			//
			system.inputs = new ZConnection[numberInputs];
			// foreach (var VARIABLE in InnerSystem.Edges.Where(x=>x.ToIndex == ))
			
			system.outputs = new ZConnection[numberOutputs];			
			
			Dictionary<string,ZSystem> map = new Dictionary<string, ZSystem>();
			if (!system.IsLeaf)
			{
				for (int i = 0; i < InnerSystem.Nodes.Length; i++)
				{
					system.Internals.Systems[i] = InnerSystem.Nodes[i].System.GetSystem(hub);
					
					var pos = new Vector2(
						Mathf.InverseLerp(InnerSystem.Bounds.xMin, InnerSystem.Bounds.xMax, InnerSystem.Nodes[i].Position.x),
						Mathf.InverseLerp(InnerSystem.Bounds.yMin, InnerSystem.Bounds.yMax, InnerSystem.Nodes[i].Position.y));
					
					system.Internals.Systems[i].relPosition = pos;
					map.Add(InnerSystem.Nodes[i].NodeID, system.Internals.Systems[i]);
				}

				for (int i = 0; i < InnerSystem.Edges.Length; i++)
				{
					if (InnerSystem.Edges[i].FromNode == "" || InnerSystem.Edges[i].ToNode == "")
					{
						continue;
					}
					
					if (!map.ContainsKey(InnerSystem.Edges[i].FromNode) || !map.ContainsKey(InnerSystem.Edges[i].ToNode))
					{
						Debug.LogWarning($"Edge connected to invalid node? {system.name}");
						continue;
					}
					var c = hub.GetConnection();

					var fromn = map[InnerSystem.Edges[i].FromNode];
					var ton = map[InnerSystem.Edges[i].ToNode];
					
					//ZConnection isn't an object, it's just an ID, so we can store it in multiple places.
					fromn.outputs[InnerSystem.Edges[i].FromIndex] = c;
					ton.inputs[InnerSystem.Edges[i].ToIndex] = c;

					system.Internals.Connections[i] = (c, fromn, ton);

				}
			}

			return system;
		}
	}
}