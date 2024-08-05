using UnityEngine;

namespace Zoompy.Generator
{
	[System.Serializable]
	public class SystemEdge 
	{
		//todo: this could be references to SystemNode instead of GUIDs.
		public string FromNode;
		public int FromIndex;
		public string ToNode;
		public int ToIndex;
	}
}