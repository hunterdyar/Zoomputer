using UnityEngine;

namespace Zoompy
{
	public class ZSystem
	{
		public string name;
		
		//how are we referencing ports/connections? structs?
		public ZConnection[] inputs;
		public ZConnection[] outputs;
		
		//Display Information
		public float width;
		public float height;
		/// <summary>
		/// A normalized position (0-1) inside the graph.
		/// </summary>
		public Vector2 relPosition;
		//Color
		
		//logic outer layer
		public ISignalHook Logic;
		
		//Inner layer definition.
		public bool IsLeaf;
		public ZSystemContainer Internals;

		public void Initialize(ConnectionHub hub)
		{
			//register ZConnetion Listeners with Logic.
		}
	}
}