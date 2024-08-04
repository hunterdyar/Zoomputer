using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class SystemNodeView : BaseNodeView
	{
		private SystemNode _systemNode;

		public SystemNodeView(Vector2 pos, ComponentGenerator parent) : base(parent)
		{
			_systemNode = new SystemNode();
			_systemNode.System = parent;
			_systemNode.NodeID = guid;
			SetPosition(new Rect(pos, defaultNodeSize));
			CreateInputPorts();
			CreateOutputPorts();
			RefreshExpandedState();
			RefreshPorts();

		}
		private void CreateInputPorts()
		{
			for (int i = 0; i < _systemNode.System.numberInputs; i++)
			{
				var input = AddPort("Input  " + i.ToString(), Direction.Output, Port.Capacity.Single);
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