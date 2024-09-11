using System;
using Zoompy.Generator;

namespace Zoompy
{
	public abstract class Logic
	{
		protected ZSystem _system;
		protected ConnectionHub _hub;
		public abstract void OnInputChange(ZConnection c, byte d);

		public void SetZSystem(ZSystem system, ConnectionHub hub)
		{
			_system = system;
			_hub = hub;
		}
		
		protected byte GetConnection(ZConnection c)
		{
			return _hub.Get(c);
		}
	}
}