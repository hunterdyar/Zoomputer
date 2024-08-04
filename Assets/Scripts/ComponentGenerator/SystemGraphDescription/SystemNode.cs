using UnityEngine;

namespace Zoompy.Generator
{
	[System.Serializable]
	public class SystemNode 
	{
		public string NodeID;
		public ComponentGenerator System;
		public Vector2 Position;
		public Vector2 Size;
	}
}