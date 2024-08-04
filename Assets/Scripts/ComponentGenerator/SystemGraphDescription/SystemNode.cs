using UnityEngine;

namespace Zoompy.Generator
{
	[System.Serializable]
	public class SystemNode 
	{
		public string NodeID;
		public Zoompy.Generator.ComponentGenerator System;
		public Vector2 Position;
	}
}