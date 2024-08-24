using System;

namespace Zoompy
{
	[AttributeUsage(AttributeTargets.Class)]
	public class InteractorAttribute : Attribute
	{
		public string Path;
	}
}