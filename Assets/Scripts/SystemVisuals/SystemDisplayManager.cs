using System;
using System.Collections.Generic;
using UnityEngine;
using Zoompy.Generator;
using Zoompy.Panels;

namespace Zoompy
{
	public class SystemDisplayManager : MonoBehaviour
	{
		private ZSystem _currentSystem;
		private EnclosureController _enclosure;
		
		//We can track through the system (step out, etc) but that should be done by some graph handler that can deal with all of our edge cases.
		public PanelModelController panelPrefab;
		public WireCreator wirePrefab;
		
		
		public Bounds containerBounds;
		
		private Dictionary<ZSystem, PanelModelController> panels = new Dictionary<ZSystem, PanelModelController>();
		//private Dictionary<ZConnection, WireCreator> wires = new Dictionary<ZConnection, WireCreator>();

		private void Awake()
		{
			_enclosure = GetComponent<EnclosureController>();
		}

		public void ClearCurrentSystem()
		{
			//Clear Currennt System
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
			panels.Clear();
			//wires.Clear();
		}

		public void DisplaySystem(ConnectionHub hub, ZSystem system)
		{
			ClearCurrentSystem();
			
			_enclosure.SetSystem(system, containerBounds, hub);
			
			//draw the header text from system name.
			//draw the input and output ports.
			//draw the nodes.
			for (int i = 0; i < system.Internals.Systems.Length; i++)
			{
				var panel = Instantiate(panelPrefab, transform);
				panel.SetWorldContext(containerBounds);
				panel.SetToSystem(system.Internals.Systems[i]);
				panel.gameObject.name = system.Internals.Systems[i].name + " Panel ";
				panels.Add(system.Internals.Systems[i],panel);
			}

			//loop through all of the 'from nodes'
			for (int i = 0; i < system.Internals.Connections.Length; i++)
			{
				
				var c = system.Internals.Connections[i];

				//make a wire for every 'to node'
				foreach (var to in c.to)
				{
					var wire = Instantiate(wirePrefab, transform);
					var from = c.from;
					wire.SetConnection(hub, c.connection);
					if (from == null || to == null)
					{
						continue;
					}

					//from ourself means from outer
					if (c.from == system)
					{
						var w = _enclosure.GetPort(c.connection);
						if (w == null)
						{
							wire.gameObject.SetActive(false);
							continue;
						}
						//todo: rename or write functions
						wire._portA = w.transform;
					}
					else
					{
						wire._portA = panels[from].GetPortTransform(c.connection);
					}


					if (to == system)
					{
						wire._portB = _enclosure.GetPort(c.connection).transform;
					}
					else
					{
						wire._portB = panels[to].GetPortTransform(c.connection);
					}

				//	wires.Add(c.connection, wire);

				}
			}
		}
	}
}