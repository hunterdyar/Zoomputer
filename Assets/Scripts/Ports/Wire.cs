using System;
using UnityEngine;

namespace Zoompy
{
	public class Wire : MonoBehaviour
	{
		public SignalPort From;
		public SignalPort To;

		private void OnEnable()
		{
			From.OnInputChange+= OnInputChange;
		}

		private void OnDisable()
		{
			From.OnInputChange += OnInputChange;
		}

		private void OnInputChange(int index, byte signal)
		{
			To.SetSignalByte(signal);
		}
	}
}