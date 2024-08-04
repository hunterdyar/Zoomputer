using UnityEditor.Experimental.GraphView;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class SystemOutputNodeView : BaseNodeView
	{
		private Port[] _ports;
		public SystemOutputNodeView(ComponentGenerator parent) : base(parent)
		{
			this._parent = parent;
			this.name = "Outputs";
			this.title = "Outputs";
			//remove the following capabilities
			this.capabilities = (this.capabilities & ~Capabilities.Deletable);
			this.capabilities = (this.capabilities & ~Capabilities.Copiable);
			this.capabilities = (this.capabilities & ~Capabilities.Groupable);

			_ports = new Port[parent.numberOutputs];
			for (int i = 0; i < parent.numberOutputs; i++)
			{
				_ports[i] = AddPort("Output "+i.ToString(), Direction.Input, Port.Capacity.Multi);
			}
			RefreshExpandedState();
			RefreshPorts();
		}
	}
}