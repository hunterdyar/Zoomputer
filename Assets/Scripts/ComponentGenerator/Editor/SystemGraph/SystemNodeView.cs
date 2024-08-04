using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class SystemNodeView : BaseNodeView
	{
		private SystemNode _systemNode;

		public SystemNodeView(SystemNode node, ComponentGenerator parent) : base(parent)
		{
			_systemNode = node;
			Init();
		}

		private void Init()
		{
			this.title = _systemNode.System.name;
			this.name = _systemNode.System.name;
			
			SetPosition(new Rect(_systemNode.Position, _systemNode.Size));

			CreateInputPorts();
			CreateOutputPorts();
			RefreshExpandedState();
			RefreshPorts();
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