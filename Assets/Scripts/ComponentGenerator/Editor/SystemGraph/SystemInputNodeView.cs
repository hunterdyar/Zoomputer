using UnityEditor.Experimental.GraphView;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class SystemInputNodeView : BaseNodeView
	{
		private ComponentGenerator _parent;
		private Port[] _ports;
		public SystemInputNodeView(ComponentGenerator parent) : base(parent)
		{
			this._parent = parent;
			this.name = "Inputs";
			this.title = "Inputs";
			_ports = new Port[parent.numberInputs];
			for (int i = 0; i < parent.numberInputs; i++)
			{
				_ports[i] = AddPort("Input " + i, Direction.Output, Port.Capacity.Multi);
			}
			
			RefreshExpandedState();
			RefreshPorts();
		}
	}
}