using System.Collections.Generic;

namespace Zoompy
{
	//Stores the history of a single tick.
	public class Impulse
	{
		public List<(ZConnection, byte)> Changes;

		public void Mark(ZConnection connectionID, byte newData)
		{
			Changes.Add((connectionID,newData));
		}
	}
}