using System;
using UnityEngine;
using Zoompy.Generator;

namespace Zoompy
{
	public class SystemDisplayManager : MonoBehaviour
	{
		private ZSystem _currentSystem;
		
		//We can track through the system (step out, etc) but that should be done by some graph handler that can deal with all of our edge cases.
		public PanelModelController panelPrefab;
		public Bounds containerBounds;
		
		private void Start()
		{
			
		}

		public void ClearCurrentSystem()
		{
			//Clear Currennt System
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}

		public void DisplaySystem(ZSystem system)
		{
			ClearCurrentSystem();
			
			//draw the header text from system name.
			//draw the input and output ports.
			//draw the nodes.
			for (int i = 0; i < system.Internals.Systems.Length; i++)
			{
				var panel = Instantiate(panelPrefab, transform);
				panel.SetWorldContext(containerBounds);
				panel.SetToSystem(system.Internals.Systems[i]);
				panel.gameObject.name = system.Internals.Systems[i].name + " Panel ";
			}
		}
	}
}