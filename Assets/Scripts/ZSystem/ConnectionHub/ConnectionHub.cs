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
		public Dictionary<ZConnection, byte> ConnectionData { get; set; }
		public Dictionary<ZConnection, Action<ZConnection, byte, ConnectionHub>> Listeners { get; set; }
		
		private readonly List<Impulse> _impulses = new List<Impulse>();
		// public Dictionary<ZConnection, IConnectionListener> ConnectionListeners { get; set; }

		private Impulse _currentImpulse;
		private int _connIDCount = 0;
		public void SetAllListeners()
		{
			//loop through all connections and set every listener.
			
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
			if (ConnectionData.ContainsKey(connectionID))
			{ 
				didChange = ConnectionData[connectionID] != newData;
				ConnectionData[connectionID] = newData;
			}
			else
			{
				didChange = true;
				ConnectionData.Add(connectionID, newData);
			}

			//todo: recursion safety check. Check our currentImpulse to see if a state updates to itself.
			_currentImpulse.Mark(connectionID, newData);

			//broadcast the changes.
			if (didChange || !onlyUpdateIfChanged)
			{
				//propogate the state.
				if (Listeners.ContainsKey(connectionID))
				{
					Listeners[connectionID].Invoke(connectionID, newData, this);
				}
			}
		}

		public ZConnection GetConnection()
		{
			_connIDCount++;
			return new ZConnection(_connIDCount);
		}
	}
}