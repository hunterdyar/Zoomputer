using System;
using UnityEngine;

namespace Zoompy
{
	public class ZSystem
	{
		public string name;
		
		public ZConnection[] Inputs;
		public ZConnection[] InternalInputs;
		public ZConnection[] Outputs;
		public ZConnection[] InternalOutputs;

		public bool RunInternal = false;
		public bool ShouldRunInternal() => !IsLeaf && RunInternal; 
		//Display Information
		public float width;
		public float height;
		/// <summary>
		/// A normalized position (0-1) inside the graph.
		/// </summary>
		public Vector2 relPosition;
		//Color
		
		//logic outer layer
		public Logic Logic;
		
		//Inner layer definition.
		public bool IsLeaf;
		public ZSystemContainer Internals;

		private bool _initialized = false;
		public void Initialize(ConnectionHub hub)
		{
			if (_initialized)
			{
				throw new Exception("Recursive Initialization Loop Detected! Bad graph. Abort!");
			}
			
			//register ZConnetion Listeners with Logic.
			RegisterConnections(hub);
			
			if (!IsLeaf)
			{
				foreach (var child in Internals.Systems)
				{
					child.Initialize(hub);
				}
			}
			
			//check if !IsLeaf and Logic is not null safety.
			if (Logic != null)
			{
				Logic.SetZSystem(this, hub);
			}


			//set to true by default for all systems that have logic.
			RunInternal = !IsLeaf;
			_initialized = true;
		}
		
		//We subscribe to all zconnection changes to our external inputs.
		//If we are displaying internals, we invoke the matching internal input.
		//if not, we call our logic function, which presumably calls our external output.
		//we also subscribe to internal outputs, and pass those along to external outputs. We don't have to turn this on or off, PRESUMABLY, because it should be reactive to input that is on/off.

		public void RegisterConnections(ConnectionHub hub)
		{
			foreach (var input in Inputs)
			{
				hub.RegisterListener(input, OnExternalInputChange);
			}
		}

		//todo: remove the hub injection and cache on initializations
		private void OnExternalInputChange(ZConnection c, byte d, ConnectionHub hub)
		{
			if (ShouldRunInternal())
			{
				var x = Array.IndexOf(Inputs, c);
				if (x >= 0)
				{
					hub.SetConnection(InternalInputs[x], d);
				}
			}
			else
			{
				//logic.Invoke.
				if (Logic != null)
				{
					Logic.OnInputChange(c, d);
				}
				else
				{
					Debug.LogError($"Could not run logic on {this}!");
				}
			}
		}
	}
}