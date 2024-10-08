﻿using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class SystemNodeView : BaseNodeView
	{
		public SystemNode SystemNode => _systemNode;
		private SystemNode _systemNode;

		public SystemNodeView(SystemNode node, ComponentGenerator parent) : base(parent)
		{
			_systemNode = node;
			Init();
		}

		private void Init()
		{
			this.capabilities = this.capabilities & ~Capabilities.Collapsible;
			if (_systemNode.System == null)
			{
				Debug.LogError("Error, System Node is null! Something is out of sync with serialization, or a leaf node has not been set as 'IsLeaf'");
			}
			this.title = _systemNode.System.name;
			this.name = _systemNode.System.name;
			this.SetID(_systemNode.NodeID);
			SetPosition(new Rect(_systemNode.Position, _systemNode.Size));

			CreateInputPorts();
			CreateOutputPorts();
			RefreshExpandedState();
			RefreshPorts();
		}

		public override void PreSaveDataPopulate()
		{
			var pos = GetPosition();
			_systemNode.Position = pos.position;
			_systemNode.Size = pos.size;
		}

		private void CreateInputPorts()
		{
			for (int i = 0; i < _systemNode.System.numberInputs; i++)
			{
				var input = AddPort("Input  " + i.ToString(), Direction.Input, Port.Capacity.Single);
			}
		}

		private void CreateOutputPorts()
		{
			for (int i = 0; i < _systemNode.System.numberOutputs; i++)
			{
				var output = AddPort("Output  " + i.ToString(), Direction.Output, Port.Capacity.Multi);
			}

		}
		
		
	}
}