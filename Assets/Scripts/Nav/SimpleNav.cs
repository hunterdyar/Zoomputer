using System;
using UnityEngine;

namespace Zoompy.Nav
{
	public class SimpleNav : MonoBehaviour
	{
		private ComponentSystem[] _allComponents;

		private void Awake()
		{
			_allComponents = GameObject.FindObjectsOfType<ComponentSystem>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				foreach (var componentSystem in _allComponents)
				{
					componentSystem.ZoomIn();
				}
			}

			else if (Input.GetKeyDown(KeyCode.S))
			{
				foreach (var componentSystem in _allComponents)
				{
					componentSystem.ZoomOut();
				}
			}
		}
	}
}