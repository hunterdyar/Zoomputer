using System.Collections.Generic;
using System.Linq;

namespace Zoompy
{
	//Stores the history of a single tick.
	public class Impulse
	{
		public readonly List<(ZConnection, byte, bool)> Changes = new List<(ZConnection, byte, bool)>();

		public void Mark(ZConnection connectionID, byte newData, bool changed)
		{
			Changes.Add((connectionID,newData, changed));
		}

		public List<ZConnection> GetChangedConnections()
		{
			//todo: check if a change is here already, and replace instead?
			return Changes.Where(x => x.Item3).Select(x=>x.Item1).ToList();
		}
	}
}