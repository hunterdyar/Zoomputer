using System.Collections.Generic;

namespace Zoompy
{
	//Stores the history of a single tick.
	public class Impulse
	{
		public readonly List<(ZConnection, byte)> Changes = new List<(ZConnection, byte)>();

		public void Mark(ZConnection connectionID, byte newData)
		{
			Changes.Add((connectionID,newData));
		}
	}
}