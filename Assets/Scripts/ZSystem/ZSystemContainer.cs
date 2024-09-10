using System.Collections.Generic;
namespace Zoompy
{
	//This is a systemDescription.
	public class ZSystemContainer
	{
		public ZSystem[] Systems;
		public (ZConnection connection, ZSystem from, ZSystem[] to)[] Connections;
	}
}