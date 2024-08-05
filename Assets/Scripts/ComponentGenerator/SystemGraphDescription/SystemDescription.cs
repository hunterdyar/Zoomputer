using System.Collections.Generic;
using UnityEngine;

namespace Zoompy.Generator
{
	[System.Serializable]
	public class SystemDescription
	{
		//save the input and output guids for the sake of determining edges.
		public string Input;
		public Rect InputPos;
		public string Output;
		public Rect OutputPos;
		
		public SystemNode[] Nodes;
		public SystemEdge[] Edges;

		public Rect Bounds;
	}
}