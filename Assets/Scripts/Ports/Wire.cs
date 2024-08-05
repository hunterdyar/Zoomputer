using System;
using UnityEngine;

namespace Zoompy
{
	public class Wire : MonoBehaviour
	{
		public SignalPort From;
		public SignalPort To;
		public WireView wireView;
		private void OnEnable()
		{
			From.OnInputChange+= OnInputChange;
			To.SetSignalByte(From.GetSignalByte());
		}

		private void OnDisable()
		{
			From.OnInputChange += OnInputChange;
		}

		private void Start()
		{
			
		}

		public void Configure(SignalPort from, SignalPort to)
		{
			From = from;
			To = to;
			gameObject.name = from.name + "->" + to.name;
			if (wireView != null)
			{
				wireView.SetWire(this);
				wireView.RebuildSpline();
			}
		}
		

		private void OnInputChange(int index, byte signal)
		{
			//the logic of a wire, is it "set to to from" or "set to from from". 
			To.SetSignalByte(signal);
		}
	}
}