using System;
using UnityEngine;
using Zoompy.Generator;

namespace Zoompy
{
	public class SystemSimulationManager : MonoBehaviour
	{
		[SerializeField] private SystemDisplayManager _displayManager;
		[SerializeField] private ComponentGenerator _outerSystem;

		private ZSystem outermostSystem;

		private void Awake()
		{
			
		}

		private void Start()
		{
			outermostSystem = _outerSystem.GetSystem();
			_displayManager.DisplaySystem(outermostSystem);
		}
	}
}