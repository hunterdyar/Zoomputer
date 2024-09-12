using System;

namespace Zoompy
{
	public struct ZConnection : IEquatable<ZConnection>
	{
		public readonly int ID;
		public int FromIndex;
		public Action<byte> OnDidChangedAfterImpulse;//Called after the entire system has propogated, for passive listeners.
		public ZConnection(int id, int index = 0)
		{
			this.ID = id;
			FromIndex = index;
		}

		public bool Equals(ZConnection other)
		{
			return ID == other.ID && FromIndex == other.FromIndex;
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