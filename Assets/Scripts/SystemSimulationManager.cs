using System;
using UnityEngine;
using Zoompy.Generator;

namespace Zoompy
{
	public class SystemSimulationManager : MonoBehaviour
	{
		[SerializeField] private SystemDisplayManager _displayManager;
		[SerializeField] private ComponentGenerator _outerSystem;

		private ConnectionHub _connectionHub;
		private ZSystem _outermostSystem;//todo: rename

		private void Awake()
		{
			_connectionHub = new ConnectionHub();
		}

		private void Start()
		{
			_outermostSystem = _outerSystem.GetSystem(_connectionHub);
			_displayManager.DisplaySystem(_connectionHub, _outermostSystem);
			//Register Logic
		}
	}
}