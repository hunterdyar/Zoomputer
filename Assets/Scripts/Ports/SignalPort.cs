using System;
using UnityEngine;

namespace Zoompy
{
	public class SignalPort : MonoBehaviour
	{
		public Action<int,byte> OnInputChange; 
		public ComponentSystem ConnectedTo;
		private byte _data;
		public int ConnectedIndex;

		private void Start()
		{
			_data = 0;
			OnInputChange?.Invoke(ConnectedIndex,_data);
		}

		public bool GetSignal()
		{
			return _data != 0;
		}

		public void SetSignal(bool data)
		{
			var nd = data ? (byte)1 : (byte)0;
			if (_data != nd)
			{
				this._data = nd;
				OnInputChange?.Invoke(ConnectedIndex, _data);
			}
		}

		public byte GetSignalByte()
		{
			return _data;
		}

		public void SetSignalByte(byte data)
		{
			if (this._data != data)
			{
				this._data = data;
				OnInputChange?.Invoke(ConnectedIndex,_data);
			}
		}

		
	}
}