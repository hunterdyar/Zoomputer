using System;

namespace Zoompy
{
	public struct ZConnection : IEquatable<ZConnection>
	{
		public readonly int ID;

		public ZConnection(int id)
		{
			this.ID = id;
		}

		public bool Equals(ZConnection other)
		{
			return ID == other.ID;
		}

		public override bool Equals(object obj)
		{
			return obj is ZConnection other && Equals(other);
		}

		public override int GetHashCode()
		{
			return ID;
		}
	}
}