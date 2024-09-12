using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zoompy
{
	/// <summary>
	/// The connection hub is a manager for all node-to-node connections.
	/// I don't think we will need node-to-[multiplenodes], those can be different connections.
	/// A wire can only be in one state, and that's just some byte data. We don't think of connections as wires with two ports, but as a single state that we can either set (outgoing port) or receive (incoming port)
	/// The system is a directed graph, so we don't know or care where an impulse came from while propogating it - we store that information in our change class via the impulse method to allow for backtracing and such.
	/// </summary>
	public class ConnectionHub
	{
		//the current state of the system. This is 
		private readonly Dictionary<ZConnection, byte> _connectionData = new Dictionary<ZConnection, byte>();
		private readonly Dictionary<ZConnection, Action<ZConnection, byte, ConnectionHub>> _listeners = new Dictionary<ZConnection, Action<ZConnection, byte, ConnectionHub>>();
		
		private readonly List<Impulse> _impulses = new List<Impulse>();
		// public Dictionary<ZConnection, IConnectionListener> ConnectionListeners { get; set; }

		private Impulse _currentImpulse;
		private int _connIDCount = 0;

		//todo: rename these to ... something else. These are part 
		public void RegisterSystemListener(ZConnection connectionID, Action<ZConnection, byte, ConnectionHub> listener)
		{
			if (_listeners.ContainsKey(connectionID))
			{
				_listeners[connectionID] += listener;
			}
			else
			{
				_listeners.Add(connectionID, listener);
			}
		}
		public void RemoveSystemListener(ZConnection connectionID, Action<ZConnection, byte, ConnectionHub> listener)
		{
			if (_listeners.ContainsKey(connectionID))
			{
				_listeners[connectionID] -= listener;
			}
		}
		
		//Conn
		public void Impulse(ZConnection connectionID, byte newData)
		{
			//create a new Impulse object.
			_currentImpulse = new Impulse();
			
			//Whether or not this is the same as previous, we should notify all listeners.
			SetConnection(connectionID, newData, false);
			
			//When data propogates (a callback from ZSystem basically), we update currentImpulse.
			//we add it to our Impulse object, which we store for tracking.
			
			//Update the visuals and other reactive listeners.
			foreach (var changed in _currentImpulse.GetChangedConnections())
			{
				//todo: broadcast updates of the changed for visual (passive) listeners.
				//we can keep an Action<byte> on the Connection
				changed.OnDidChangedAfterImpulse?.Invoke(_connectionData[changed]);
			}
			
			//after that happens, the code call is back here, and we ... finish.
			_impulses.Add(_currentImpulse);
			Debug.Log(_currentImpulse.Changes.Count);
			
		}

				
		/// <summary>
		/// This should get called by ZSystems/logic as a callback to continue propogating through the system graph.
		/// </summary>
		public void SetConnection(ZConnection connectionID, byte newData, bool onlyUpdateIfChanged = true)
		{
			//Set the data.
			bool didChange = false;
			if (_connectionData.ContainsKey(connectionID))
			{ 
				didChange = _connectionData[connectionID] != newData;
				_connectionData[connectionID] = newData;
			}
			else
			{
				didChange = true;
				_connectionData.Add(connectionID, newData);
			}

			_currentImpulse.Mark(connectionID, newData, didChange);

			//broadcast the changes.
			if (didChange || !onlyUpdateIfChanged)
			{
				//propogate the state.
				if (_listeners.ContainsKey(connectionID))
				{
					_listeners[connectionID].Invoke(connectionID, newData, this);
				}
			}
		}

		public void SetConnection(ZConnection connectionID, bool newData, bool onlyUpdateIfChanged = true)
		{
			SetConnection(connectionID, newData ? (byte)1 : (byte)0, onlyUpdateIfChanged);
		}


		public ZConnection GetNewConnectionFactory()
		{
			_connIDCount++;
			return new ZConnection(_connIDCount);
		}

		public byte Get(ZConnection zConnection)
		{
			if (_connectionData.ContainsKey(zConnection))
			{
				return _connectionData[zConnection];
			}
			else
			{
				Debug.LogWarning($"Unable to get value of connection {zConnection}");
				//todo: I think this bug is the internal/external connections not being registered correctly?
				return 0;
			}
		}
	}
}