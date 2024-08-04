using System.Collections.Generic;

namespace Zoompy.Generator
{
	[System.Serializable]
	public class SystemDescription
	{
		public List<SystemNode> Nodes = new List<SystemNode>();
		public List<SystemEdge> Edges = new List<SystemEdge>();
	}
}