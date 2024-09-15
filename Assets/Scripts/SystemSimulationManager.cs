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
			_outermostSystem.Initialize(_connectionHub);

			foreach (var input in _outermostSystem.Inputs)
			{
				_connectionHub.Impulse(input, 0);
			}
			//todo: this calls onChanged from impulse, and then calls it on everyone again.
			// _connectionHub.ForceRefresh();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
			{
				_connectionHub.Impulse(_outermostSystem.Inputs[0], (byte)Mathf.Abs(1-_connectionHub.Get(_outermostSystem.Inputs[0])));
				Debug.Log(_connectionHub.Get(_outermostSystem.Outputs[0]));
			}

			if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
			{
				_connectionHub.Impulse(_outermostSystem.Inputs[1],
					(byte)Mathf.Abs(1 - _connectionHub.Get(_outermostSystem.Inputs[1])));
				Debug.Log(_connectionHub.Get(_outermostSystem.Outputs[0]));

			}

			if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
			{
				_connectionHub.Impulse(_outermostSystem.Inputs[2],
					(byte)Mathf.Abs(1 - _connectionHub.Get(_outermostSystem.Inputs[2])));
				Debug.Log(_connectionHub.Get(_outermostSystem.Outputs[0]));
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				_connectionHub.ForceRefresh();
			}
		}
	}
}